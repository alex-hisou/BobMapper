using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using BobMapper.Services;

namespace BobMapper.Model.MapObjects
{
    public class Wall : IDoublePointObject, INotifyPropertyChanged
    {
        public enum WallType
        {
            Normal,
            Paperthin
        }

        [JsonIgnore]
        public static Array WallTypeValues => Enum.GetValues(typeof(WallType));

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

        private WallType type;
        public WallType Type
        {
            get { return type; }
            set
            {
                type = value;
                if (value == WallType.Normal)
                {
                    Width = 20;
                }
                else { Width = 5; }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(type)));
            }
        }

        private string texture1;
        public string Texture1
        {
            get { return texture1; }
            set { texture1 = value; InternalTexture1 = InternalNameSevice.GetInternalName(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(texture1)));
            }
        }

        private string internalTexture1;
        [JsonIgnore]
        public string InternalTexture1
        {
            get { return internalTexture1; }
            set { internalTexture1 = value; }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private string texture2;

        public string Texture2
        {
            get { return texture2; }
            set { texture2 = value; InternalTexture2 = InternalNameSevice.GetInternalName(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(texture2)));
            }
        }

        private string internalTexture2;
        [JsonIgnore]
        public string InternalTexture2
        {
            get { return internalTexture2; }
            set { internalTexture2 = value; }
        }

        private int width;
        [JsonIgnore]
        public int Width
        {
            get { return width; }
            set
            {
                width = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(width)));
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
}
