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

namespace BobMapper.View
{
    /// <summary>
    /// Look, the code sucks, but it needs to work
    /// </summary>
    public partial class InjectorPrompt : Window
    {
        Array chapters => Enum.GetValues(typeof(Map.Chapter));

        public InjectorPrompt()
        {
            InitializeComponent();
            ChapterComboBox.ItemsSource = chapters;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]");
        }

        public event EventHandler<MapManager.LevelInjectPromptEventArgs> ConfirmationComplete;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //What a mouthful
            MapManager.LevelInjectPromptEventArgs eventArgs = new MapManager.LevelInjectPromptEventArgs();
            eventArgs.Chapter = (Map.Chapter)ChapterComboBox.SelectedItem;
            eventArgs.Level = int.Parse(LevelNumber.Text);
            ConfirmationComplete?.Invoke(this, eventArgs);
            this.Close();
        }
    }
}
