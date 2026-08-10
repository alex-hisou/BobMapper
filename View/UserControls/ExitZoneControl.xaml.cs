using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BobMapper.View.UserControls
{
    /// <summary>
    /// Interaction logic for ExitZoneControl.xaml
    /// </summary>
    public partial class ExitZoneControl : UserControl
    {
        public ExitZoneControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty SelectedProperty = DependencyProperty.Register
            (nameof(Selected), typeof(bool), typeof(ExitZoneControl));

        public bool Selected
        {
            get => (bool)GetValue(SelectedProperty);
            set => SetValue(SelectedProperty, value);
        }

        public static readonly DependencyProperty Point1Property = DependencyProperty.Register
            (nameof(Point1), typeof(SnapCoordinate), typeof(ExitZoneControl));

        public static readonly DependencyProperty Point2Property = DependencyProperty.Register
            (nameof(Point2), typeof(SnapCoordinate), typeof(ExitZoneControl));

        public static readonly DependencyProperty Point3Property = DependencyProperty.Register
            (nameof(Point3), typeof(SnapCoordinate), typeof(ExitZoneControl));

        public static readonly DependencyProperty Point4Property = DependencyProperty.Register
            (nameof(Point4), typeof(SnapCoordinate), typeof(ExitZoneControl));

        public SnapCoordinate Point1
        {
            get => (SnapCoordinate)GetValue(Point1Property); 
            set => SetValue(Point1Property, value);
        }

        public SnapCoordinate Point2
        {
            get => (SnapCoordinate)GetValue(Point2Property);
            set => SetValue(Point2Property, value);
        }

        public SnapCoordinate Point3
        {
            get => (SnapCoordinate)GetValue(Point3Property);
            set => SetValue(Point3Property, value);
        }

        public SnapCoordinate Point4
        {
            get => (SnapCoordinate)GetValue(Point4Property);
            set { SetValue(Point4Property, value); }
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double deltax = e.HorizontalChange;
            double deltay = e.VerticalChange;
            Thumb thumb = (Thumb)sender;
            string tag = thumb.Tag.ToString();
            Resize(deltax, deltay, tag);
        }

        private void Resize(double deltax, double deltay, string tag)
        {
            deltax = Math.Floor(deltax / 64);
            deltay = Math.Floor(deltay / 64);
            //Points implementation from notes
            switch(tag)
            {
                case "N":
                    Point1.SnappedYPos += (float)deltay;
                    Point2.SnappedYPos += (float)deltay;
                    break;
                case "W":
                    Point2.SnappedXPos += (float)deltax;
                    Point3.SnappedXPos += (float)deltax;
                    break;
                case "E":
                    Point1.SnappedXPos += (float)deltax;
                    Point4.SnappedXPos += (float)deltax;
                    break;
                case "S":
                    Point3.SnappedYPos += (float)deltay;
                    Point4.SnappedYPos += (float)deltay;
                    break;
            }
            //I wish it would just bind properly, but the spaghetti doesnt let me
            Rectangle.Points = new PointCollection([Point1, Point2, Point3, Point4]);
            UpdateView();
        }

        private void UpdateView()
        {
            double midpointNorth = (Point1.XPos + Point2.XPos) / 2;
            double midpointWest = (Point2.YPos + Point3.YPos) / 2;
            double midpointEast = (Point1.YPos + Point4.YPos) / 2;
            double midpointSouth = (Point3.XPos + Point4.XPos) / 2;
            PlaceHandle(NorthHandle, midpointNorth, Point1.YPos);
            PlaceHandle(WestHandle, Point2.XPos, midpointWest);
            PlaceHandle(EastHandle, Point1.XPos, midpointEast);
            PlaceHandle(SouthHandle, midpointSouth, Point4.YPos);
        }

        private static void PlaceHandle(FrameworkElement handle, double x, double y)
        {
            Canvas.SetLeft(handle, x);
            Canvas.SetTop(handle, y);
        }

        private void Rectangle_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateView();
        }
    }
}
