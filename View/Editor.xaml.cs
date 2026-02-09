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
using static BobMapper.Model.MapObjects;

namespace BobMapper
{
    public partial class Editor : Window
    {
        public Editor()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight; //otherwise covers taskbar
            Map map = new Map();
            map.walls.Add(new MapObjects.Wall(new Coordinate(1, 2), new Coordinate(2, 2), MapObjects.Wall.WallType.Normal));
            map.walls.Add(new MapObjects.Wall(new Coordinate(3, 2), new Coordinate(4, 2), MapObjects.Wall.WallType.Normal));
            map.props.Add(new MapObjects.Prop(new Coordinate(50, 20), 180));
            map.props.Add(new MapObjects.Prop(new Coordinate(10, 40), 90));
            map.npcs.Add(new MapObjects.NPC(new Coordinate(10, 20), MapObjects.NPC.NPCType.Biff));
            map.npcs.Add(new MapObjects.NPC(new Coordinate(40, 20), MapObjects.NPC.NPCType.RedDressLady));
            map.pathPoints.Add(new MapObjects.PathPoint(new Coordinate(10, 20), 1, 0, 2));
            map.pathPoints.Add(new MapObjects.PathPoint(new Coordinate(10, 20), 2, 1, 3));
            map.miscs.Add(new MapObjects.Misc(new Coordinate(10, 20), MapObjects.Misc.MiscObjects.Lock));
            DataParse.SaveData(map);
        }
    }
}
