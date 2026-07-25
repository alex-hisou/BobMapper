using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BobMapper.Properties;
using BobMapper.Model;
using BobMapper.Model.MapObjects;
using System.Media;

namespace BobMapper.Model
{
    public class Map
    {
        public List<Wall> walls = new List<Wall>();
        public List<Door> doors = new List<Door>();
        public List<Prop> props = new List<Prop>();
        public List<NPC> npcs = new List<NPC>();
        public List<PathPoint> pathPoints = new List<PathPoint>();
        public List<Misc> miscs = new List<Misc>();
        public List<Loot> loots = new List<Loot>();
        public Floor[][] floors;
        public MapProperties mapProperties;

        public Map(int sizeX, int sizeY, Tilesets tileset)
        {
            MapProperties mapProperties = new(sizeX, sizeY, tileset);
            this.mapProperties = mapProperties;
            floors = new Floor[mapProperties.Width][];
            //System.Text.Json doesnt support multi-d arrays, which is why we do this terribleness
            //And Im too lazy to switch to newtonsoft
            for (int i = 0; i < mapProperties.Width; i++)
            {
                floors[i] = new Floor[mapProperties.Height];
                for (int j = 0; j < mapProperties.Height; j++)
                {
                    floors[i][j] = new Floor(@"/Resources/FloorTextures/Floor_Nothing.png", @"/Resources/FloorTextures/Floor_Nothing.png", 0);
                }
            }
            mapProperties.Width *= SnapCoordinate.FloorSize; 
            mapProperties.Height *= SnapCoordinate.FloorSize;
        }

        


        [JsonConstructor] //Use only for initialization from json. Otherwise write properties directly using the no param constructor above
        public Map(List<Wall> walls, List<Prop> props, List<NPC> npcs, List<PathPoint> pathPoints, List<Misc> miscs, List<Loot> loots, Floor[][] floors, List<Door> doors, MapProperties mapProperties)
        {
            this.walls = walls;
            this.props = props;
            this.npcs = npcs;
            this.pathPoints = pathPoints;
            this.miscs = miscs;
            this.loots = loots;
            this.floors = floors;
            this.mapProperties = mapProperties;
            this.doors = doors;
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
        
        public static Array TextureTypeValues => Enum.GetValues(typeof(TextureType));

    }
}
