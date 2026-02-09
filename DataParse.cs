using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model;
using System.Text.Json;
using System.IO;

namespace BobMapper
{
    internal static class DataParse
    {
        private static FileInfo saveFile = new FileInfo("D:\\Hobbies\\Coding\\BobMapper\\examplemap.json");

        internal static void LoadData(Map map)
        {

        }

        internal static void SaveData(Map map)
        {
            var wrapper = new { map.walls, map.props, map.npcs, map.pathPoints, map.miscs };
            var jsonData = JsonSerializer.Serialize(wrapper, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(saveFile.ToString(), jsonData);
        }
    }
}
