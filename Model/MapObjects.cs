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

            public void DeleteObject()
            {
                
            }

            public void UpdatePos(Coordinate newCoordinate)
            {
                
            }
        }

        internal class NPC : IObject
        {
            internal enum NPCType
            {
                BulkyCop,
                BaldCop,
                RedDresslady,
                RedShirtGuy,
                Grandma,
                Dog,
                Agent,
                Scientist,
                RedShirtLady2,
                SkinnyCop,
                BaldCop_Flashlight,
                SecretSam,
                Biff

            }
            internal NPCType type;
            internal Coordinate coordinates;
            public Coordinate Coordinates { get; set; }

            public void UpdatePos(Coordinate newCoordinate)
            {
                throw new NotImplementedException();
            }

            public void DeleteObject()
            {
                throw new NotImplementedException();
            }
        }

        internal class PathPoint : IObject
        {
            internal Coordinate coordinates;
            internal int id;
            internal int connectFromId;
            internal int connectToId;

            public Coordinate Coordinates { get; set; }

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
