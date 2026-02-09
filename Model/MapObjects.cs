using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobMapper.Model
{
    internal class MapObjects
    {
        internal class Wall
        {
            internal enum WallType
            {
                Normal,
                Door,
                Paperthin,
                Fence
            }
            internal Coordinate point1;
            internal Coordinate point2;
            internal WallType type;

            internal Wall(Coordinate point1, Coordinate point2, WallType type)
            {
                this.point1 = point1;
                this.point2 = point2;
                this.type = type;
            }
        }

        internal class Prop : IObject
        {
            internal Coordinate coordinates;
            internal int rotation;
            public Coordinate Coordinates { get; set; }

            internal Prop(Coordinate coordinates, int rotation)
            {
                this.coordinates = coordinates;
                this.rotation = rotation;
            }

            public void DeleteObject()
            {
                
            }

            public void UpdatePos(Coordinate newCoordinate)
            {
                
            }
        }

        internal class NPC : IObject
        {
            
            internal NPCType type;
            internal Coordinate coordinates;
            public Coordinate Coordinates { get; set; }
            internal NPC(Coordinate coordinates, NPCType npcType)
            {
                this.coordinates = coordinates;
                this.type = npcType;
            }
            public void UpdatePos(Coordinate newCoordinate)
            {
                throw new NotImplementedException();
            }

            public void DeleteObject()
            {
                throw new NotImplementedException();
            }
            internal enum NPCType
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

        internal class PathPoint : IObject
        {
            internal Coordinate coordinates;
            internal int id;
            internal int connectFromId;
            internal int connectToId;

            public Coordinate Coordinates { get; set; }

            internal PathPoint(Coordinate coordinates, int id, int connectFromID, int connectToId)
            {
                this.coordinates = coordinates;
                this.id = id;
                this.connectFromId = connectFromID;
                this.connectToId = connectToId;
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

        internal class Misc : IObject
        {
            internal enum MiscObjects
            {
                Loot = 6,
                MainLoot = 7,
                Key = 8,
                Lock = 9,
                DisguisePoint = 11,
                SoundPoint = 12,
                PermLock = 13
            }

            internal MiscObjects type;
            internal Coordinate coordinates;

            public Coordinate Coordinates { get; set; }

            internal Misc(Coordinate coordinates, MiscObjects type)
            {
                this.coordinates = coordinates;
                this.type = type;
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

        internal enum Type
        {
            Wall,
            Prop,
            NPC,
            PathPoint,
            Misc
        }
    }
}
