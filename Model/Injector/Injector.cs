using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BobMapper.Data;
using BobMapper.Services;
using NetTopologySuite.Utilities;
using static BobMapper.Model.Injector.FileStager;

namespace BobMapper.Model.Injector
{
    internal class Injector
    {
        string tempLevelsXmlPath;
        string tempLevelNamesLocale;
        string finalLevEntryFileName;
        string destination;
        string tempDestination;
        ZipArchiveEntry levelsXmlEntry;
        ZipArchiveEntry levelNamesLocaleEntry;
        bool android;
        public bool Success { get; set; } = false;

        internal Injector(string destination, string tempLevFile, MapProperties mapProperties, bool buildApk, bool insertToSteam, Map.Chapter chapter, int level)
        {
            this.destination = destination;
            InitializeInjection(tempLevFile, mapProperties, buildApk, insertToSteam, chapter, level);
        }

        private void InitializeInjection(string tempLevFile, MapProperties mapProperties, bool buildApk, bool insertToSteam, Map.Chapter chapter, int level)
        {
            tempDestination = Path.GetTempFileName();
            File.Copy(destination, tempDestination, true);
            try
            {
                using (ZipArchive zip = ZipFile.Open(destination, ZipArchiveMode.Read))
                {
                    ZipArchiveEntry androidEntry = zip.GetEntry(@"Moddable Robbery Bob 1/assets/common/Levels/Levels.xml");
                    ZipArchiveEntry steamEntry = zip.GetEntry(@"common/Levels/Levels.xml");
                    if (androidEntry != null)
                        android = true;
                    else if (steamEntry != null)
                        android = false;
                    else { return; }
                }
            }
            catch (Exception ex) 
            { return; }
            CheckAndDecrypt();
            RetrieveFiles();
            InsertLevel(tempLevFile, mapProperties.Name);
            FileStager fileStager = new FileStager(tempLevelsXmlPath, tempLevelNamesLocale, android, mapProperties, level, chapter, finalLevEntryFileName);
            if (android)
            {
                AndroidWrite(buildApk);
            }
            else
            {
                SteamWrite(insertToSteam);
            }
            Success = true;
        }

        private void CheckAndDecrypt()
        {
            //Using dependency because dotnet library cannot check if the file is encrypted.
            using (ZipArchive zipArchive = ZipFile.Open(tempDestination, ZipArchiveMode.Update))
            {
                foreach (ZipArchiveEntry archiveEntry in zipArchive.Entries)
                {
                    if (!archiveEntry.IsEncrypted)
                    {
                        return;
                    }
                    else break;
                }
            }
            string currentDir = Directory.GetCurrentDirectory();
            currentDir = Path.Combine(currentDir, @"Model\Injector");
            string bkCrackPath = Path.Combine(currentDir, @"bkcrack");
            string xmlHeader = Path.Combine(currentDir, @"xmlHeader.txt");
            string script = Path.Combine(currentDir, @"BobMapper Decrypt Steam Resources.ps1");
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "powershell.exe";
            info.Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{script}\" -moddedPath \"{tempDestination}\" -bkCrackPath \"{bkCrackPath}\" -xmlHeader \"{xmlHeader}\"";
            info.UseShellExecute = true;
            info.Verb = "runas";
            using (Process process = Process.Start(info))
            {
                process.WaitForExit();
            }
        }

        private void RetrieveFiles()
        {
            using ZipArchive archive = ZipFile.Open(tempDestination, ZipArchiveMode.Update);
            ZipArchiveEntry levelsXmlEntryBackup;
            ZipArchiveEntry levelNamesLocaleEntryBackup;
            if (android)
            {
                levelsXmlEntry = archive.GetEntry(@"Moddable Robbery Bob 1/assets/common/Levels/Levels.xml");
                levelsXmlEntryBackup = archive.GetEntry(@"Moddable Robbery Bob 1/assets/common/Levels/Levels.xml.backup");
                tempLevelsXmlPath = Path.GetTempFileName();
                levelsXmlEntry.ExtractToFile(tempLevelsXmlPath, overwrite: true);
                if (levelsXmlEntryBackup == null)
                {
                    archive.CreateEntryFromFile(tempLevelsXmlPath, @"Moddable Robbery Bob 1/assets/common/Levels/Levels.xml.backup");
                }
                tempLevelNamesLocale = Path.GetTempFileName();
                levelNamesLocaleEntry = archive.GetEntry(@"Moddable Robbery Bob 1/assets/localization/en.lproj/LevelNames.locale.csv");
                levelNamesLocaleEntryBackup = archive.GetEntry(@"Moddable Robbery Bob 1/assets/localization/en.lproj/LevelNames.locale.csv.backup");
                levelNamesLocaleEntry.ExtractToFile(tempLevelNamesLocale, overwrite: true);
                if (levelNamesLocaleEntryBackup == null)
                {
                    archive.CreateEntryFromFile(tempLevelNamesLocale, @"Moddable Robbery Bob 1/assets/localization/en.lproj/LevelNames.locale.csv.backup");
                }
            }
            else
            {
                levelsXmlEntry = archive.GetEntry(@"common/Levels/Levels.xml");
                levelsXmlEntryBackup = archive.GetEntry(@"common/Levels/Levels.xml.backup");
                tempLevelsXmlPath = Path.GetTempFileName();
                levelsXmlEntry.ExtractToFile(tempLevelsXmlPath, overwrite: true);
                if (levelsXmlEntryBackup == null)
                {
                    archive.CreateEntryFromFile(tempLevelsXmlPath, @"common/Levels/Levels.xml.backup");
                }
                tempLevelNamesLocale = Path.GetTempFileName();
                levelNamesLocaleEntry = archive.GetEntry(@"localization/en.lproj/LevelNames.csv");
                levelNamesLocaleEntryBackup = archive.GetEntry(@"localization/en.lproj/LevelNames.csv.backup");
                levelNamesLocaleEntry.ExtractToFile(tempLevelNamesLocale, overwrite: true);
                if (levelNamesLocaleEntryBackup == null)
                {
                    archive.CreateEntryFromFile(tempLevelNamesLocale, @"localization/en.lproj/LevelNames.csv.backup");
                }
            }
        }

        private void InsertLevel(string tempLevFile, string mapName)
        {
            string filename = "BobMapper";
            mapName = Regex.Replace(mapName, @"[^A-Za-z0-9]", "");
            finalLevEntryFileName = filename + mapName;
            using ZipArchive archive = ZipFile.Open(tempDestination, ZipArchiveMode.Update);
            string noExtEntryDirectory = @"common/Levels/" + finalLevEntryFileName;
            if (android)
            {
                noExtEntryDirectory = @"Moddable Robbery Bob 1/assets/common/Levels/" + finalLevEntryFileName;
            }
            string extEntryDirectory = noExtEntryDirectory + ".lev";
            ZipArchiveEntry levelEntry = archive.GetEntry(extEntryDirectory);
            int failSafeIndex = 1;
            while (levelEntry != null)
            {
                failSafeIndex++;
                extEntryDirectory = noExtEntryDirectory + failSafeIndex + ".lev";
                levelEntry = archive.GetEntry(extEntryDirectory);
            }
            if (failSafeIndex > 1)
            {
                finalLevEntryFileName += failSafeIndex;
            }
            finalLevEntryFileName += ".lev";
            archive.CreateEntryFromFile(tempLevFile, extEntryDirectory);
        }

        private void AndroidWrite(bool buildApk)
        {
            using (ZipArchive zipArchive = ZipFile.Open(tempDestination, ZipArchiveMode.Update))
            {
                zipArchive.GetEntry(levelsXmlEntry.FullName).Delete();
                zipArchive.GetEntry(levelNamesLocaleEntry.FullName).Delete();
                zipArchive.CreateEntryFromFile(tempLevelsXmlPath, levelsXmlEntry.FullName);
                zipArchive.CreateEntryFromFile(tempLevelNamesLocale, levelNamesLocaleEntry.FullName);
            }
            File.Copy(tempDestination, destination, true);
            if(!buildApk)
            {
                return;
            }
            string unzippedApkParent = Directory.GetParent(destination).FullName;
            string unzippedApk = Path.Combine(unzippedApkParent, "Moddable Robbery Bob 1");
            if(!Directory.Exists(unzippedApk))
            {
                using (ZipArchive unzipArchive = ZipFile.Open(destination, ZipArchiveMode.Read))
                {
                    unzipArchive.ExtractToDirectory(unzippedApkParent);
                }
            }
            DirectoryInfo di = new DirectoryInfo(unzippedApk);
            di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            string currentDir = Directory.GetCurrentDirectory();
            string toolsDir = Path.Combine(currentDir, @"Model\Injector");
            currentDir = Path.Combine(currentDir, @"Model\Injector\BobMapper Android Injection Script.ps1");
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "powershell.exe";
            info.Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{currentDir}\" -moddedPath \"{unzippedApk}\" -toolsPath \"{toolsDir}\"";
            info.UseShellExecute = true;
            using (Process process = Process.Start(info))
            {
                process.WaitForExit();
            }
        }

        private void SteamWrite(bool insertToSteam)
        {
            using (ZipArchive zipArchive = ZipFile.Open(tempDestination, ZipArchiveMode.Update))
            {
                zipArchive.GetEntry(levelsXmlEntry.FullName).Delete();
                zipArchive.GetEntry(levelNamesLocaleEntry.FullName).Delete();
                zipArchive.CreateEntryFromFile(tempLevelsXmlPath, levelsXmlEntry.FullName);
                zipArchive.CreateEntryFromFile(tempLevelNamesLocale, levelNamesLocaleEntry.FullName);
            }
            if(!insertToSteam)
            {
                File.Copy(tempDestination, destination, true);
                return;
            }
            string currentDir = Directory.GetCurrentDirectory();
            currentDir = Path.Combine(currentDir, @"Model\Injector\BobMapper Steam Injection Script.ps1");
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "powershell.exe";
            info.Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{currentDir}\" -moddedPath \"{tempDestination}\" -destination \"{destination}\"";
            info.UseShellExecute = true;
            info.Verb = "runas";
            using (Process process = Process.Start(info))
            {
                process.WaitForExit();
            }
        }
    }
}
