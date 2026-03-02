using System;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BobMapper.Model.MapObjects
{
    public class PathPoint : ISinglePointObject, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int? ConnectToPoint { get; set; }

        public Coordinate Coordinates { get; set; }

        [JsonConstructor]
        public PathPoint(Coordinate coordinates, int id, int connectToId)
        {
            Coordinates = coordinates;
            Id = id;
            ConnectToPoint = connectToId;
        }

        public PathPoint(Coordinate coordinates, int id)
        {
            Coordinates = coordinates;
            Id = id;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void DeleteObject()
        {
            throw new NotImplementedException();
        }
    }
}
