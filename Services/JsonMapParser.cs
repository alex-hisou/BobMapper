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
    internal static class JsonMapParse
    {
        private static readonly FileInfo saveFile = new("examplemap.json");
        private static readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true, IncludeFields = true };

        internal static Map LoadData()
        {
            var jsonData = File.ReadAllText(saveFile.ToString());
            Map map = JsonSerializer.Deserialize<Map>(jsonData, jsonSerializerOptions);
            return map;
        }

        internal static void SaveData(Map map)
        {
            var jsonData = JsonSerializer.Serialize(map, jsonSerializerOptions);
            File.WriteAllText(saveFile.ToString(), jsonData);
        }
    }
}
