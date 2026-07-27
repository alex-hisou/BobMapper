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

namespace BobMapper.View
{
    /// <summary>
    /// Interaction logic for ExpandOrContract.xaml
    /// </summary>
    public partial class ExpandOrContract : Window
    {
        Map currentMap;
        public ExpandOrContract(Map map)
        {
            InitializeComponent();
            currentMap = map;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int northOffset = int.Parse(NorthTextBox.Text);
            int southOffset = int.Parse(SouthTextBox.Text);
            int eastOffset = int.Parse(EastTextBox.Text);
            int westOffset = int.Parse(WestTextBox.Text);
            int netVerticalOffset = northOffset + southOffset;
            int netHorizontalOffset = westOffset + eastOffset;
            int snapHeight = currentMap.mapProperties.Height / 64;
            int snapWidth = currentMap.mapProperties.Width / 64;
            int newFloorHeight = snapHeight + netVerticalOffset;
            int newFloorWidth = snapWidth + netHorizontalOffset;
            if(newFloorHeight < 1 || newFloorWidth < 1)
            {
                MessageBox.Show("Invalid map size", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            currentMap.ExpandOrContractMap(northOffset, southOffset, eastOffset, westOffset);
            this.Close();
        }
    }
}
