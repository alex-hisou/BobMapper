using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;
using BobMapper.Services;
using System.Text.Json.Serialization;

namespace BobMapper.Data
{
    public class UserSettings
    {
        private static string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "BobMapper");
        private static string settingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            @"BobMapper/UserSettings.json");

        [JsonInclude]
        public string SteamResourcesDirectory { get; set; } = null;

        [JsonInclude]
        public bool AutoSelect { get; set; } = true;

        private static UserSettings instance;

        public static UserSettings Instance
        {
            get 
            {
                if(instance == null)
                {
                    instance = Load();
                }
                return instance;
            }
        }

        [JsonConstructor]
        public UserSettings(bool AutoSelect, string SteamResourcesDirectory)
        {
            this.AutoSelect = AutoSelect;
            this.SteamResourcesDirectory = SteamResourcesDirectory;
        }

        public void Save()
        {
            string json = JsonSerializer.Serialize(instance, JsonMapParse.jsonSerializerOptions);
            File.WriteAllText(settingsFile, json);
        }

        private static UserSettings Load()
        {
            UserSettings userSettings = new(true, null);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            if (!File.Exists(settingsFile))
            {
                string saveJson = JsonSerializer.Serialize(userSettings, JsonMapParse.jsonSerializerOptions);
                File.WriteAllText(settingsFile, saveJson);
                return userSettings;
            }
            string json = File.ReadAllText(settingsFile);
            return JsonSerializer.Deserialize<UserSettings>(json, JsonMapParse.jsonSerializerOptions) ?? userSettings;
        }
    }
}
