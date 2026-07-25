using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BobMapper.Model
{
    public class MapProperties : INotifyPropertyChanged
    {

        public Tilesets tileset;
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }

        private bool isApartment;

        public bool IsApartment
        {
            get { return isApartment; }
            set { isApartment = value;
                OnPropertyChanged();
            }
        }

        public string BackgroundImage { get; set; }

        public bool IsNightTime { get; set; }

        public MapProperties(int width, int height, Tilesets tileset) 
        {
            this.tileset = tileset;
            Width = width;
            Height = height;
            Name = "Unnamed Map";
            IsApartment = false;
            BackgroundImage = "";
            IsNightTime = false;
        }

        [JsonConstructor]
        public MapProperties(int width, int height, Tilesets tileset, string name, bool isApartment, string BackgroundImage, bool IsNightTime)
        {
            this.tileset = tileset;
            this.Width = width;
            this.Height = height;
            this.Name = name;
            this.IsApartment = isApartment;
            this.BackgroundImage = BackgroundImage;
            this.IsNightTime = IsNightTime;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
