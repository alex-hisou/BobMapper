using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BobMapper.Model.Injector
{
    public class FileStager
    {
        internal FileStager(string levelsXml, string levelNamesLocale, bool android, MapProperties mapProperties, int level, Map.Chapter chapter, string levFileName) 
        {
            StageLevelsXml(levelsXml, android, mapProperties, chapter, level, levFileName);
            StageLevelNamesLocale(levelsXml, android);
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
        }

        private void StageLevelNamesLocale(string levelNamesLocale, bool android)
        {

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
