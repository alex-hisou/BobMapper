using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using BobMapper.Services;

namespace BobMapper.Model.MapObjects
{
    public class Loot : ISinglePointObject, INotifyPropertyChanged
    {
        public Coordinate Coordinates {  get; set; }

        private string texture;

        public string Texture
        {
            get { return texture; }
            set { texture = value; InternalTexture = InternalNameSevice.GetInternalName(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Texture))); }
        }

        private string internalTexture;
        [JsonIgnore]
        public string InternalTexture
        {
            get { return internalTexture; }
            set { internalTexture = value; }
        }

        private int rotation;

        public int Rotation
        {
            get { return rotation; }
            set { rotation = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rotation))); }
        }


        public Loot(string texture, Coordinate coordinates, int rotation)
        {
            Coordinates = coordinates;
            Texture = texture;
            Rotation = rotation;
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
