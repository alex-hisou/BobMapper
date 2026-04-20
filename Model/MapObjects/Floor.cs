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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Texture1))); }

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

        private string internalTexture2;
        [JsonIgnore]
        public string InternalTexture2
        {
            get { return internalTexture2; }
            set { internalTexture2 = value; }
        }

        public Floor(string texture1, string texture2)
        {
            Texture1 = texture1; Texture2 = texture2;
        }
    }
}
