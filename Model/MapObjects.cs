using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using BobMapper.Properties;

namespace BobMapper.Model
{
    public static class MapObjects
    {
        

        public static ResourceManager resourceManager = Resources.ResourceManager;

        public static Dictionary<int, Coordinate> houseSizeSchema = new Dictionary<int, Coordinate>()
        {
            { 0, new Coordinate(40,40) }

        };
    }

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
        public string Texture1 { get; set; }
        public string Texture2 { get; set; }

        public Wall(Coordinate point1, Coordinate point2, WallType type, string texture2, string texture1)
        {
            Point1 = point1;
            Point2 = point2;
            Type = type;
            Texture2 = texture2;
            Texture1 = texture1;
        }
    }

    public class Prop : IObject
    {
        public int Rotation { get; set; }
        public Coordinate Coordinates { get; set; }

        public string PropTexture { get; set; }
        public bool IsSelected { get; set; }

        public Prop(Coordinate coordinates, int rotation, string propTextureName)
        {
            Coordinates = coordinates;
            Rotation = rotation;
            PropTexture = propTextureName;
            IsSelected = false;
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
        public bool IsSelected { get; set; }

        public NPC(Coordinate coordinates, NPCType type)
        {
            Coordinates = coordinates;
            Type = type;
            IsSelected= false;
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
        public bool IsSelected { get; set; }

        public PathPoint(Coordinate coordinates, int id, int connectFromID, int connectToId)
        {
            Coordinates = coordinates;
            Id = id;
            ConnectFromId = connectFromID;
            ConnectToId = connectToId;
            IsSelected = false;
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

    public struct Floor
    {
        public string Texture1 { get; set; }
        public string Texture2 { get; set; }

        public Floor(string texture1, string texture2)
        {
            Texture1 = texture1; Texture2 = texture2;
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
        public bool IsSelected { get; set; }

        public Misc(Coordinate coordinates, MiscObjects type)
        {
            Coordinates = coordinates;
            Type = type;
            IsSelected = false;
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
}
