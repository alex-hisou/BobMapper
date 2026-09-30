using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BobMapper.Services
{
    public enum Themes
    {
        Classic,
        Dark
    }

    public class ThemeService
    {

        public static void SetTheme(Themes theme)
        {
            Application.Current.Resources.MergedDictionaries.RemoveAt(0);
            string themeName;
            switch (theme)
            {
                case Themes.Classic:
                    themeName = "Classic.xaml";
                    break;
                case Themes.Dark:
                    themeName = "Dark.xaml";
                    break;
                default:
                    throw new NotImplementedException();
            }
            ResourceDictionary currentTheme;
            var assemblyName = typeof(App).Assembly.GetName().Name;
            currentTheme = new ResourceDictionary
            {
                Source = new Uri(
                    $"/{assemblyName};component/View/Themes/{themeName}",
                    UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries
                .Insert(0, currentTheme);
        }
    }
}
