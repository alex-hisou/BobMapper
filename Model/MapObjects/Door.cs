using System.ComponentModel;

namespace BobMapper.Model.MapObjects
{
    public class Door : IDoublePointObject, INotifyPropertyChanged
    {

        private SnapCoordinate point1;
        public SnapCoordinate Point1
        {
            get { return point1; }
            set { point1 = value; }
        }
        private SnapCoordinate point2;
        public SnapCoordinate Point2
        {
            get { return point2; }
            set { point2 = value; }
        }
        private string texture1;
        public string Texture1
        {
            get { return texture1; }
            set { texture1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Texture1)));
            }
        }

        private bool locked;
        public bool Locked
        {
            get { return locked; }
            set { locked = value; }
        }

        private bool permLocked;
        public bool PermLocked
        {
            get { return permLocked; }
            set { permLocked = value; }
        }

        public Door(SnapCoordinate point1, SnapCoordinate point2, string texture1, bool locked, bool permlocked)
        {
            Point1 = point1;
            Point2 = point2;
            Texture1 = texture1;
            Locked = locked;
            PermLocked = permlocked;
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
