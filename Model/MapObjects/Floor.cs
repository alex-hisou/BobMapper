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

        private int visualScaleX;
        [JsonIgnore]
        public int VisualScaleX
        {
            get { return visualScaleX; }
            set { visualScaleX = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisualScaleX)));
            }
        }

        private int visualScaleY;
        [JsonIgnore]
        public int VisualScaleY
        {
            get { return visualScaleY; }
            set { visualScaleY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisualScaleY)));
            }
        }

        private string internalTexture2;
        [JsonIgnore]
        public string InternalTexture2
        {
            get { return internalTexture2; }
            set { internalTexture2 = value; }
        }

        public Floor(string texture1, string texture2, int flip)
        {
            Texture1 = texture1; Texture2 = texture2;
            Flip = flip;
        }

        private void SetVisualFlip()
        {
            switch (Flip)
            {
                //Totally fucked up but whatever
                case 0:
                    VisualScaleX = -1;
                    VisualScaleY = 1;
                    break;
                case 1:
                    VisualScaleX = 1;
                    VisualScaleY = 1;
                    break;
                case 2:
                    VisualScaleX = 1;
                    VisualScaleY = -1;
                    break;
                case 3: 
                    VisualScaleX = -1;
                    VisualScaleY = -1;
                    break;
                default:
                    throw new Exception("Invalid floor flip value");
            }
        }

    }
}
