using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BobMapper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
    }

    internal struct Coordinate
    {
        internal int x, y;

        internal Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
