using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model;
using System.Text.Json;
using System.IO;
using System.Text.Json.Nodes;
using System.Windows.Navigation;

namespace BobMapper.Services
{
    internal static class JsonMapParse
    {
        internal static readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true, IncludeFields = true };

        internal static Map LoadData(string filename)
        {
            var jsonData = File.ReadAllText(filename);
            JsonObject root = JsonNode.Parse(jsonData).AsObject();
            if (!root.ContainsKey("mapProperties"))
            {
                jsonData = FixCompatibilityIssues(jsonData);
            }
            Map map = JsonSerializer.Deserialize<Map>(jsonData, jsonSerializerOptions);
            return map;
        }

        internal static void SaveData(Map map, string filename)
        {
            var jsonData = JsonSerializer.Serialize(map, jsonSerializerOptions);
            File.WriteAllText(filename.ToString(), jsonData);
        }

        internal static string GetMapJson(Map map)
        {
            var jsonData = JsonSerializer.Serialize(map, jsonSerializerOptions);
            return jsonData;
        }

        private static string FixCompatibilityIssues(string incompatibleJson)
        {
            JsonObject root = JsonNode.Parse(incompatibleJson).AsObject();
            int width = root["Width"].GetValue<int>();
            root.Remove("Width");
            int height = root["Height"].GetValue<int>();
            root.Remove("Height");
            int tileset = root["tileset"].GetValue<int>();
            root.Remove("tileset");
            root.Remove("levelNumber");
            root.Remove("levelChapter");
            root["mapProperties"] = JsonSerializer.SerializeToNode(new MapProperties(width, height, (Tilesets)tileset), jsonSerializerOptions);
            return root.ToJsonString(jsonSerializerOptions);
        }
    }
}
