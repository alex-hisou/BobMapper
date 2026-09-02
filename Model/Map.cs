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
        public List<ExitZone> exitZones = new List<ExitZone>();
        public List<Cable> cables = new List<Cable>();
        public Floor[][] floors;
        public MapProperties mapProperties;

        public Map(MapProperties mapProperties)
        {
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
        public Map(List<Wall> walls, List<Prop> props, List<NPC> npcs, List<PathPoint> pathPoints, List<Misc> miscs, List<Loot> loots, Floor[][] floors, List<Door> doors, MapProperties mapProperties, List<ExitZone> exitZones, List<Cable> cables)
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
            this.exitZones = exitZones;
            this.cables = cables;
        }

        public enum Chapter
        {
            Suburbs,
            Downtown,
            SecretLabs,
            Advanced,
            Winter,
            Highrise,
            SummerCamp,
            Bonus,
            Extras,
            Challenge
        }

        public void ExpandOrContractMap(int northOffset, int southOffset, int eastOffset, int westOffset)
        {
            int netVerticalOffset = northOffset + southOffset;
            int netHorizontalOffset = westOffset + eastOffset;
            int snapHeight = mapProperties.Height / 64;
            int snapWidth = mapProperties.Width / 64;
            int newFloorHeight = snapHeight + netVerticalOffset;
            int newFloorWidth = snapWidth + netHorizontalOffset;
            Floor[][] newFloor = new Floor[newFloorWidth][];
            for (int i = 0 - westOffset; i < snapWidth + eastOffset; i++)
            {
                int adjustedI = i + westOffset;
                newFloor[adjustedI] = new Floor[newFloorHeight];
                for (int j = 0 - southOffset;  j < snapHeight + northOffset; j++)
                {
                    int adjustedJ = j + southOffset;
                    if (j < 0 || j >= snapHeight || i < 0 || i >= snapWidth)
                    {
                        newFloor[adjustedI][adjustedJ] = new Floor(@"/Resources/FloorTextures/Floor_Nothing.png", @"/Resources/FloorTextures/Floor_Nothing.png", 0);
                        newFloor[adjustedI][adjustedJ].SetOpacity(mapProperties.IsApartment);
                    }
                    else
                    {
                        newFloor[adjustedI][adjustedJ] = floors[i][j];
                    }
                }
            }
            mapProperties.Height = newFloorHeight * 64;
            mapProperties.Width = newFloorWidth * 64;
            floors = newFloor;
            MapSizeChanged?.Invoke(this, new(northOffset, eastOffset, westOffset, southOffset));
        }

        public event EventHandler<MapSizeChangedEventArgs> MapSizeChanged;

        public static Array TextureTypeValues => Enum.GetValues(typeof(TextureType));

    }
}
