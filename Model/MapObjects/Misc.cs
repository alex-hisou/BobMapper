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
            Key = 8,
            DisguisePoint = 11,
            SoundPoint = 12,
        }

        public MiscObjects Type { get; set; }

        public Coordinate Coordinates { get; set; }

        public Misc(Coordinate coordinates, MiscObjects type, int rotation)
        {
            Coordinates = coordinates;
            Type = type;
            SetMiscTexture();
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
                case MiscObjects.SoundPoint:
                    Texture = "/Resources/MiscTextures/SoundPoint.png";
                    break;
            }
        }

        [JsonConstructor]
        public Misc(Coordinate coordinates, string texture, MiscObjects type, int rotation)
        {
            Coordinates = coordinates;
            Texture = texture;
            Type = type;
            Rotation = rotation;
        }

        public string Texture
        {
            get { return texture; }
            set { texture = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Texture))); }
        }

        private int rotation;

        public int Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
    }
}
