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

        private int rotation;

        public int Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rotation)));
            }
        }

        public int Duration { get; set; }

        private SnapCoordinate coordinates;

        public SnapCoordinate Coordinates
        {
            get { return coordinates; }
            set { coordinates = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Coordinates)));
                ConnectionPointChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConnectToId)));
            }
        }


        [JsonConstructor]
        public PathPoint(SnapCoordinate coordinates, int duration, int id, int? connectToId)
        {
            Coordinates = coordinates;
            Id = id;
            ConnectToId = connectToId;
            Duration = duration;
        }

        public PathPoint(SnapCoordinate coordinates, int duration, int id)
        {
            Coordinates = coordinates;
            Id = id;
            Duration = duration;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler ConnectionPointChanged;

    }
}
