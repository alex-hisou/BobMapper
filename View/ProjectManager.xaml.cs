using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using BobMapper.Model;
using BobMapper.Services;
using BobMapper.ViewModel;

namespace BobMapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ProjectManager : Window
    {
        public ProjectManager()
        {
            InitializeComponent();
        }

        private void NewMod_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "Map"; 
            dialog.DefaultExt = ".json"; 
            dialog.Filter = "BobMapper Json Files (.json)|*.json"; 

            bool? result = dialog.ShowDialog();
            string filename;

            if (result == true)
            {
                filename = dialog.FileName;
            }
            else { return; }
            Map map = new Map(0);
            string emptyJson = JsonSerializer.Serialize(map, JsonMapParse.jsonSerializerOptions);
            File.WriteAllText(filename, emptyJson);
            Editor editor = new Editor(filename);
            editor.Show();
            this.Close();
        }

        private void LoadMod_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".json"; 
            dialog.Filter = "BobMapper Json Files (.json)|*.json"; 

            // Show open file dialog box
            bool? result = dialog.ShowDialog();
            string filename;
            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                filename = dialog.FileName;
            }
            else { return; }
            Editor editor = new Editor(filename);
            editor.Show();
            this.Close();
        }
    }
}
