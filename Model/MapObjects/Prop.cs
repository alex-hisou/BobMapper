using System.ComponentModel;

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
    }
}
