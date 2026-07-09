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
            Door,
            Loot
        }


        public static ObservableCollection<ObservableCollection<Floor>> FlattenFloors(Floor[][] floors)
        {
            ObservableCollection<ObservableCollection<Floor>> flattenedArray = new ObservableCollection<ObservableCollection<Floor>>();
            for (int i = 0; i < floors.Length; i++)
            {
                var nestedCollection = new ObservableCollection<Floor>();
                for (int j = 0; j < floors[i].Length; j++)
                {
                    Floor newFloor = floors[i][j];
                    if (string.IsNullOrEmpty(newFloor.InternalTexture1))
                    {
                        newFloor.InternalTexture1 = "/Resources/FloorTextures/Floor_JunkieTiles.png";
                    }
                    if (string.IsNullOrEmpty(newFloor.InternalTexture2))
                    {
                        newFloor.InternalTexture2 = "/Resources/FloorTextures/Floor_JunkieTiles.png";
                    }
                    nestedCollection.Add(newFloor);
                }
                flattenedArray.Add(nestedCollection);
            }
            return flattenedArray;
        }

        public static ResourceManager resourceManager = Resources.ResourceManager;
    }
}
