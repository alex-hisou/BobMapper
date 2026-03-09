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
        internal void Compile(Map map)
        {
            byte[] fileHeader = [0x01, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00];
            output.AddRange(fileHeader);
            output.AddRange(CablesAsBytes());
            output.AddRange(FloorAsBytes(map.floors));
            output.AddRange(WallsAsBytes(map.walls));
            output.AddRange(DoorsAsBytes(map.doors));
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
                byte[] byteWall = new byte[76];
                switch (wall.Type)
                {
                    case Wall.WallType.Normal:
                        byteWall[0] = 0x33;
                        break;
                    case Wall.WallType.Paperthin:
                        byteWall[0] = 0x31;
                        byteWall[1] = 0x33;
                        break;
                }

            }
            return byteWalls;
        }

        private List<byte> DoorsAsBytes(List<Door> doors)
        {

        }
    }
}
