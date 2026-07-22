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
    /// Interaction logic for ObjectList.xaml
    /// </summary>
    public partial class ObjectList : Window
    {
        public ObjectList(Map map)
        {
            InitializeComponent();
            ObjectListViewModel objectListViewModel = new(map);
            DataContext = objectListViewModel;
        }
    }
}
