using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using BobMapper.Services;

namespace BobMapper.Model.MapObjects
{
    public class Floor : INotifyPropertyChanged
    {
        private string texture1;

        public event PropertyChangedEventHandler PropertyChanged;
        
        public string Texture1
        {
            get { return texture1; }
            set { texture1 = value; InternalTexture1 = InternalNameSevice.GetInternalName(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Texture1)));
                
            }

        }

        private string internalTexture1;

        [JsonIgnore]
        public string InternalTexture1
        {
            get { return internalTexture1; }
            set { internalTexture1 = value; }
        }

        private string texture2;
        
        public string Texture2
        {
            get { return texture2; }
            set { texture2 = value; InternalTexture2 = InternalNameSevice.GetInternalName(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Texture2))); }

        }

        private int flip;

        public int Flip
        {
            get { return flip; }
            set {   if(value > 3)
                    { flip = 0; }
                else flip = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Flip)));
                SetVisualFlip();
            }
        }

        private int visualRotate;
        [JsonIgnore]
        public int VisualRotate
        {
            get { return visualRotate; }
            set { visualRotate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisualRotate)));
            }
        }

        private string internalTexture2;
        [JsonIgnore]
        public string InternalTexture2
        {
            get { return internalTexture2; }
            set { internalTexture2 = value; }
        }

        private float opacity;

        public float Opacity
        {
            get { return opacity; }
            set { opacity = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Opacity))); }
        }

        public Floor(string texture1, string texture2, int flip)
        {
            Texture1 = texture1; Texture2 = texture2;
            Flip = flip; Opacity = 1.0f;
        }

        public void SetOpacity(bool isApartment)
        {
            Opacity = 1.0f;
            if(isApartment)
            {
                if (Texture1 == @"/Resources/FloorTextures/Floor_Nothing.png")
                {
                    Opacity = 0.2f;
                }
            }
        }

        private void SetVisualFlip()
        {
            switch (Flip)
            {
                //Totally fucked up but whatever
                case 0:
                    VisualRotate = 90;
                    break;
                case 1:
                    VisualRotate = 0;
                    break;
                case 2:
                    VisualRotate = 270;
                    break;
                case 3: 
                    VisualRotate = 180;
                    break;
                default:
                    throw new Exception("Invalid floor flip value");
            }
        }

    }
}
