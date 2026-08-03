using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CsvHelper;
using CsvHelper.Configuration;

namespace BobMapper.Model.Injector
{
    public class FileStager
    {
        internal FileStager(string levelsXml, string levelNamesLocale, bool android, MapProperties mapProperties, int level, Map.Chapter chapter, string levFileName) 
        {
            StageLevelsXml(levelsXml, android, mapProperties, chapter, level, levFileName);
            StageLevelNamesLocale(levelNamesLocale, android, chapter, level, mapProperties.Name);
        }

        private void StageLevelsXml(string levelsXml, bool android, MapProperties mapProperties, Map.Chapter chapter, int level, string levFileName)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Levels));
            FileStream fileStream = new(levelsXml, FileMode.Open);
            Levels root = (Levels)xmlSerializer.Deserialize(fileStream);
            Chapter targetChapter = root.Chapters.First(x => x.Id == chapter.ToString());
            Level targetLevel = targetChapter.Levels[level - 1];
            targetLevel.Filename = levFileName;
            targetLevel.IsBobMapper = true;
            targetLevel.Shadows = mapProperties.IsNightTime;
            if(mapProperties.IsApartment)
            {
                if(android)
                {
                    string androidBackground = androidBackgrounds[mapProperties.BackgroundImage];
                    targetLevel.Background = androidBackground;
                }
                else 
                {
                    targetLevel.Background = mapProperties.BackgroundImage;
                }
                targetLevel.BackgroundHeight = mapProperties.Height;
            }
            else
            {
                targetLevel.Background = null;
                targetLevel.BackgroundHeight = 0;
            }
            fileStream.SetLength(0);
            xmlSerializer.Serialize(fileStream, root);
            fileStream.Close();
        }

        private void StageLevelNamesLocale(string levelNamesLocale, bool android, Map.Chapter chapter, int level, string mapName)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                ShouldQuote = args => true
            };
            string temporaryLocale = Path.GetTempFileName();
            var reader = new StreamReader(levelNamesLocale);
            var csvReader = new CsvReader(reader, config);
            var writer = new StreamWriter(temporaryLocale);
            var csvWriter = new CsvWriter(writer, config);
            bool alreadyMatched = false;
            while (csvReader.Read())
            {
                string internalName = csvReader.GetField(0);
                string displayName = csvReader.GetField(1);
                string chapterName = chapter.ToString();
                if (Regex.IsMatch(internalName, $@"{chapterName}.*{level}", RegexOptions.IgnoreCase) && !alreadyMatched)
                {
                    displayName = mapName;
                    alreadyMatched = true;
                }
                var record = new { InternalName = internalName, DisplayName = displayName };
                csvWriter.WriteRecord(record);
            }
            csvWriter.Flush();
            reader.Dispose();
            writer.Dispose(); //just in case to prevent conflicts
            char[] unparsedCsv = File.ReadAllText(temporaryLocale).ToCharArray();
            //WORST SOLUTION OF ALL TIME
            bool evenComma = false;
            for (int i = 0; i < unparsedCsv.Length; i++)
            {
                char c = unparsedCsv[i];
                if (c != ',')
                    continue;
                if (evenComma)
                {
                    unparsedCsv[i] = '\n';
                }
                evenComma = !evenComma;
            }
            string parsedCsv = new string(unparsedCsv);
            File.WriteAllText(temporaryLocale, parsedCsv);
            File.Move(temporaryLocale, levelNamesLocale, true);
        }

        private Dictionary<string, string> androidBackgrounds = new Dictionary<string, string>()
        {
            {"LevelGfx/Chapter2/BackgroundDownTown1.png" ,"BackgroundDownTown1"},
            {"LevelGfx/Chapter2/BackgroundDownTown1_2.png", "BackgroundDownTown1_2"},
            {"LevelGfx/Chapter2/BackgroundDownTown2.png", "BackgroundDownTown2"}
        };

        [XmlRoot]
        public class Levels
        {
            [XmlElement("Chapter")]
            public List<Chapter> Chapters { get; set; }
        }

        public class Chapter
        {
            [XmlAttribute("id")]
            public string Id { get; set; }

            [XmlElement("Level")]
            public List<Level> Levels { get; set; }
        }

        public class Level
        {
            [XmlAttribute("filename")]
            public string Filename { get; set; }

            [XmlAttribute("objective")]
            public string Objective { get; set; }

            [XmlAttribute("background")]
            public string Background { get; set; }

            public bool ShouldSerializeBackground()
            {
                return !string.IsNullOrEmpty(Background);
            }

            [XmlAttribute("background-height")]
            public float BackgroundHeight { get; set; }

            public bool ShouldSerializeBackgroundHeight()
            {
                return BackgroundHeight != 0;
            }

            [XmlAttribute("shadows")]
            public bool Shadows { get; set; }

            public bool ShouldSerializeShadows()
            {
                return Shadows;
            }

            [XmlAttribute("BobMapper")]
            public bool IsBobMapper { get; set; }

            public bool ShouldSerializeIsBobMapper()
            {
                return IsBobMapper;
            }

            [XmlAttribute("disable-path")]
            public bool DisablePath { get; set; }

            public bool ShouldSerializeDisablePath()
            {
                return DisablePath;
            }
        }
    }
}
