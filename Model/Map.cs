using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BobMapper.Model
{
    public class Map
    {
        public int houseSize;
        public List<MapObjects.Wall> walls = new List<MapObjects.Wall>();
        public List<MapObjects.Prop> props = new List<MapObjects.Prop>();
        public List<MapObjects.NPC> npcs = new List<MapObjects.NPC>();
        public List<MapObjects.PathPoint> pathPoints = new List<MapObjects.PathPoint>();
        public List<MapObjects.Misc> miscs = new List<MapObjects.Misc>();

        public Map()
        {
            
        }

        
        [JsonConstructor]
        public Map(List<MapObjects.Wall> walls, List<MapObjects.Prop> props, List<MapObjects.NPC> npcs, List<MapObjects.PathPoint> pathPoints, List<MapObjects.Misc> miscs)
        {
            this.walls = walls;
            this.props = props;
            this.npcs = npcs;
            this.pathPoints = pathPoints;
            this.miscs = miscs;
        }
        
    }
}
