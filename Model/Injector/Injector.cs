using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using BobMapper.Services;
using NetTopologySuite.Utilities;

namespace BobMapper.Model.Injector
{
    internal class Injector
    {
        const string filter = "resources.dat|resources.dat|Moddable Robbery Bob 1.zip|Moddable Robbery Bob 1.zip|All files (*.*)|*.*";
        string levelsXmlPath;
        string levelNamesLocale;
        string finalLevFileName;

        bool android;

        //FOR FRIDAY - Get FileStager atleast somewhat working

        internal Injector(string levFilePath, MapProperties mapProperties, bool buildApk, Map.Chapter chapter, int level)
        {
            FileDialogService fileDialogService = new FileDialogService();
            string destination = fileDialogService.LoadFileDialog(filter);
            switch (Path.GetExtension(destination))
            {
                default:
                    throw new Exception();
                case ".dat":
                    android = false;
                    break;
                case ".zip":
                    android = true;
                    break;
            }
            RetrieveFiles(destination);
            InsertLevel(levFilePath, destination, mapProperties.Name);
            FileStager fileStager = new FileStager(levelsXmlPath, levelNamesLocale, android, mapProperties, level, chapter, finalLevFileName);

            //Ask user where to inject, what level |||||DONE!
            //Retrieve level xml and set all the properties.
            //Retrieve other files and Inject the level name and level whatever
            //(COPY WITH NEW NAME LIKE BobMapper{propertyname w/o spaces} AND CHECK IF A FILE OF SUCH A NAME EXISTS, IF SO, APPEND NUMBER)
            //EXAMPLE: BobMapperHouse.lev BobMapperHouse2.lev BobMapperHouse3.lev and so on
            //Once all files have been edited, run powershell script to inject them in. If it's steam, open robbery bob
        }

        private void RetrieveFiles(string destination)
        {
            using ZipArchive archive = ZipFile.Open(destination, ZipArchiveMode.Update);
            ZipArchiveEntry levelsXmlEntry;
            ZipArchiveEntry levelsXmlEntryBackup;
            ZipArchiveEntry levelNamesLocaleEntry;
            ZipArchiveEntry levelNamesLocaleEntryBackup;
            if (android)
            {
                levelsXmlEntry = archive.GetEntry(@"Moddable Robbery Bob 1/assets/common/Levels/Levels.xml");
                levelsXmlEntryBackup = archive.GetEntry(@"Moddable Robbery Bob 1/assets/common/Levels/Levels.xml.backup");
                levelsXmlPath = Path.GetTempFileName();
                levelsXmlEntry.ExtractToFile(levelsXmlPath, overwrite: true);
                if (levelsXmlEntryBackup == null)
                {
                    archive.CreateEntryFromFile(levelsXmlPath, @"Moddable Robbery Bob 1/assets/common/Levels/Levels.xml.backup");
                }
                levelNamesLocale = Path.GetTempFileName();
                levelNamesLocaleEntry = archive.GetEntry(@"Moddable Robbery Bob 1/assets/localization/en.lproj/LevelNames.locale.csv");
                levelNamesLocaleEntryBackup = archive.GetEntry(@"Moddable Robbery Bob 1/assets/localization/en.lproj/LevelNames.locale.csv.backup");
                levelNamesLocaleEntry.ExtractToFile(levelNamesLocale, overwrite: true);
                if (levelNamesLocaleEntryBackup == null)
                {
                    archive.CreateEntryFromFile(levelNamesLocale, @"Moddable Robbery Bob 1/assets/localization/en.lproj/LevelNames.locale.csv.backup");
                }
            }
            else
            {
                levelsXmlEntry = archive.GetEntry(@"common/Levels/Levels.xml");
                levelsXmlEntryBackup = archive.GetEntry(@"common/Levels/Levels.xml.backup");
                levelsXmlPath = Path.GetTempFileName();
                levelsXmlEntry.ExtractToFile(levelsXmlPath, overwrite: true);
                if (levelsXmlEntryBackup == null)
                {
                    archive.CreateEntryFromFile(levelsXmlPath, @"common/Levels/Levels.xml.backup");
                }
                levelNamesLocale = Path.GetTempFileName();
                levelNamesLocaleEntry = archive.GetEntry(@"localization/en.lproj/LevelNames.locale.csv");
                levelNamesLocaleEntryBackup = archive.GetEntry(@"localization/en.lproj/LevelNames.locale.csv.backup");
                levelNamesLocaleEntry.ExtractToFile(levelNamesLocale, overwrite: true);
                if (levelNamesLocaleEntryBackup == null)
                {
                    archive.CreateEntryFromFile(levelNamesLocale, @"localization/en.lproj/LevelNames.locale.csv.backup");
                }
            }
        }

        private void InsertLevel(string levFilePath, string destination, string mapName)
        {
            string filename = "BobMapper";
            mapName = Regex.Replace(mapName, @"[^A-Za-z0-9]", "");
            finalLevFileName = filename + mapName;
            using ZipArchive archive = ZipFile.Open(destination, ZipArchiveMode.Update);
            if (android)
            {
                string entryDirectory = @"Moddable Robbery Bob 1/assets/common/Levels/" + finalLevFileName;
                ZipArchiveEntry levelEntry = archive.GetEntry(entryDirectory);
                int failSafeIndex = 1;
                string failSafeDirectory = entryDirectory;
                while (levelEntry != null)
                {
                    failSafeDirectory = entryDirectory + failSafeIndex;
                    levelEntry = archive.GetEntry(failSafeDirectory);
                    failSafeIndex++;
                }
                if(failSafeIndex > 1)
                {
                    finalLevFileName += failSafeIndex;
                }
                failSafeDirectory += ".lev";
                finalLevFileName += ".lev";
                archive.CreateEntryFromFile(levFilePath, failSafeDirectory);
            }
        }

        private void AndroidWrite(bool buildApk)
        {

        }

        private void SteamWrite()
        {

        }
    }
}
