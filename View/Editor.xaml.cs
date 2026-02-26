using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BobMapper.Model;
using BobMapper.Services;
using BobMapper.ViewModel;
using static BobMapper.Model.MapObjects;

namespace BobMapper
{
    public partial class Editor : Window
    {
        public Editor()
        {
            InitializeComponent();

            var editorViewModel = new EditorViewModel();
            DataContext = editorViewModel;

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight; //otherwise covers taskbar

            
            
            //Map map = DataParse.LoadData();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9][^-]+");
        }

        private void ClickEmpty(object sender, MouseEventArgs e)
        {
            var mousePos = e.GetPosition(ScrollPlane);
            int wholeX = Convert.ToInt32(mousePos.X);
            int wholeY = Convert.ToInt32(mousePos.Y);
            int cartesianFrameWidth = Convert.ToInt32(ScrollPlane.ActualWidth / 2);
            int cartesianFrameHeight = Convert.ToInt32(ScrollPlane.ActualHeight / 2);
            Coordinate placementPos = new Coordinate(wholeX - cartesianFrameWidth, cartesianFrameHeight - wholeY);
            if (DataContext is EditorViewModel editorViewModel)
            {
                editorViewModel.ClickEmpty(placementPos);
            }
        }

        private void ToolToggle(object sender)
        {

        }
    }
}
