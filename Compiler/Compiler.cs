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
        internal static List<byte> output;
        internal static List<QueuedLocator> locatorQueue;

        //
        internal void Compile(Map map)
        {
            byte[] fileHeader = [0x01, 0x00, 0x00, 0x00, 0x08, 0x00, 0x00, 0x00];
            output.AddRange(fileHeader);
            output.AddRange(CablesAsBytes());
            output.AddRange(FloorAsBytes(map.floors));

            Items_v4 items_V4 = new(map.walls, map.doors, map.props, map.loots);
            output.AddRange(items_V4.itemsOutput);

            output.AddRange(Level_v2(map.Width, map.Height));
            Locators_v3 locators_V3 = new(map.npcs, map.pathPoints, map.miscs);
            output.AddRange(locators_V3.locatorsOutput);
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
                    Encoding.ASCII.GetBytes(floor.InternalTexture1, 0, floor.InternalTexture1.Length, byteTexture1, 0);
                    byteFloors.AddRange(byteTexture1);
                    byte[] byteTexture2 = new byte[24];
                    Encoding.ASCII.GetBytes(floor.InternalTexture2, 0, floor.InternalTexture2.Length, byteTexture1, 0);
                    byteFloors.AddRange(byteTexture2);
                }
            }
            return byteFloors;
        }
        //TODO: Double check EVERYTHING once done
        

        private byte[] Level_v2(int width, int  height)
        {
            byte[] level_v2 = new byte[33];
            level_v2[0] = 0x08;
            Encoding.ASCII.GetBytes("Level_v2", 0, level_v2.Length, level_v2, 4);
            level_v2[12] = 0x14; //no idea why, but its needed
            level_v2[16] = Convert.ToByte(width);
            level_v2[20] = Convert.ToByte(height);
            level_v2[24] = 0x01; //Mystery byte
            level_v2[32] = 0x01;
            return level_v2;
        }

        
    }
}
