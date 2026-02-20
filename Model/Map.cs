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
        public int Width { get; set; }
        public int Height { get; set; }
        public int levelNumber;
        public Chapter levelChapter;
        public List<Wall> walls = new List<Wall>();
        public List<Prop> props = new List<Prop>();
        public List<NPC> npcs = new List<NPC>();
        public List<PathPoint> pathPoints = new List<PathPoint>();
        public List<Misc> miscs = new List<Misc>();
        public Floor[][] floors;

        

        public Map(int inputHouseSize)
        {
            Width = MapObjects.houseSizeSchema[inputHouseSize].XPos;
            Height = MapObjects.houseSizeSchema[inputHouseSize].YPos;
            floors = new Floor[Width][];
            //System.Text.Json doesnt support multi-d arrays, which is why we do this terribleness
            //And Im too lazy to switch to newtonsoft
            for (int i = 0; i < Height; i++)
            {
                floors[i] = new Floor[Height];
            }
            Width *= Coordinate.FloorSize; 
            Height *= Coordinate.FloorSize;
        }




        [JsonConstructor] //Use only for initialization from json. Otherwise write properties directly using the no param constructor above
        public Map(int inputHouseSize, List<Wall> walls, List<Prop> props, List<NPC> npcs, List<PathPoint> pathPoints, List<Misc> miscs, Floor[][] floors, Chapter chapter, int levelNumber)
        {
            this.walls = walls;
            this.props = props;
            this.npcs = npcs;
            this.pathPoints = pathPoints;
            this.miscs = miscs;
            this.floors = floors;
            this.levelChapter = chapter;
            this.levelNumber = levelNumber;


        }

        public enum Chapter
        {
            Suburbs,    //NOT USED
            Downtown,   //NOT USED
            SecretLabs, //NOT USED
            Advanced,
            Winter,
            HighRise,
            SummerCamp,
            Bonus,
            Extras,
            Challenge
        }
        
    }
}
