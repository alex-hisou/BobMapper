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

        private static ResourceDictionary currentTheme;

        public static void SetTheme(Themes theme)
        {
            if (currentTheme != null)
                Application.Current.Resources.MergedDictionaries
                    .Remove(currentTheme);
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
