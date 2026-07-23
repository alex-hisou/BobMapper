using System;
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
using BobMapper.Model.MapObjects;
using BobMapper.Properties;
using Microsoft.Data.Sqlite;

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

        public static string ValidateTexture(string inputTexture, TextureType textureType, Tilesets tileset, bool setDefault)
        {
            SqliteConnection textureManifestConnection = new("Data Source=Data/TextureManifest.sqlite");
            textureManifestConnection.Open();
            var selectTexturesCommand = textureManifestConnection.CreateCommand();
            selectTexturesCommand.CommandText = $"SELECT ResourceName FROM Textures WHERE Tilesets LIKE '%{(int)tileset}%' AND Type LIKE '%{(int)textureType}%' AND ResourceName LIKE '%{inputTexture}%'";
            var reader = selectTexturesCommand.ExecuteReader();
            string query = "";
            while (reader.Read())
            {
                query = reader.GetString(0);
            }
            if(string.IsNullOrEmpty(query))
            {
                if (!setDefault)
                {
                    return query;
                }
                return GetDefaultTexture(textureType, tileset);
            }
            return query;
        }


        public static string GetDefaultTexture(TextureType textureType, Tilesets tileset)
        {
            switch(textureType)
            {
                case TextureType.Wall:
                    if(tileset == Tilesets.Strip || tileset == Tilesets.Winter)
                    {
                        return "/Resources/WallTextures/Wall_brown_striped.png";
                    }
                    return "/Resources/WallTextures/Wall_Plain_Blue.png";
                case TextureType.Floor:
                    if (tileset == Tilesets.Strip || tileset == Tilesets.Winter)
                    {
                        return "/Resources/FloorTextures/Floor_wstone_centre.png";
                    }
                    return "/Resources/FloorTextures/Floor_WhiteTiles.png";
                case TextureType.Door:
                    if(tileset == Tilesets.Strip || tileset == Tilesets.Winter)
                    {
                        return "/Resources/DoorTextures/Door_indoor_white.png";
                    }
                    if(tileset == Tilesets.Labs)
                    {
                        return "/Resources/DoorTextures/Door_Lab1.png";
                    }
                    return "/Resources/DoorTextures/Door_Blue.png";
                case TextureType.Loot:
                    if (tileset == Tilesets.Downtown)
                    {
                        return "/Resources/LootTextures/Loot_Laptop.png";
                    }
                    if (tileset == Tilesets.Labs)
                    {
                        return "/Resources/LootTextures/Loot_Shard.png";
                    }
                    return "/Resources/LootTextures/Loot_BrokenRecord.png";
                case TextureType.Prop:
                    if(tileset == Tilesets.Camp)
                    {
                        return "/Resources/PropTextures/Outside_GrillBig.png";
                    }
                    return "/Resources/PropTextures/ColaMachine.png";
                default:
                    throw new NotImplementedException();
            }
        }

        public static ResourceManager resourceManager = Resources.ResourceManager;
    }
}
