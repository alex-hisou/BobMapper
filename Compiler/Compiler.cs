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
        internal static List<byte> output = new List<byte>();
        internal static List<QueuedLocator> locatorQueue = new List<QueuedLocator>();
        internal void Compile(Map map)
        {
            byte[] fileHeader = [0x01, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00];
            output.AddRange(fileHeader);
            //output.AddRange(CablesAsBytes());
            output.AddRange(FloorAsBytes(map.floors));

            Items_v4 items_V4 = new(map.walls, map.doors, map.props, map.loots);
            output.AddRange(items_V4.itemsOutput);

            output.AddRange(Level_v2(map.Width / SnapCoordinate.FloorSize, map.Height / SnapCoordinate.FloorSize, map.tileset));

            Locators_v3 locators_V3 = new(map.npcs, map.pathPoints, map.miscs);
            output.AddRange(locators_V3.locatorsOutput);

            NavMesh navMesh = new NavMesh(map.Width / SnapCoordinate.FloorSize, map.Height / SnapCoordinate.FloorSize, map.walls, map.doors, map.props);
            output.AddRange(navMesh.navMeshOutput);

            output.AddRange(RoomGeometry());
            output.AddRange(Zones());


        }

        private List<byte> CablesAsBytes()
        {
            List<byte> ByteCables = new List<byte>();
            return ByteCables;
        }

        private List<byte> FloorAsBytes(Floor[][] floors)
        {
            List<byte> byteFloors = new List<byte>();
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
                    byteTexture2[25] = 0x01;
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
            Array.Copy(BitConverter.GetBytes((int)tileset), 0, level_v2, 24, 4);
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

        private List<byte> Zones()
        {
            //TODO: Figure out the purpose of this section
            List<byte> zones = new List<byte>();
            zones.AddRange([0x0C, 0x00, 0x00, 0x00]); //SECTION HEAD
            byte[] zonesText = Encoding.ASCII.GetBytes("Zones");
            zones.AddRange(zonesText);
            byte[] emptyContent = new byte[4]; 
            zones.AddRange(BitConverter.GetBytes(emptyContent.Length));
            zones.AddRange(emptyContent);
            return zones;
        }

        
    }
}
