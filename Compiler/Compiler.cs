using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Compiler.WriteSteps;
using BobMapper.Model;
using BobMapper.Model.MapObjects;

namespace BobMapper.Compiler
{
    internal class Compiler
    {
        internal List<byte> output = new List<byte>();
        internal List<QueuedLocator> locatorQueue = new List<QueuedLocator>();
        internal void Compile(Map map)
        {
            List<Wall> unsplitWalls = SplitWalls(map.walls);
            byte[] fileHeader = [0x01, 0x00, 0x00, 0x00];
            output.AddRange(fileHeader);
            if(map.cables.Count > 0)
                output.AddRange(CablesAsBytes(map.cables));

            output.AddRange(FloorAsBytes(map.floors));

            Items_v4 items_V4 = new(unsplitWalls, map.doors, map.props, map.loots, this);
            output.AddRange(items_V4.itemsOutput);

            output.AddRange(Level_v2(map.mapProperties.Width / SnapCoordinate.FloorSize, map.mapProperties.Height / SnapCoordinate.FloorSize, map.mapProperties.Tileset));

            Locators_v3 locators_V3 = new(map.npcs, map.pathPoints, map.miscs, this);
            output.AddRange(locators_V3.locatorsOutput);

            NavMesh navMesh = new NavMesh(map.mapProperties.Width / SnapCoordinate.FloorSize, map.mapProperties.Height / SnapCoordinate.FloorSize, 
                map.walls, map.doors, map.props, map.mapProperties.AutomaticExitZones);
            output.AddRange(navMesh.navMeshOutput);

            output.AddRange(RoomGeometry());
            output.AddRange(Zones(map.exitZones));


        }

        private List<Wall> SplitWalls(List<Wall> unsplitWalls)
        {
            List<Wall> splitWalls = new();
            foreach (Wall wall in unsplitWalls)
            {
                double xdiff = wall.Point2.SnappedXPos - wall.Point1.SnappedXPos;
                double ydiff = wall.Point2.SnappedYPos - wall.Point1.SnappedYPos;
                bool isHorizontal = ydiff == 0;
                bool isVertical = xdiff == 0;
                bool isDiagonal = Math.Abs(xdiff) == Math.Abs(ydiff);
                if (!isHorizontal && !isVertical && !isDiagonal)
                {
                    splitWalls.Add(wall);
                    continue;
                }
                double length = Math.Max(Math.Abs(xdiff), Math.Abs(ydiff));
                if (length <= 1)
                {
                    splitWalls.Add(wall);
                    continue;
                }
                float xSign = Math.Sign(xdiff);
                float ySign = Math.Sign(ydiff);
                for (int i = 0; i < length; i++)
                {
                    float currX = (float)wall.Point1.SnappedXPos + i * xSign;
                    float currY = (float)wall.Point1.SnappedYPos + i * ySign;
                    float nextX = currX + xSign;
                    float nextY = currY + ySign;
                    SnapCoordinate point1 = new(currX, currY);
                    SnapCoordinate point2 = new(nextX, nextY);
                    splitWalls.Add(new(point1, point2, wall.Type, wall.Texture2, wall.Texture1));
                }
            }
            return splitWalls;
        }

        private List<byte> CablesAsBytes(List<Cable> cables)
        {
            List<byte> cablesOutput = new List<byte>();
            byte[] sectionHeader = [0x06, 0x00, 0x00, 0x00];
            cablesOutput.AddRange(sectionHeader);
            byte[] cablesText = Encoding.ASCII.GetBytes("Cables");
            cablesOutput.AddRange(cablesText);
            List<byte> cablesByteBuffer = new();
            int cablesCount = Convert.ToInt32(cables.Count);
            cablesByteBuffer.AddRange(BitConverter.GetBytes(cablesCount));
            foreach (Cable cable in cables)
            {
                List<byte> currentCableOutput = new();
                //Regex out #
                byte[] hexBytes = Encoding.ASCII.GetBytes(cable.ColourHex);
                currentCableOutput.AddRange(hexBytes);
                byte[] cableSegmentHeader = [0x0A, 0xD7, 0xA3, 0x3D];
                currentCableOutput.AddRange(cableSegmentHeader);
                int cableNodesCount = Convert.ToInt32(cable.Coordinates.Count);
                currentCableOutput.AddRange(BitConverter.GetBytes(cableNodesCount));
                foreach(SnapCoordinate coordinate  in cable.Coordinates)
                {
                    FloatCoordinate floatCoordinate = new(coordinate);
                    currentCableOutput.AddRange(floatCoordinate.CompiledBytes);
                }
            }
            int sectionLength = Convert.ToInt32(cablesByteBuffer.Count);
            cablesOutput.AddRange(BitConverter.GetBytes(sectionLength));
            return cablesOutput;
        }

        private List<byte> FloorAsBytes(Floor[][] floors)
        {
            List<byte> byteFloors = new List<byte>();
            byte[] sectionHeader = [0x08, 0x00, 0x00, 0x00];
            byteFloors.AddRange(sectionHeader);
            byte[] floors_v3 = Encoding.ASCII.GetBytes("Floor_v3");
            byteFloors.AddRange(floors_v3);
            
            List<byte> floorByteBuffer = new List<byte>();
            byte[] byteWidth = BitConverter.GetBytes(floors[0].Length);
            byte[] byteHeight = BitConverter.GetBytes(floors.Length);
            floorByteBuffer.AddRange(byteHeight);
            floorByteBuffer.AddRange(byteWidth);
            for (int i = 0; i < floors[0].Length; i++)
            {
                for (int j = 0; j < floors.Length; j++)
                {
                    Floor floor = floors[j][i];
                    byte[] byteTexture1 = new byte[24];
                    Encoding.ASCII.GetBytes(floor.InternalTexture1, 0, floor.InternalTexture1.Length, byteTexture1, 0);
                    floorByteBuffer.AddRange(byteTexture1);
                    //TODO: Make Texture2 work
                    byte[] byteTexture2 = new byte[26];
                    Encoding.ASCII.GetBytes(floor.InternalTexture1, 0, floor.InternalTexture1.Length, byteTexture2, 0);
                    byteTexture2[25] = (byte)floor.Flip;
                    floorByteBuffer.AddRange(byteTexture2);
                }
            }
            byteFloors.AddRange(BitConverter.GetBytes(floorByteBuffer.Count));
            byteFloors.AddRange(floorByteBuffer);
            return byteFloors;
        }
        

        private byte[] Level_v2(int width, int  height, Tilesets tileset)
        {
            byte[] level_v2 = new byte[36];
            level_v2[0] = 0x08;
            string level_v2Text = "Level_v2";
            byte[] level_v2TextBytes = Encoding.ASCII.GetBytes(level_v2Text, 0, level_v2Text.Length);
            Array.Copy(level_v2TextBytes, 0, level_v2, 4, level_v2TextBytes.Length);
            level_v2[12] = 0x14; //buffer length
            Array.Copy(BitConverter.GetBytes(width), 0, level_v2, 16, 4);
            Array.Copy(BitConverter.GetBytes(height), 0, level_v2, 20, 4);
            int ingametilesetindex = (int)tileset;
            if (tileset == Tilesets.Winter)
                ingametilesetindex = 5;
            if (tileset == Tilesets.Camp)
                ingametilesetindex = 7;
            Array.Copy(BitConverter.GetBytes(ingametilesetindex), 0, level_v2, 24, 4);
            level_v2[32] = 0x01;
            return level_v2;
        }

        private List<byte> RoomGeometry()
        {
            List<byte> roomGeometry = new List<byte>();
            roomGeometry.AddRange([0x0C, 0x00, 0x00, 0x00]); //SECTION HEAD
            byte[] roomGeometryText = Encoding.ASCII.GetBytes("RoomGeometry");
            roomGeometry.AddRange(roomGeometryText);
            byte[] emptyContent = new byte[8];
            roomGeometry.AddRange(BitConverter.GetBytes(emptyContent.Length));
            roomGeometry.AddRange(emptyContent);
            return roomGeometry;
        }

        private List<byte> Zones(List<ExitZone> exitZones)
        {
            //TODO: Figure out the purpose of this section
            List<byte> zones = new List<byte>();
            zones.AddRange([0x05, 0x00, 0x00, 0x00]); //SECTION HEAD
            byte[] zonesText = Encoding.ASCII.GetBytes("Zones");
            zones.AddRange(zonesText);
            List<byte> byteContentZones = new List<byte>();
            foreach (var zone in exitZones)
            {
                byte[] byteZone = new byte[10];
                CompiledCoordinate coordinate1 = new(zone.Point1);
                Array.Copy(coordinate1.CompiledBytes, 0, byteZone, 0, 4);
                CompiledCoordinate coordinate2 = new(zone.Point3);
                Array.Copy(coordinate2.CompiledBytes, 0, byteZone, 4, 4);
                byteZone[8] = 0x03;
                byteZone[9] = 0x00;
                byteContentZones.AddRange(byteZone);

            }
            if(byteContentZones.Count == 0)
            {
                byteContentZones.AddRange([0x00, 0x00, 0x00, 0x00]);
            }
            zones.AddRange(BitConverter.GetBytes(Convert.ToInt32(byteContentZones.Count)));
            zones.AddRange(byteContentZones);
            return zones;
        }

        
    }
}
