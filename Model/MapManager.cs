using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;
using BobMapper.Properties;
using BobMapper.Model.MapObjects;
using System;

namespace BobMapper.Model
{
    public static class MapManager
    {
        public enum ObjectType
        {
            None,
            Wall,
            Prop,
            NPC,
            PathPoint,
            Floor,
            Misc,
            Door
        }


        public static ObservableCollection<ObservableCollection<Floor>> FlattenFloors(Floor[][] floors)
        {
            ObservableCollection<ObservableCollection<Floor>> flattenedArray = new ObservableCollection<ObservableCollection<Floor>>();
            foreach (var row in floors)
            {
                var nestedCollection = new ObservableCollection<Floor>();
                foreach (Floor floor in row)
                {
                    Floor newFloor = new Floor("/Resources/FloorTextures/Floor_JunkieTiles.png","");
                    nestedCollection.Add(newFloor);
                }
                flattenedArray.Add(nestedCollection);
            }
            return flattenedArray;
        }

        public static ResourceManager resourceManager = Resources.ResourceManager;

        public static Dictionary<int, Coordinate> houseSizeSchema = new Dictionary<int, Coordinate>()
        {
            { 0, new Coordinate(12,12) }

        };
    }
}
