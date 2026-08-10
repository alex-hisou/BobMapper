using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BobMapper.Model.MapObjects
{
    public class ExitZone : INotifyPropertyChanged
    {
        //Points 1 and 3 form a diagonal that the game uses for its zone logic.
        //Points 2 and 4 do not matter, but are present for usability reasons.
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        private SnapCoordinate point3;
        public SnapCoordinate Point3
        {
            get { return point3; }
            set { point3 = value; }
        }

        private SnapCoordinate point4;
        public SnapCoordinate Point4
        {
            get { return point4; }
            set { point4 = value; }
        }

        private bool selected = false;

        [JsonIgnore]
        public bool Selected
        {
            get { return selected; }
            set { selected = value; OnPropertyChanged(); }
        }


        [JsonConstructor]
        public ExitZone(SnapCoordinate point1, SnapCoordinate point2, SnapCoordinate point3, SnapCoordinate point4) 
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
            Point4 = point4;
        }

    }
}
