using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BobMapper.Model.MapObjects
{
    public class Misc : ISinglePointObject, INotifyPropertyChanged
    {
        public enum MiscObjects
        {
            Spawn = 2,
            MainLoot = 7,
            Key = 8,
            DisguisePoint = 11,
            SoundPoint = 12,
        }

        [JsonIgnore]
        public static Array MiscTypeValues => Enum.GetValues(typeof(MiscObjects));

        private MiscObjects type;

        public MiscObjects Type
        {
            get { return type; }
            set { type = value; SetMiscTexture(); }
        }

        public SnapCoordinate Coordinates { get; set; }

        public Misc(SnapCoordinate coordinates, MiscObjects type, int rotation)
        {
            Coordinates = coordinates;
            Type = type;
            Rotation = rotation;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string texture;

        private void SetMiscTexture()
        {
            switch(Type)
            {
                case MiscObjects.Spawn:
                    Texture = "/Resources/MiscTextures/BobSpawn.png";
                    break;
                case MiscObjects.Key:
                    Texture = "/Resources/MiscTextures/Key.png";
                    break;
                case MiscObjects.DisguisePoint:
                    Texture = "/Resources/MiscTextures/DisguisePoint.png";
                    break;
                case MiscObjects.MainLoot:
                    Texture = "/Resources/MiscTextures/MainLoot.png";
                    break;
                case MiscObjects.SoundPoint:
                    Texture = "/Resources/MiscTextures/SoundPoint.png";
                    break;
            }
        }

        [JsonIgnore]
        public string Texture
        {
            get { return texture; }
            set { texture = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Texture))); }
        }

        private int rotation;

        public int Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rotation)));
                ViewRotation = value + 180;
            }
        }

        private int viewRotation;

        [JsonIgnore]
        public int ViewRotation
        {
            get { return viewRotation; }
            set { viewRotation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ViewRotation)));
            }
        }
    }
}
