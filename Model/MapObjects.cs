using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobMapper.Model
{
    public class MapObjects
    {
        
        public class Wall
        {
            public enum WallType
            {
                Normal,
                Door,
                Paperthin,
                Fence
            }
            public Coordinate Point1 { get; set; }
            public Coordinate Point2 { get; set; }
            public WallType Type { get; set; }

            public Wall(Coordinate point1, Coordinate point2, WallType type)
            {
                Point1 = point1;
                Point2 = point2;
                Type = type;
            }
        }

        public class Prop : IObject
        {
            public int Rotation { get; set; }
            public Coordinate Coordinates { get; set; }

            public Prop(Coordinate coordinates, int rotation)
            {
                Coordinates = coordinates;
                Rotation = rotation;
            }

            public void DeleteObject()
            {
                
            }

            public void UpdatePos(Coordinate newCoordinate)
            {
                
            }
        }

        public class NPC : IObject
        {
            
            public NPCType Type { get; set; }
            public Coordinate Coordinates { get; set; }
            public NPC(Coordinate coordinates, NPCType type)
            {
                Coordinates = coordinates;
                Type = type;
            }
            public void UpdatePos(Coordinate newCoordinate)
            {
                throw new NotImplementedException();
            }

            public void DeleteObject()
            {
                throw new NotImplementedException();
            }
            public enum NPCType
            {
                BulkyCop,
                BaldCop,
                RedDressLady,
                RedShirtGuy,
                Grandma,
                Dog,
                Agent,
                Scientist,
                RedDressLady2,
                SkinnyCop,
                BaldCop_Flashlight,
                SecretSam,
                Biff

            }
        }

        public class PathPoint : IObject
        {
            public int Id { get; set; }
            public int ConnectFromId { get; set; }
            public int ConnectToId { get; set; }

            public Coordinate Coordinates { get; set; }

            public PathPoint(Coordinate coordinates, int id, int connectFromID, int connectToId)
            {
                Coordinates = coordinates;
                Id = id;
                ConnectFromId = connectFromID;
                ConnectToId = connectToId;
            }

            public void DeleteObject()
            {
                throw new NotImplementedException();
            }

            public void UpdatePos(Coordinate newCoordinate)
            {
                throw new NotImplementedException();
            }
        }

        public class Misc : IObject
        {
            public enum MiscObjects
            {
                Loot = 6,
                MainLoot = 7,
                Key = 8,
                Lock = 9,
                DisguisePoint = 11,
                SoundPoint = 12,
                PermLock = 13
            }

            public MiscObjects Type { get; set; }

            public Coordinate Coordinates { get; set; }

            public Misc(Coordinate coordinates, MiscObjects type)
            {
                Coordinates = coordinates;
                Type = type;
            }

            public void UpdatePos(Coordinate newCoordinate)
            {
                throw new NotImplementedException();
            }

            public void DeleteObject()
            {
                throw new NotImplementedException();
            }
        }

        public enum Type
        {
            Wall,
            Prop,
            NPC,
            PathPoint,
            Misc
        }

        private static string texture;

        public static string Texture { get { return texture; } set { texture = value; }  }

        private static readonly List<string> textureSchema = new() { "text 1" };
    }
}
