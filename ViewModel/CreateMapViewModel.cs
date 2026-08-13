using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BobMapper.Model;
using BobMapper.Services;
using CommunityToolkit.Mvvm.Input;

namespace BobMapper.ViewModel
{
    internal partial class CreateMapViewModel : ViewModelBase
    {
        private int sizeX;

        public int SizeX
        {
            get { return sizeX; }
            set { sizeX = value; }
        }

        private int sizeY;

        public int SizeY
        {
            get { return sizeY; }
            set { sizeY = value; }
        }

        private Tilesets selectedTileset;

        public Tilesets SelectedTileset
        {
            get { return selectedTileset; }
            set { selectedTileset = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        public Array TilesetEnum => Enum.GetValues(typeof(Tilesets));

        public CreateMapViewModel()
        {
            SizeX = 0;
            SizeY = 0;
            SelectedTileset = Tilesets.Suburbs;
        }

        internal void CreateMap()
        {
            if (SizeX <= 0 || SizeY <= 0)
                return;
            var dialog = new Microsoft.Win32.SaveFileDialog();
            FileDialogService dialogService = new FileDialogService();
            string filename = dialogService.SaveFileDialog("BobMapper Map File (.bobmap)|*.bobmap",
                    ".bobmap", $"{Name}.bobmap");
            if (string.IsNullOrEmpty(filename))
                return;
            MapProperties mapProperties = new(SizeX, SizeY, selectedTileset, Name);
            Map map = new Map(mapProperties);
            string emptyJson = JsonSerializer.Serialize(map, JsonMapParse.jsonSerializerOptions);
            File.WriteAllText(filename, emptyJson);
            Editor editor = new Editor(filename);
            editor.Show();
            
        }


    }
}
