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
using static BobMapper.Model.MapProperties;

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

        private void ToolToggle(object sender, RoutedEventArgs e)
        {
           //Fuck this code, worst way to do this
            var vm = (EditorViewModel)DataContext;
            MenuItem senderReference = (MenuItem)sender;
            ToolDrawer.Items.OfType<MenuItem>().ToList().ForEach(x => x.Background = ToolDrawer.Background);
            switch (senderReference.Name)
            {
                case "SelectTool":
                    vm.SelectTool(Tools.Select);
                    if(vm.CurrentSelections.SelectedTool == Tools.Select)
                    { SelectTool.Background = Brushes.DarkGray; }
                    break;
                case "MoveTool":
                    vm.SelectTool(Tools.Move);
                    if(vm.CurrentSelections.SelectedTool == Tools.Move)
                    { MoveTool.Background = Brushes.DarkGray; }
                    break;
                case "RotateTool":
                    vm.SelectTool(Tools.Rotate);
                    if(vm.CurrentSelections.SelectedTool == Tools.Rotate)
                    { RotateTool.Background = Brushes.DarkGray; }
                    break;
                case "AddWallTool":
                    vm.SelectTool(Tools.AddWall);
                    if(vm.CurrentSelections.SelectedTool == Tools.AddWall)
                    { AddWallTool.Background = Brushes.DarkGray; }
                    break;
                case "AddPropTool":
                    vm.SelectTool(Tools.AddProp);
                    if(vm.CurrentSelections.SelectedTool == Tools.AddProp)
                    { AddPropTool.Background = Brushes.DarkGray; }
                    break;
                case "AddNPCTool":
                    vm.SelectTool(Tools.AddNPC);
                    if(vm.CurrentSelections.SelectedTool == Tools.AddNPC)
                    { AddNPCTool.Background = Brushes.DarkGray; }
                    break;
                case "AddPathTool":
                    vm.SelectTool(Tools.AddPathPoint);
                    if(vm.CurrentSelections.SelectedTool == Tools.AddPathPoint)
                    { AddPathTool.Background = Brushes.DarkGray; }
                    break;
                case "ChangeFloorTool":
                    vm.SelectTool(Tools.ChangeFloor);
                    if(vm.CurrentSelections.SelectedTool == Tools.ChangeFloor)
                    { ChangeFloorTool.Background = Brushes.DarkGray; }
                    break;
                case "AddMiscTool":
                    vm.SelectTool(Tools.AddMisc);
                    if(vm.CurrentSelections.SelectedTool == Tools.AddMisc)
                    { AddMiscTool.Background = Brushes.DarkGray; }
                    break;
                default:
                    throw new Exception("YOU DONE FUCKED UP!!!!!1!!1!!!!!!!1!!1!!!!1");

            }
        }


    }
}
