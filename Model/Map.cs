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
        public int Width { get; set; }
        public int Height { get; set; }
        public int levelNumber;
        public Chapter levelChapter;
        public List<Wall> walls = new List<Wall>();
        public List<Door> doors = new List<Door>();
        public List<Prop> props = new List<Prop>();
        public List<NPC> npcs = new List<NPC>();
        public List<PathPoint> pathPoints = new List<PathPoint>();
        public List<Misc> miscs = new List<Misc>();
        public Floor[][] floors;
        public Tilesets tileset;

        public Map(int inputHouseSize)
        {
            Width = MapManager.houseSizeSchema[inputHouseSize].XPos;
            Height = MapManager.houseSizeSchema[inputHouseSize].YPos;
            floors = new Floor[Width][];
            //System.Text.Json doesnt support multi-d arrays, which is why we do this terribleness
            //And Im too lazy to switch to newtonsoft
            for (int i = 0; i < Height; i++)
            {
                floors[i] = new Floor[Height];
            }
            Width *= SnapCoordinate.FloorSize; 
            Height *= SnapCoordinate.FloorSize;
            //FOR TESTING ONLY, DELETE LATER
            tileset = Tilesets.Downtown;
        }

        internal void AttachAllPathPointHandlers()
        {
            pathPoints.ForEach(x => x.ConnectionPointChanged += FillPathPointConnectCoordinate);
        }

        public void FillPathPointConnectCoordinate(object sender, EventArgs e)
        {
            PathPoint SelectedPathPoint = (PathPoint)sender;
            if (pathPoints.Any(x => x.Id == SelectedPathPoint.ConnectToId))
            {
                Coordinate ConnectionCoordinate = pathPoints.FirstOrDefault(x => x.Id == SelectedPathPoint.ConnectToId).Coordinates;
                SelectedPathPoint.AbsoluteLineConnectionCoordinates = ConnectionCoordinate;
            }
            else { SelectedPathPoint.LineConnectionCoordinate = SelectedPathPoint.Coordinates; SystemSounds.Exclamation.Play(); }

        }


        [JsonConstructor] //Use only for initialization from json. Otherwise write properties directly using the no param constructor above
        public Map(List<Wall> walls, List<Prop> props, List<NPC> npcs, List<PathPoint> pathPoints, List<Misc> miscs, Floor[][] floors, Chapter levelChapter, int levelNumber, Tilesets tileset, List<Door> doors, int Width, int Height)
        {
            this.walls = walls;
            this.props = props;
            this.npcs = npcs;
            this.pathPoints = pathPoints;
            this.miscs = miscs;
            this.floors = floors;
            this.levelChapter = levelChapter;
            this.levelNumber = levelNumber;
            this.tileset = tileset;
            this.doors = doors;
            this.Width = Width;
            this.Height = Height;
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
