using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BobMapper.Model.MapObjects
{
    public class Loot : ISinglePointObject, INotifyPropertyChanged
    {
        public Coordinate Coordinates {  get; set; }

        private string texture;

        public string Texture
        {
            get { return texture; }
            set { texture = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Texture))); }
        }

        private bool isMain;

        public bool IsMain
        {
            get { return isMain; }
            set { isMain = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMain))); }
        }

        private int rotation;

        public int Rotation
        {
            get { return rotation; }
            set { rotation = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rotation))); }
        }


        public Loot(string texture, Coordinate coordinates, bool isMain, int rotation)
        {
            Coordinates = coordinates;
            Texture = texture;
            IsMain = isMain;
            Rotation = rotation;
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
