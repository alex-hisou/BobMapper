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
            Editor editor = new Editor();
            editor.Show();
        }
    }
}
