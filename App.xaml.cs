using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
        AddDoor,
        AddLoot
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

    public class Coordinate
    {
        public float XPos {  get; set; }
        public float YPos { get; set; }

        public Coordinate(float XPos, float YPos)
        {
            this.XPos = XPos;
            this.YPos = YPos;
        }

        
    }

    
    public class CenteringCanvas : Canvas
    {
        //WARNING: VIBECODED CRAP!!!
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            foreach (UIElement child in Children)
            { 
                double x = GetLeft(child);
                double y = GetTop(child);
                if (double.IsNaN(x)) x = 0;
                if (double.IsNaN(y)) y = 0;
                double centeredX = x - (child.DesiredSize.Width / 2);
                double centeredY = y - (child.DesiredSize.Height / 2);

                child.Arrange(new Rect(new Point(centeredX, centeredY), child.DesiredSize));
            }
            return arrangeSize;
        }
    }

    public class SnapCoordinate : INotifyPropertyChanged
    {
        //ABHORRENT PATCHWORK AHEAD
        public event PropertyChangedEventHandler PropertyChanged;
        [JsonIgnore]
        public const int FloorSize = 64;

        private float xPos;

        [JsonIgnore]
        public float XPos
        {
            get { return xPos; }
            set { xPos = value; OnPropertyChanged(nameof(XPos));
            }
        }


        private float yPos;
        [JsonIgnore]
        public float YPos
        {
            get { return yPos; }
            set { yPos = value; OnPropertyChanged(nameof(yPos));
            }
        }

        private float snappedXPos;
        public float SnappedXPos
        {
            get { return snappedXPos; }
            set { snappedXPos = value;
                float floatValue = value * FloorSize;
                XPos = Convert.ToInt32(floatValue);
            }
        }
        private float snappedYPos;
        public float SnappedYPos
        {
            get { return snappedYPos; }
            set { snappedYPos = value;
                float floatValue = value * FloorSize;
                YPos = floatValue;
            }
        }

        public SnapCoordinate(float snappedXPos, float snappedYPos)
        {
            SnappedXPos = snappedXPos;
            SnappedYPos = snappedYPos;
        }

        public static SnapCoordinate UnsnappedCoordinateFactory(float unsnappedXPos, float unsnappedYPos)
        {
            float snappedXPos = (unsnappedXPos - (unsnappedXPos % FloorSize)) / FloorSize;
            float snappedYPos = (unsnappedYPos - (unsnappedYPos % FloorSize)) / FloorSize;
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
}
