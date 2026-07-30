using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BobMapper.Services;

namespace BobMapper.Model.Injector
{
    internal class Injector
    {
        const string filter = "resources.dat|resources.dat|Moddable Robbery Bob 1.zip|Moddable Robbery Bob 1.zip|All files (*.*)|*.*";
        string levelsXmlPath;
        string levelNamesLocale;

        bool android;

        //FOR FRIDAY - Get FileStager atleast somewhat working

        internal Injector(string levFilePath, Map map, bool buildApk, Map.Chapter chapter, int level) 
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
                    android= true;
                    break;
            }
            RetrieveFiles(destination);
            FileStager fileStager = new FileStager();

            //Ask user where to inject, what level |||||DONE!
            //Retrieve level xml and set all the properties.
            //Retrieve other files and Inject the level name and level whatever
            //(COPY WITH NEW NAME LIKE BobMapper{propertyname w/o spaces} AND CHECK IF A FILE OF SUCH A NAME EXISTS, IF SO, APPEND NUMBER)
            //EXAMPLE: BobMapperHouse.lev BobMapperHouse2.lev BobMapperHouse3.lev and so on
            //Once all files have been edited, run powershell script to inject them in. If it's steam, open robbery bob
        }

        private void RetrieveFiles(string destination)
        {
            using (ZipArchive archive = ZipFile.Open(destination, ZipArchiveMode.Update))
            {
                ZipArchiveEntry levelsXml;
                ZipArchiveEntry levelsXmlBackup;
                if (android)
                {
                    levelsXml = archive.GetEntry(@"Moddable Robbery Bob 1/assets/common/Levels/Levels.xml");
                    levelsXmlBackup = archive.GetEntry(@"Moddable Robbery Bob 1/assets/common/Levels/Levels.xml.backup");
                    levelsXmlPath = Path.GetTempFileName();
                    levelsXml.ExtractToFile(levelsXmlPath, overwrite:true);
                    if(levelsXmlBackup == null)
                    {
                        archive.CreateEntryFromFile(levelsXmlPath, @"Moddable Robbery Bob 1/assets/common/Levels/Levels.xml.backup");
                    }

                }
                else
                {

                }
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
