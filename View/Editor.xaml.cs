using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BobMapper.Model.MapObjects;
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
                Keyboard.ClearFocus();
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(this), this);
            }
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

        private void AboutOpen(object sender, RoutedEventArgs e)
        {
            About about = new About();
            about.Show();
        }

        private void MapPropertiesClick(object sender, RoutedEventArgs e)
        {
            var vm = (EditorViewModel)DataContext;
            MapPropertiesWindow mapPropertiesWindow = new(vm.CurrentMapProperties, vm.CurrentSelections);
            mapPropertiesWindow.Show();
        }

        private void PrefrencesClick(object sender, RoutedEventArgs e)
        {
            Prefrences prefrences = new Prefrences();
            prefrences.Show();
        }

        private void ExpandOrContractOpen(object sender, RoutedEventArgs e)
        {
            var vm = (EditorViewModel)DataContext;
            ExpandOrContract expandOrContract = new(vm.CurrentMap);
            expandOrContract.Show();
            EventHandler mapSizeChangeHandler = null!;
            mapSizeChangeHandler = (sender, e) =>
            {
                vm.CurrentObjectCollection.CurrentFloors = new ObservableCollection<ObservableCollection<Floor>>(FlattenFloors(vm.CurrentMap.floors));
                vm.CurrentMap.MapSizeChanged -= mapSizeChangeHandler;
            };
            vm.CurrentMap.MapSizeChanged += mapSizeChangeHandler;
        }

        private void OpenProject(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "BobMapper Map File (.bobmap)|*.bobmap|BobMapper Json Files (.json)|*.json";
            bool? result = dialog.ShowDialog();
            string filename;
            if (result == true)
            {
                filename = dialog.FileName;
            }
            else { return; }
            Editor editor = new Editor(filename);
            editor.Show();
        }

        private void NewProject(object sender, RoutedEventArgs e)
        {
            CreateMap createMap = new CreateMap();
            createMap.Show();
        }

        private void ExitAreasOpen(object sender, RoutedEventArgs e)
        {
            var vm = (EditorViewModel)DataContext;
            ExitAreasWindow exitAreasWindow = new(vm.CurrentObjectCollection.CurrentExitZones, vm.CurrentLayerData);
            exitAreasWindow.Show();
        }

        private void DownloadZip(object sender, RoutedEventArgs e)
        {
            Uri uri = new("https://drive.google.com/uc?export=download&id=1FitSAD96k9nVp6HoTE7XwP0A5u42kiBN");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = uri.AbsoluteUri,
                UseShellExecute = true
            });
        }
    }
}
