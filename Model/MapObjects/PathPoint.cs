using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BobMapper.Model.MapObjects
{
    //Proper bullshit this code is
    public class PathPoint : ISinglePointObject, INotifyPropertyChanged
    {
        public int Id { get; set; }

        private int? connectToId;

        public int? ConnectToId
        {
            get { return connectToId; }
            set { connectToId = value; 
                ConnectionPointChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConnectToId)));
                
            }
        }

        private float rotation;

        public float Rotation
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
            get => coordinates;
            set
            {
                if (coordinates != null)
                    coordinates.PropertyChanged -= OnCoordinatesChanged;
                coordinates = value;
                if (coordinates != null)
                    coordinates.PropertyChanged += OnCoordinatesChanged;
                OnPropertyChanged();
                NotifyConnectionChanged();
            }
        }

        private PathPoint connectedPathPoint;

        [JsonIgnore]
        public PathPoint ConnectedPathPoint
        {
            get => connectedPathPoint;
            set
            {
                if (connectedPathPoint != null)
                    connectedPathPoint.PropertyChanged -= OnConnectedPointChanged;
                connectedPathPoint = value;
                if (connectedPathPoint != null)
                    connectedPathPoint.PropertyChanged += OnConnectedPointChanged;
                NotifyConnectionChanged();
            }
        }

        [JsonIgnore]
        public float ConnectionDeltaX => ConnectedPathPoint == null ? 0 : ConnectedPathPoint.Coordinates.XPos - Coordinates.XPos;

        [JsonIgnore]
        public float ConnectionDeltaY => ConnectedPathPoint == null ? 0 : ConnectedPathPoint.Coordinates.YPos - Coordinates.YPos;


        [JsonConstructor]
        public PathPoint(SnapCoordinate coordinates, int duration, int id, int? connectToId, float rotation)
        {
            Coordinates = coordinates;
            Id = id;
            ConnectToId = connectToId;
            Duration = duration;
            Rotation = rotation;
        }

        public PathPoint(SnapCoordinate coordinates, int duration, int id, float rotation)
        {
            Coordinates = coordinates;
            Id = id;
            Duration = duration;
            Rotation = rotation;
        }

        private void OnCoordinatesChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Coordinates));
            NotifyConnectionChanged();
        }

        private void OnConnectedPointChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Coordinates))
                NotifyConnectionChanged();
        }

        private void NotifyConnectionChanged()
        {
            OnPropertyChanged(nameof(ConnectionDeltaX));
            OnPropertyChanged(nameof(ConnectionDeltaY));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler ConnectionPointChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
