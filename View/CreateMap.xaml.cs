using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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

namespace BobMapper.View
{
    /// <summary>
    /// Interaction logic for CreateMap.xaml
    /// </summary>
    public partial class CreateMap : Window
    {
        CreateMapViewModel viewModel;

        public CreateMap()
        {
            InitializeComponent();
            CreateMapViewModel createMapViewModel = new CreateMapViewModel();
            DataContext = createMapViewModel;
            viewModel = createMapViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            viewModel.CreateMap();
            this.Close();
            MapCreationComplete.Invoke(this, EventArgs.Empty);
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]");
        }

        public event EventHandler MapCreationComplete;
    }
}
