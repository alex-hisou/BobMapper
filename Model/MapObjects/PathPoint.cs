using System;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BobMapper.Model.MapObjects
{
    public class PathPoint : ISinglePointObject, INotifyPropertyChanged
    {
        public int Id { get; set; }

        private int? connectToId;

        public int? ConnectToId
        {
            get { return connectToId; }
            set { connectToId = value; ConnectionPointChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConnectToId))); }
        }

        public int Duration { get; set; }

        private Coordinate coordinates;

        public Coordinate Coordinates
        {
            get { return coordinates; }
            set { coordinates = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Coordinates)));
                ConnectionPointChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConnectToId)));
            }
        }

        
        private Coordinate lineConnectionCoordinate;
        [JsonIgnore]
        public Coordinate LineConnectionCoordinate
        {
            get { return lineConnectionCoordinate; }
            set 
            { 
                Coordinate shiftedCoordinate = value;
                shiftedCoordinate.XPos += -Coordinates.XPos;
                shiftedCoordinate.YPos += -Coordinates.YPos;
                lineConnectionCoordinate = shiftedCoordinate;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LineConnectionCoordinate))); 
            }
        }

        private Coordinate absoluteLineConnectionCoordinate;

        public Coordinate AbsoluteLineConnectionCoordinates
        {
            get { return absoluteLineConnectionCoordinate; }
            set { absoluteLineConnectionCoordinate = value; LineConnectionCoordinate = value; }
        }


        [JsonConstructor]
        public PathPoint(Coordinate coordinates, int duration, int id, int connectToId)
        {
            Coordinates = coordinates;
            Id = id;
            ConnectToId = connectToId;
            Duration = duration;
        }

        public PathPoint(Coordinate coordinates, int duration, int id)
        {
            Coordinates = coordinates;
            Id = id;
            LineConnectionCoordinate = coordinates;
            Duration = duration;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler ConnectionPointChanged;


        public void DeleteObject()
        {
            throw new NotImplementedException();
        }
    }
}
