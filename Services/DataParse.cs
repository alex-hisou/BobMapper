using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model;
using System.Text.Json;
using System.IO;

namespace BobMapper.Services
{
    internal static class DataParse
    {
        private static FileInfo saveFile = new FileInfo("examplemap.json");
        private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };

        internal static Map LoadData()
        {
            var jsonData = File.ReadAllText(saveFile.ToString());
            Map map = new Map();
            map = JsonSerializer.Deserialize<Map>(jsonData, jsonSerializerOptions);
            return map;
        }

        internal static void SaveData(Map map)
        {
            var wrapper = new { map.walls, map.props, map.npcs, map.pathPoints, map.miscs };
            var jsonData = JsonSerializer.Serialize(wrapper, jsonSerializerOptions);
            File.WriteAllText(saveFile.ToString(), jsonData);
        }
    }
}
