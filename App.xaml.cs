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


    internal enum Tools
    {
        None,
        Select,
        Move,
        Rotate,
        AddWall,
        AddProp,
        AddNPC,
        AddPathPoint,
        AddMisc,
        ChangeFloor
    }

    public enum Tilesets
    {
        Suburbs,
        Downtown,
        Labs,
        Strip,
        Winter,
        Camp
    }
    internal static class TextureSources
    {
        internal static string FloorTextures = @"/Resources/FloorTextures/";
        internal static string LootTextures = @"/Resources/LootTextures/";
        internal static string NPCTextures = @"/Resources/NPCTextures/";
        internal static string PropTextures = @"/Resources/PropTextures/";
        internal static string SpecialWallTextures = @"/Resources/SpecialWallTextures/";
        internal static string WallTextures = @"/Resources/WallTextures/";
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

        public void SnapCoordinate()
        {
            XPos -= XPos % FloorSize;
            YPos -= YPos % FloorSize;
        }
    }
}
