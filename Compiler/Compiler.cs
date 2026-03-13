using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model;
using BobMapper.Model.MapObjects;

namespace BobMapper.Compiler
{
    internal class Compiler
    {
        private List<byte> output;
        private List<QueuedLocator> locatorQueue;
        internal void Compile(Map map)
        {
            byte[] fileHeader = [0x01, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00];
            output.AddRange(fileHeader);
            output.AddRange(CablesAsBytes());
            output.AddRange(FloorAsBytes(map.floors));
            output.AddRange(WallsAsBytes(map.walls));
            output.AddRange(DoorsAsBytes(map.doors));
            output.AddRange(PropsAsBytes(map.props));
        }

        private List<byte> CablesAsBytes()
        {
            List<byte> ByteCables = new List<byte>();
            return ByteCables;
        }

        private List<byte> FloorAsBytes(Floor[][] floors)
        {
            List<byte> byteFloors = new List<byte>();
            byte[] floors_v3 = Encoding.ASCII.GetBytes("Floors_v3");
            byteFloors.AddRange(floors_v3);
            byteFloors.AddRange([0x00, 0x00]);
            byte width = Convert.ToByte(floors[0].Length);
            byte height = Convert.ToByte(floors.Length);
            byteFloors.AddRange([height, 0x00, 0x00, 0x00, width, 0x00, 0x00, 0x00]);
            foreach(var floorRow in floors)
            {
                foreach(Floor floor in floorRow)
                {
                    byte[] byteTexture1 = new byte[24];
                    Encoding.ASCII.GetBytes(floor.Texture1, 0, floor.Texture1.Length, byteTexture1, 0);
                    byteFloors.AddRange(byteTexture1);
                    byte[] byteTexture2 = new byte[24];
                    Encoding.ASCII.GetBytes(floor.Texture2, 0, floor.Texture2.Length, byteTexture1, 0);
                    byteFloors.AddRange(byteTexture2);
                }
            }
            return byteFloors;
        }
        //TODO: Double check EVERYTHING once done
        private List<byte> WallsAsBytes(List<Wall> walls)
        {
            List<byte> byteWalls = new List<byte>();
            byte[] items_v4 = Encoding.ASCII.GetBytes("Items_v4");
            byteWalls.AddRange(items_v4);
            //first two are mystery bytes (filled in 00 for now)
            byteWalls.AddRange([0x00, 0x00, 0x00, 0x00]);
            foreach (Wall wall in walls)
            {
                byte[] currentByteWall = new byte[76];
                switch (wall.Type)
                {
                    case Wall.WallType.Normal:
                        currentByteWall[0] = 0x33;
                        currentByteWall[1] = 0x00;
                        break;
                    case Wall.WallType.Paperthin:
                        currentByteWall[0] = 0x31;
                        currentByteWall[1] = 0x33;
                        break;
                }
                currentByteWall[2] = 0x00;
                currentByteWall[3] = 0x00;
                CompiledCoordinate compiledPoint1 = new(wall.Point1);
                currentByteWall[4] = compiledPoint1.CompiledX[0];
                currentByteWall[5] = compiledPoint1.CompiledX[1];
                currentByteWall[6] = compiledPoint1.CompiledY[0];
                currentByteWall[7] = compiledPoint1.CompiledY[1];
                CompiledCoordinate compiledPoint2 = new(wall.Point2);
                currentByteWall[8] = compiledPoint2.CompiledX[0];
                currentByteWall[9] = compiledPoint2.CompiledX[1];
                currentByteWall[10] = compiledPoint2.CompiledY[0];
                currentByteWall[11] = compiledPoint2.CompiledY[1];
                Encoding.ASCII.GetBytes(wall.Texture1, 0, wall.Texture1.Length, currentByteWall, 12);
                Encoding.ASCII.GetBytes(wall.Texture2, 0, wall.Texture2.Length, currentByteWall, 44);
                byteWalls.AddRange(currentByteWall);

            }
            return byteWalls;
        }

        private List<byte> DoorsAsBytes(List<Door> doors)
        {
            List<byte> byteDoors = new List<byte>();
            foreach (Door door in doors)
            {
                byte[] currentByteDoor = new byte[45];
                currentByteDoor[0] = 0x34;
                Array.Fill<byte>(currentByteDoor, 0x00, 1, 3);
                CompiledCoordinate compiledPoint1 = new(door.Point1);
                currentByteDoor[4] = compiledPoint1.CompiledX[0];
                currentByteDoor[5] = compiledPoint1.CompiledX[1];
                currentByteDoor[6] = compiledPoint1.CompiledY[0];
                currentByteDoor[7] = compiledPoint1.CompiledY[1];
                CompiledCoordinate compiledPoint2 = new(door.Point2);
                currentByteDoor[8] = compiledPoint2.CompiledX[0];
                currentByteDoor[9] = compiledPoint2.CompiledX[1];
                currentByteDoor[10] = compiledPoint2.CompiledY[0];
                currentByteDoor[11] = compiledPoint2.CompiledY[1];
                Encoding.ASCII.GetBytes(door.Texture1, 0, door.Texture1.Length, currentByteDoor, 12);
                byteDoors.AddRange(currentByteDoor);
                if(door.Locked)
                {
                    //TODO: Write SnapCoordinateConverter
                    QueuedLocator additionalLocator = new(QueuedLocator.LocatorTypes.Lock, door.Point1);
                    locatorQueue.Add(additionalLocator);
                }
                if(door.PermLocked)
                {
                    QueuedLocator additionalLocator = new(QueuedLocator.LocatorTypes.PermanentLock, door.Point1);
                    locatorQueue.Add(additionalLocator);
                }
            }
            return byteDoors;
        }

        private List<byte> PropsAsBytes(List<Prop> props)
        {
            List<byte> byteProps = new List<byte>();
            foreach (Prop prop in props)
            {
                byte[] currentByteProp = new byte[48];
                currentByteProp[0] = 0x35;
                Array.Fill<byte>(currentByteProp, 0x00, 1, 3);
                Encoding.ASCII.GetBytes(prop.PropTexture, 0, prop.PropTexture.Length, currentByteProp, 4);
                _64CompiledCoordinate compiledCoordinate = new(prop.Coordinates, prop.Rotation);
                currentByteProp[38] = compiledCoordinate.CompiledX[0];
                currentByteProp[39] = compiledCoordinate.CompiledX[1];
                currentByteProp[42] = compiledCoordinate.CompiledY[0];
                currentByteProp[43] = compiledCoordinate.CompiledY[1];
                currentByteProp[47] = compiledCoordinate.CompiledRotation;
                byteProps.AddRange(currentByteProp);
            }
            return byteProps;
        }
    }
}
