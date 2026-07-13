using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for MoveGizmo.xaml
    /// </summary>
    public partial class MoveGizmo : UserControl
    {
        private bool isDragging;
        private SnapCoordinate lastMousePos;

        public ICommand DragCommand
        {
            get => (ICommand)GetValue(DragCommandProperty);
            set => SetValue(DragCommandProperty, value);
        }

        public static readonly DependencyProperty DragCommandProperty =
            DependencyProperty.Register(
                nameof(DragCommand),
                typeof(ICommand),
                typeof(MoveGizmo));

        public MoveGizmo()
        {
            InitializeComponent();
            lastMousePos = new(0, 0);
        }



        private void Element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UIElement element = sender as UIElement;
            if (element == null)
            {
                return;
            }
            isDragging = true;
            element.CaptureMouse();
            Point clickPosition = e.GetPosition(this);
            lastMousePos = SnapCoordinate.UnsnappedCoordinateFactory((float)clickPosition.X, (float)clickPosition.Y);

        }

        private void Element_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging)
            {
                return;
            }

            var element = sender as UIElement;
            if (element == null) return;

            Point currentMousePos = e.GetPosition(this);

            int newX = (int)currentMousePos.X - lastMousePos.XPos;
            int newY = (int)currentMousePos.Y - lastMousePos.YPos;
            SnapCoordinate newCoordinate = SnapCoordinate.UnsnappedCoordinateFactory(newX, newY);
            DragCommand.Execute(newCoordinate);
        }

        private void Element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            UIElement element = sender as UIElement;
            if (!isDragging)
            {
                return;
            }
            if (element != null)
            {
                isDragging = false;
                element.ReleaseMouseCapture();
            }
        }


    }
}
