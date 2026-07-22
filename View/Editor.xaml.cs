using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using BobMapper.View;
using BobMapper.ViewModel;
using static BobMapper.Model.MapManager;

namespace BobMapper
{
    public partial class Editor : Window
    {

        public Editor(string filename)
        {
            InitializeComponent();
            var editorViewModel = new EditorViewModel(filename);
            DataContext = editorViewModel;
            editorViewModel.CurrentSelections.SelectedToolChanged += ToolToggle;
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight; //otherwise covers taskbar
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9\\-.]");
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox textBox = sender as TextBox;
                if (textBox != null)
                {
                    var bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);
                    bindingExpression?.UpdateSource();
                    Keyboard.ClearFocus();
                }
                e.Handled = true;
            }
        }

        private void ClickEmpty(object sender, MouseEventArgs e)
        {
            Keyboard.Focus(WindowGrid);
            WindowGrid.Focus();
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
            Keyboard.ClearFocus();
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this), this);
            ScrollPlane.Focus();
        }

        private void TryClose(object sender, CancelEventArgs e)
        {
            if (DataContext is EditorViewModel editorViewModel)
            {
                if(!editorViewModel.CheckForChanges())
                {
                    e.Cancel = false;
                    return;
                }
            }
            var result = MessageBox.Show("There are unsaved changes. Are you sure you want to exit?", "Exit", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        private void ToolToggle(object sender, EventArgs e)
        {
            //Fuck this code, worst way to do this
            Selections selections = (Selections)sender;
            ToolDrawer.Items.OfType<MenuItem>().ToList().ForEach(x => x.Background = ToolDrawer.Background);
            switch (selections.SelectedTool)
            {
                case Tools.Select:
                    SelectTool.Background = Brushes.DarkGray;
                    break;
                case Tools.Move:
                    MoveTool.Background = Brushes.DarkGray;
                    break;
                case Tools.Rotate:
                    RotateTool.Background = Brushes.DarkGray;
                    break;
                case Tools.AddWall:
                    AddWallTool.Background = Brushes.DarkGray;
                    break;
                case Tools.AddProp:
                    AddPropTool.Background = Brushes.DarkGray;
                    break;
                case Tools.AddNPC:
                    AddNPCTool.Background = Brushes.DarkGray;
                    break;
                case Tools.AddPathPoint:
                    AddPathTool.Background = Brushes.DarkGray;
                    break;
                case Tools.ChangeFloor:
                    ChangeFloorTool.Background = Brushes.DarkGray;
                    break;
                case Tools.AddMisc:
                    AddMiscTool.Background = Brushes.DarkGray;
                    break;
                case Tools.AddDoor:
                    AddDoorTool.Background = Brushes.DarkGray;
                    break;
                case Tools.AddLoot:
                    AddLootTool.Background = Brushes.DarkGray;
                    break;
                case Tools.None:
                    break;
                default:
                    throw new Exception("Invalid tool selection");

            }
        }

        private void ShortcutListOpen(object sender, RoutedEventArgs e)
        {
            ShortcutList shortcutList = new ShortcutList();
            shortcutList.Show();
        }

        private void AssetGalleryScroll_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Keyboard.ClearFocus();
            FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this), this);
        }
    }
}
