using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobMapper.Model
{
    internal class Map
    {
        internal int houseSize;
        internal List<MapObjects.Wall> walls = new List<MapObjects.Wall>();
        internal List<MapObjects.Prop> props = new List<MapObjects.Prop>();
        internal List<MapObjects.NPC> npcs = new List<MapObjects.NPC>();
        internal List<MapObjects.PathPoint> pathPoints = new List<MapObjects.PathPoint>();
        internal List<MapObjects.Misc> miscs = new List<MapObjects.Misc>();

        internal Map() 
        {
            
        }
    }
}
