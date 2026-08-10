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

        private Tilesets tileset;

        public Tilesets Tileset
        {
            get { return tileset; }
            set { tileset = value; }
        }
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

        private string backGroundImage;

        public string BackgroundImage
        {
            get { return backGroundImage; }
            set { backGroundImage = value;
                OnPropertyChanged();
            }
        }

        public bool IsNightTime { get; set; }

        private double apartmentHeight;

        public double ApartmentHeight
        {
            get { return apartmentHeight; }
            set { apartmentHeight = value;
                OnPropertyChanged();
            }
        }

        private bool automaticExitZones;

        public bool AutomaticExitZones
        {
            get { return automaticExitZones; }
            set { automaticExitZones = value;
                OnPropertyChanged();
            }
        }


        public MapProperties(int width, int height, Tilesets tileset) 
        {
            this.Tileset = tileset;
            Width = width;
            Height = height;
            Name = "Unnamed Map";
            IsApartment = false;
            BackgroundImage = "/Resources/Backgrounds/BackgroundDownTown1.png";
            IsNightTime = false;
            ApartmentHeight = 1.0;
            AutomaticExitZones = true;
        }

        [JsonConstructor]
        public MapProperties(int width, int height, Tilesets tileset, string name, bool isApartment, string BackgroundImage, bool IsNightTime, double apartmentHeight, bool automaticExitZones)
        {
            this.Tileset = tileset;
            this.Width = width;
            this.Height = height;
            this.Name = name;
            this.IsApartment = isApartment;
            this.BackgroundImage = BackgroundImage;
            this.IsNightTime = IsNightTime;
            ApartmentHeight = apartmentHeight;
            AutomaticExitZones = automaticExitZones;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event EventHandler TilesetChanged;

        public void ChangeTileset()
        {
            TilesetChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
