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
using System.Windows.Shapes;
using BobMapper.Model;
using BobMapper.ViewModel;

namespace BobMapper.View
{
    /// <summary>
    /// Interaction logic for MapPropertiesWindow.xaml
    /// </summary>
    public partial class MapPropertiesWindow : Window
    {
        public MapPropertiesWindow(MapProperties mapProperties)
        {
            InitializeComponent();
            MapPropertiesWindowViewModel vm = new MapPropertiesWindowViewModel(mapProperties);
            DataContext = vm;
        }
    }
}
