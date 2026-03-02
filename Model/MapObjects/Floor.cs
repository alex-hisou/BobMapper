using System.ComponentModel;

namespace BobMapper.Model.MapObjects
{
    public class Floor : INotifyPropertyChanged
    {
        private string texture1;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Texture1
        {
            get { return texture1; }
            set { texture1 = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Texture1))); }

        }

        private string texture2;
        public string Texture2
        {
            get { return texture2; }
            set { texture2 = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Texture2))); }

        }

        public Floor(string texture1, string texture2)
        {
            Texture1 = texture1; Texture2 = texture2;
        }
    }
}
