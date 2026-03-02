using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BobMapper.Model.MapObjects
{
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

        public void DeleteObject()
        {
            throw new NotImplementedException();
        }
    }
}
