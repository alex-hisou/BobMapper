using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Resources;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;
using BobMapper.Properties;

namespace BobMapper.Model
{
    public static class MapObjects
    {
        public enum ObjectType
        {
            None,
            Wall,
            Prop,
            NPC,
            PathPoint,
            Floor,
            Misc
        }

        public static ObservableCollection<ObservableCollection<Floor>> FlattenFloors(Floor[][] floors)
        {
            ObservableCollection<ObservableCollection<Floor>> flattenedArray = new ObservableCollection<ObservableCollection<Floor>>();
            foreach (var row in floors)
            {
                var nestedCollection = new ObservableCollection<Floor>();
                foreach (Floor floor in row)
                {
                    Floor newFloor = new Floor("/Resources/FloorTextures/Floor_JunkieTiles.png","");
                    nestedCollection.Add(newFloor);
                }
                flattenedArray.Add(nestedCollection);
            }
            return flattenedArray;
        }

        public static ResourceManager resourceManager = Resources.ResourceManager;

        public static Dictionary<int, Coordinate> houseSizeSchema = new Dictionary<int, Coordinate>()
        {
            { 0, new Coordinate(12,12) }

        };
    }

    public class Wall : IDoublePointObject, INotifyPropertyChanged
    {
        public enum WallType
        {
            Normal,
            Paperthin
        }
        private SnapCoordinate point1;

        public SnapCoordinate Point1
        {
            get { return point1; }
            set {
                point1 = value;
            }
        }
        private SnapCoordinate point2;

        public SnapCoordinate Point2
        {
            get { return point2; }
            set {
                point2 = value;
            }
        }

        public WallType Type { get; set; }

        private string texture1;
        public string Texture1
        {
            get { return texture1; }
            set { texture1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(texture1)));
            }
        }

        private string texture2;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Texture2
        {
            get { return texture2; }
            set { texture2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(texture2)));
            }
        }

        public Wall(SnapCoordinate point1, SnapCoordinate point2, WallType type, string texture2, string texture1)
        {
            Point1 = point1;
            Point2 = point2;
            Type = type;
            Texture2 = texture2;
            Texture1 = texture1;
        }
    }

    public class Door : IDoublePointObject, INotifyPropertyChanged
    {

        private SnapCoordinate point1;
        public SnapCoordinate Point1
        {
            get { return point1; }
            set { point1 = value; }
        }
        private SnapCoordinate point2;
        public SnapCoordinate Point2
        {
            get { return point2; }
            set { point2 = value; }
        }
        private string texture1;
        public string Texture1
        {
            get { return texture1; }
            set { texture1 = value; }
        }
        private string texture2;
        public string Texture2
        {
            get { return texture2; }
            set { texture2 = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Prop : ISinglePointObject, INotifyPropertyChanged
    {
        private int rotation;

        public int Rotation
        {
            get { return rotation; }
            set { rotation = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rotation))); }
        }

        public Coordinate Coordinates { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private string propTexture;
        public string PropTexture
        {
            get { return propTexture; }
            set { propTexture = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PropTexture))); }
        }


        public Prop(Coordinate coordinates, int rotation, string propTextureName)
        {
            Coordinates = coordinates;
            Rotation = rotation;
            PropTexture = propTextureName;
        }

        public void DeleteObject()
        {

        }

        public void UpdatePos(Coordinate newCoordinate)
        {

        }
    }
    public class NPC : ISinglePointObject, INotifyPropertyChanged
    {

        public NPCType Type { get; set; }
        public Coordinate Coordinates { get; set; }

        private int rotation;
        public int Rotation
        {
            get { return rotation; }
            set { rotation = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rotation))); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string texture;
        [JsonIgnore]
        public string Texture
        {
            get { return texture; }
            set { texture = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Texture))); }
        }


        public NPC(Coordinate coordinates, NPCType type, int rotation)
        {
            Coordinates = coordinates;
            Type = type;
            Rotation = rotation;
            switch (type)
            {
                //TODO: Rewrite with aliases and finish
                case NPCType.BulkyCop:
                    Texture = "/Resources/NPCTextures/Guard.png";
                    break;
                case NPCType.BaldCop:
                    Texture = "/Resources/NPCTextures/BaldGuard.png";
                    break;
                case NPCType.RedDressLady:
                    Texture = "/Resources/NPCTextures/Female.png";
                    break;

            }
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

    public class PathPoint : ISinglePointObject, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int? ConnectToPoint { get; set; }

        public Coordinate Coordinates { get; set; }

        [JsonConstructor]
        public PathPoint(Coordinate coordinates, int id, int connectToId)
        {
            Coordinates = coordinates;
            Id = id;
            ConnectToPoint = connectToId;
        }

        public PathPoint(Coordinate coordinates, int id)
        {
            Coordinates = coordinates;
            Id = id;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void DeleteObject()
        {
            throw new NotImplementedException();
        }

        public void UpdatePos(Coordinate newCoordinate)
        {
            throw new NotImplementedException();
        }

        public void UpdateConnectPoint(bool fromTo, PathPoint source)
        {
            throw new NotImplementedException(); 
        }
    }

    public class Floor : INotifyPropertyChanged
    {
        private string texture1;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Texture1
        {
            get { return texture1; }
            set { texture1 = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Texture1))); }

        }

        private string texture2;
        public string Texture2
        {
            get { return texture2; }
            set { texture2 = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Texture2))); }

        }

        public Floor(string texture1, string texture2)
        {
            Texture1 = texture1; Texture2 = texture2;
        }
    }

    public class Misc : ISinglePointObject, INotifyPropertyChanged
    {
        public enum MiscObjects
        {
            Loot = 6,
            MainLoot = 7,
            Key = 8,
            DisguisePoint = 11,
            SoundPoint = 12,
        }

        public MiscObjects Type { get; set; }

        public Coordinate Coordinates { get; set; }

        public Misc(Coordinate coordinates, MiscObjects type)
        {
            Coordinates = coordinates;
            Type = type;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string texture;

        [JsonIgnore]
        public string Texture
        {
            get { return texture; }
            set { texture = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Texture))); }
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
