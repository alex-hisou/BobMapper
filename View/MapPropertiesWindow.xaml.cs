using System;
using System.Collections.Generic;
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
using BobMapper.ViewModel;

namespace BobMapper.View
{
    /// <summary>
    /// Interaction logic for MapPropertiesWindow.xaml
    /// </summary>
    public partial class MapPropertiesWindow : Window
    {
        public MapPropertiesWindow(MapProperties mapProperties, Selections selections)
        {
            InitializeComponent();
            MapPropertiesWindowViewModel vm = new MapPropertiesWindowViewModel(mapProperties, selections);
            DataContext = vm;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9\\.]");
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
    }
}
