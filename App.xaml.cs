using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

namespace BobMapper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    
    public partial class App : Application
    {
    }


    internal enum Tools
    {
        None,
        Select,
        Move,
        Rotate,
        AddWall,
        AddProp,
        AddNPC,
        AddPathPoint,
        AddMisc,
        ChangeFloor,
        AddDoor
    }

    public enum Tilesets
    {
        Suburbs,
        Downtown,
        Labs,
        Strip,
        Winter,
        Camp
    }

    public enum TextureType
    {
        All = -1,
        Wall,
        Prop,
        Floor,
        Loot,
        Door
    }

    public class Coordinate : ICoordinate
    {
        public int XPos {  get; set; }
        public int YPos { get; set; }

        public Coordinate(int XPos, int YPos)
        {
            this.XPos = XPos;
            this.YPos = YPos;
        }

        
    }

    public class SnapCoordinate : ICoordinate, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        [JsonIgnore]
        public const int FloorSize = 64;

        private int xPos;

        [JsonIgnore]
        public int XPos
        {
            get { return xPos; }
            set { xPos = value; OnPropertyChanged(nameof(XPos)); }
        }


        private int yPos;
        [JsonIgnore]
        public int YPos
        {
            get { return yPos; }
            set { yPos = value; OnPropertyChanged(nameof(yPos)); }
        }

        private int snappedXPos;
        public int SnappedXPos
        {
            get { return snappedXPos; }
            set { snappedXPos = value; XPos = value * FloorSize; }
        }
        private int snappedYPos;
        public int SnappedYPos
        {
            get { return snappedYPos; }
            set { snappedYPos = value; YPos = value * FloorSize; }
        }


        public SnapCoordinate(int snappedXPos, int snappedYPos)
        {
            SnappedXPos = snappedXPos;
            SnappedYPos = snappedYPos;
            XPos = snappedXPos * FloorSize;
            YPos = snappedYPos * FloorSize;
        }

        public static SnapCoordinate UnsnappedCoordinateFactory(int unsnappedXPos, int unsnappedYPos)
        {
            int snappedXPos = (unsnappedXPos - (unsnappedXPos % FloorSize)) / FloorSize;
            int snappedYPos = (unsnappedYPos - (unsnappedYPos % FloorSize)) / FloorSize;
            SnapCoordinate SnapCoordinate = new SnapCoordinate(snappedXPos, snappedYPos);
            return SnapCoordinate;
        }

        public static explicit operator Coordinate(SnapCoordinate snapCoordinate)
        {
            Coordinate coordinate = new Coordinate(snapCoordinate.SnappedXPos, snapCoordinate.SnappedYPos);
            return coordinate;
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public interface ICoordinate 
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
    }
}
