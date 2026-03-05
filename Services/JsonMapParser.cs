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
        internal static readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true, IncludeFields = true };

        internal static Map LoadData(string filename)
        {
            var jsonData = File.ReadAllText(filename);
            Map map = JsonSerializer.Deserialize<Map>(jsonData, jsonSerializerOptions);
            map.AttachAllPathPointHandlers();
            return map;
        }

        internal static void SaveData(Map map, string filename)
        {
            var jsonData = JsonSerializer.Serialize(map, jsonSerializerOptions);
            File.WriteAllText(filename.ToString(), jsonData);
        }
    }
}
