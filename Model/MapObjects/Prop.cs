using System.ComponentModel;
using System.Text.Json.Serialization;
using BobMapper.Services;

namespace BobMapper.Model.MapObjects
{
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
            set { propTexture = value; InternalTexture = InternalNameSevice.GetInternalName(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PropTexture))); }
        }


        private string internalTexture;
        [JsonIgnore]
        public string InternalTexture
        {
            get { return internalTexture; }
            set { internalTexture = value; }
        }


        public Prop(Coordinate coordinates, int rotation, string propTexture)
        {
            Coordinates = coordinates;
            Rotation = rotation;
            PropTexture = propTexture;
        }

        public void DeleteObject()
        {

        }
    }
}
