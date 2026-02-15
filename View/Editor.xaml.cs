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

            DataContext = new EditorViewModel();

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight; //otherwise covers taskbar
            
            //Map map = DataParse.LoadData();
        }
    }
}
