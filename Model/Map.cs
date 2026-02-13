using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BobMapper.Model;
using BobMapper.Properties;

namespace BobMapper.Model
{
    public class Map
    {
        public double Width;
        public double Height;
        public List<Wall> walls = new List<Wall>();
        public List<Prop> props = new List<Prop>();
        public List<NPC> npcs = new List<NPC>();
        public List<PathPoint> pathPoints = new List<PathPoint>();
        public List<Misc> miscs = new List<Misc>();
        public Floor[,] floors;

        

        public Map(int inputHouseSize)
        {
            Width = MapObjects.houseSizeSchema[inputHouseSize].x;
            Height = MapObjects.houseSizeSchema[inputHouseSize].y;
        }




        [JsonConstructor] //Use only for initialization from json. Otherwise write properties directly using the no param constructor above
        public Map(int inputHouseSize, List<Wall> walls, List<Prop> props, List<NPC> npcs, List<PathPoint> pathPoints, List<Misc> miscs, Floor[,] floors)
        {
            this.walls = walls;
            this.props = props;
            this.npcs = npcs;
            this.pathPoints = pathPoints;
            this.miscs = miscs;
            this.floors = floors;



        }

        
    }
}
