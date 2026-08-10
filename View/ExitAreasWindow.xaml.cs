using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using BobMapper.Model;
using BobMapper.Model.MapObjects;
using BobMapper.ViewModel;

namespace BobMapper.View
{
    /// <summary>
    /// Interaction logic for ExitArea.xaml
    /// </summary>
    public partial class ExitAreasWindow : Window
    {
        public ExitAreasWindow(ObservableCollection<ExitZone> exitZones, LayerData layerData)
        {
            InitializeComponent();
            ExitAreasViewModel viewModel = new ExitAreasViewModel(exitZones, layerData);
            DataContext = viewModel;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }
    }
}
