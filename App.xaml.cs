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
    /// 

    
    public partial class App : Application
    {
    }

    public struct Coordinate
    {
        public const int gridFactor = 5;
        public int x {  get; set; }
        public int y { get; set; }

        public Coordinate(int x, int y)
        {
            this.x = x * gridFactor;
            this.y = y * gridFactor;
        }
    }
}
