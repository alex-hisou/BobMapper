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
            Rectangle.Points = new PointCollection(4);
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
            set => SetValue(Point4Property, value);
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double delta = e.HorizontalChange + e.VerticalChange;
            Thumb thumb = (Thumb)sender;
            string tag = thumb.Tag.ToString();
            Resize(delta, tag);
        }

        private void Resize(double delta, string tag)
        {
            //Points implementation from notes
            switch(tag)
            {
                case "N":
                    break;
                case "W":
                    break;
                case "E":
                    break;
                case "S":
                    break;
            }
        }
    }
}
