using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model;
using BobMapper.Model.MapObjects;

namespace BobMapper.Compiler.WriteSteps
{
    internal class Items_v4
    {
        internal List<byte> itemsOutput;
        internal Items_v4(List<Wall> walls, List<Door> doors, List<Prop> props, List<Loot> loots)
        {
            itemsOutput = new List<byte>();
            itemsOutput.AddRange([0x08, 0x00, 0x00, 0x00]); //SECTION HEAD
            byte[] items_v4 = Encoding.ASCII.GetBytes("Items_v4");
            itemsOutput.AddRange(items_v4);

            List<byte> objectByteBuffer =
            [
                .. WallsAsBytes(walls),
                .. DoorsAsBytes(doors),
                .. PropsAsBytes(props),
                .. LootTexturesAsBytes(loots),
            ];
            //This gets written before the buffer due to the section length bytes
            short sectionLength = Convert.ToInt16(objectByteBuffer.Count);
            itemsOutput.AddRange(BitConverter.GetBytes(sectionLength));
            itemsOutput.AddRange([0x00, 0x00]);

            itemsOutput.AddRange(objectByteBuffer);
        }

        private List<byte> WallsAsBytes(List<Wall> walls)
        {
            List<byte> byteWalls = new List<byte>();
            
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
                CompiledCoordinate compiledPoint1 = new(wall.Point1);
                Array.Copy(compiledPoint1.CompiledBytes, 0, currentByteWall, 4, 4);
                CompiledCoordinate compiledPoint2 = new(wall.Point2);
                Array.Copy(compiledPoint2.CompiledBytes, 0, currentByteWall, 8, 4);
                Encoding.ASCII.GetBytes(wall.InternalTexture1, 0, wall.InternalTexture1.Length, currentByteWall, 12);
                Encoding.ASCII.GetBytes(wall.InternalTexture2, 0, wall.InternalTexture2.Length, currentByteWall, 44);
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
                CompiledCoordinate compiledPoint1 = new(door.Point1);
                Array.Copy(compiledPoint1.CompiledBytes, 0, currentByteDoor, 4, 4);
                CompiledCoordinate compiledPoint2 = new(door.Point2);
                Array.Copy(compiledPoint2.CompiledBytes, 0, currentByteDoor, 8, 4);
                Encoding.ASCII.GetBytes(door.InternalTexture1, 0, door.InternalTexture1.Length, currentByteDoor, 12);
                byteDoors.AddRange(currentByteDoor);
                if (door.Locked)
                {
                    QueuedLocator additionalLocator = new(QueuedLocator.LocatorTypes.Lock, (Coordinate)door.Point1);
                    Compiler.locatorQueue.Add(additionalLocator);
                }
                if (door.PermLocked)
                {
                    QueuedLocator additionalLocator = new(QueuedLocator.LocatorTypes.PermanentLock, (Coordinate)door.Point1);
                    Compiler.locatorQueue.Add(additionalLocator);
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
                Encoding.ASCII.GetBytes(prop.InternalTexture, 0, prop.InternalTexture.Length, currentByteProp, 4);
                _64CompiledCoordinate compiledCoordinate = new(prop.Coordinates, prop.Rotation);
                Array.Copy(compiledCoordinate.CompiledBytes, 0, currentByteProp, 38, 14);
                byteProps.AddRange(currentByteProp);
            }
            return byteProps;
        }

        private List<byte> LootTexturesAsBytes(List<Loot> loots)
        {
            List<byte> byteLootTextures = new List<byte>();
            foreach (Loot loot in loots)
            {
                byte[] currentLootTexture = new byte[48];
                currentLootTexture[0] = 0x35;
                Encoding.ASCII.GetBytes(loot.InternalTexture, 0, loot.InternalTexture.Length, currentLootTexture, 4);
                _64CompiledCoordinate compiledCoordinate = new(loot.Coordinates, loot.Rotation);
                Array.Copy(compiledCoordinate.CompiledBytes, 0, currentLootTexture, 38, 14);
                QueuedLocator queuedLocatorLoot = new(QueuedLocator.LocatorTypes.Loot, loot.Coordinates);
                byteLootTextures.AddRange(currentLootTexture);
            }
            return byteLootTextures;
        }
    }
}
