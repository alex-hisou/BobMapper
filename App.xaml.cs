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

    

    public class Coordinate
    {
        public const int FloorSize = 64;
        public int XPos {  get; set; }
        public int YPos { get; set; }

        public Coordinate(int x, int y)
        {
            this.XPos = x;
            this.YPos = y;
        }
    }
}
