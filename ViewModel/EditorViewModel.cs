using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BobMapper.Model;
using BobMapper.Model.Injector;
using BobMapper.Model.MapObjects;
using BobMapper.Services;
using BobMapper.View;
using BobMapper.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static BobMapper.Model.MapManager;

namespace BobMapper.ViewModel
{
    internal partial class EditorViewModel : ViewModelBase
    {
        public Selections CurrentSelections { get; set; }
        public ViewportData CurrentViewportData { get; set; }
        public GizmoData CurrentGizmoData { get; set; }
        public string FileName { get; set; }
        public LayerData CurrentLayerData { get; set; }
        public ObjectCollection CurrentObjectCollection { get; set; }
        public Map CurrentMap { get; set; }
        public MapProperties CurrentMapProperties { get; set; }
        public EditingInteractions CurrentEditingInteractions { get; set; }

        public EditorViewModel(string filename)
        {
            FileName = filename;
            CurrentMap = JsonMapParse.LoadData(filename);
            CurrentMapProperties = CurrentMap.mapProperties;
            CurrentMapProperties.TilesetChanged += ChangeTileset;
            CurrentViewportData = new ViewportData
            {
                ViewOffsetX = CurrentMapProperties.Width / -2,
                ViewOffsetY = CurrentMapProperties.Height / -2,
                CameraX = 0,
                CameraY = 0,
                ZoomX = 1,
                ZoomY = -1
            };
            CurrentSelections = new Selections();
            CurrentGizmoData = new GizmoData(CurrentSelections);
            CurrentLayerData = new LayerData();
            if (CurrentMap.exitZones == null)
                CurrentMap.exitZones = new();
            CurrentObjectCollection = new ObjectCollection
            {
                CurrentProps = new ObservableCollection<Prop>(CurrentMap.props),
                CurrentWalls = new ObservableCollection<Wall>(CurrentMap.walls),
                CurrentNPCs = new ObservableCollection<NPC>(CurrentMap.npcs),
                CurrentPathPoints = new ObservableCollection<PathPoint>(CurrentMap.pathPoints),
                CurrentMiscs = new ObservableCollection<Misc>(CurrentMap.miscs),
                CurrentFloors = new ObservableCollection<ObservableCollection<Floor>>(FlattenFloors(CurrentMap.floors)),
                CurrentDoors = new ObservableCollection<Door>(CurrentMap.doors),
                CurrentLoots = new ObservableCollection<Loot>(CurrentMap.loots),
                CurrentExitZones = new ObservableCollection<ExitZone>(CurrentMap.exitZones)
            };
            CurrentEditingInteractions = new(CurrentObjectCollection, CurrentSelections, CurrentMapProperties);
            CurrentEditingInteractions.AttachAllPathPointHandlers();
            CurrentSelections.GetFilteredTextureSet(TextureType.All, CurrentMapProperties.Tileset);
            CurrentSelections.SelectedTextureType = TextureType.All;
        }

        private void ChangeTileset(object sender, EventArgs e)
        {
            CurrentSelections.GetFilteredTextureSet(CurrentSelections.SelectedTextureType, CurrentMapProperties.Tileset);
            foreach(Wall wall in CurrentObjectCollection.CurrentWalls)
            {
                wall.Texture1 = ValidateTexture(wall.Texture1, TextureType.Wall, CurrentMapProperties.Tileset, true);
                wall.Texture2 = ValidateTexture(wall.Texture2, TextureType.Wall, CurrentMapProperties.Tileset, true);
            }
            foreach(Prop prop in CurrentObjectCollection.CurrentProps)
            {
                prop.PropTexture = ValidateTexture(prop.PropTexture, TextureType.Prop, CurrentMapProperties.Tileset, true);
            }
            foreach(Loot loot in CurrentObjectCollection.CurrentLoots)
            {
                loot.Texture = ValidateTexture(loot.Texture, TextureType.Loot, CurrentMapProperties.Tileset, true);
            }
            foreach(Door door in CurrentObjectCollection.CurrentDoors)
            {
                door.Texture1 = ValidateTexture(door.Texture1, TextureType.Door, CurrentMapProperties.Tileset, true);
            }
            foreach(var floorRow in CurrentObjectCollection.CurrentFloors)
            {
                foreach(Floor floor in floorRow)
                {
                    floor.Texture1 = ValidateTexture(floor.Texture1, TextureType.Floor, CurrentMapProperties.Tileset, true);
                }
            }
        }

        [RelayCommand]
        public void SelectTool(Tools tool)
        {
            if (CurrentSelections.SelectedTool != tool)
            {
                CurrentSelections.SelectedTool = tool;
            }
            else { CurrentSelections.SelectedTool = Tools.None;  }
        }

        public void ClickEmpty(Coordinate placementPos)
        {
            CurrentEditingInteractions.HandleClickEmpty(placementPos);
        }

        public bool CheckForChanges()
        {
            string mapFileString = File.ReadAllText(FileName);
            string currentMapString = JsonMapParse.GetMapJson(CurrentMap);
            if(mapFileString == currentMapString)
            {
                return false;
            }
            else return true;
        }

        [RelayCommand]
        internal void Save(bool saveNewFile)
        {
            CurrentMap.walls = CurrentObjectCollection.CurrentWalls.ToList();
            CurrentMap.doors = CurrentObjectCollection.CurrentDoors.ToList();
            CurrentMap.props = CurrentObjectCollection.CurrentProps.ToList();
            CurrentMap.pathPoints = CurrentObjectCollection.CurrentPathPoints.ToList();
            CurrentMap.npcs = CurrentObjectCollection.CurrentNPCs.ToList();
            CurrentMap.miscs = CurrentObjectCollection.CurrentMiscs.ToList();
            CurrentMap.loots = CurrentObjectCollection.CurrentLoots.ToList();
            CurrentMap.floors = SaveFloor();
            CurrentMap.exitZones = CurrentObjectCollection.CurrentExitZones.ToList();
            if(saveNewFile)
            {
                FileDialogService fileDialogService = new FileDialogService();
                FileName = fileDialogService.SaveFileDialog("BobMapper Map File (.bobmap)|*.bobmap", ".bobmap");
            }
            JsonMapParse.SaveData(CurrentMap, FileName);
        }

        private Floor[][] SaveFloor()
        {
            Floor[][] jaggedFloor = new Floor[CurrentObjectCollection.CurrentFloors.Count][];
            for (int i = 0; i < CurrentObjectCollection.CurrentFloors.Count; i++)
            {
                var currentColumn = CurrentObjectCollection.CurrentFloors[i];
                Floor[] floorRow = new Floor[currentColumn.Count];
                for (int j = 0; j < currentColumn.Count; j++)
                {
                    floorRow[j] = currentColumn[j];
                }
                jaggedFloor[i] = floorRow;
            }
            return jaggedFloor;
        }

        [RelayCommand]
        internal void Compile()
        {
            CurrentMap.floors = SaveFloor();
            FileDialogService fileDialogService = new FileDialogService();
            string compileFilePath = fileDialogService.SaveFileDialog("Compiled map (*.lev)|*.lev", ".lev");
            if (string.IsNullOrEmpty(compileFilePath))
            {
                return;
            }
            if (File.Exists(compileFilePath))
            {
                File.Delete(compileFilePath);
            }
            Compiler.Compiler compiler = new Compiler.Compiler();
            compiler.Compile(CurrentMap);
            File.WriteAllBytes(compileFilePath, Compiler.Compiler.output.ToArray());
            Process.Start("explorer.exe", $"/select,\"{compileFilePath}\"");
        }

        [RelayCommand]
        internal void Inject(bool buildApk)
        {
            Save(false);
            InjectorPrompt injectorPrompt = new InjectorPrompt();
            injectorPrompt.Show();
            EventHandler<LevelInjectPromptEventArgs> injectionPromptComplete = null!;
            injectionPromptComplete = (sender, e) =>
            {
                string tempLevFile = Path.GetTempFileName();
                Compiler.Compiler compiler = new();
                compiler.Compile(CurrentMap);
                File.WriteAllBytes(tempLevFile, Compiler.Compiler.output.ToArray());
                string filter = "resources.dat|resources.dat|Moddable Robbery Bob 1.zip|Moddable Robbery Bob 1.zip|All files (*.*)|*.*";
                FileDialogService fileDialogService = new FileDialogService();
                string destination = fileDialogService.LoadFileDialog(filter);
                Injector injector = new(destination, tempLevFile, CurrentMapProperties, buildApk, false, e.Chapter, e.Level);
                injectorPrompt.ConfirmationComplete -= injectionPromptComplete;
            };
            injectorPrompt.ConfirmationComplete += injectionPromptComplete;
        }

        [RelayCommand]
        internal void SteamInject()
        {
            if(string.IsNullOrEmpty(UserSettings.Instance.SteamResourcesDirectory))
            {
                MessageBox.Show("To Inject to Steam, please set the Steam directory in Prefrences", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Save(false);
            InjectorPrompt injectorPrompt = new InjectorPrompt();
            injectorPrompt.Show();
            EventHandler<LevelInjectPromptEventArgs> injectionPromptComplete = null!;
            injectionPromptComplete = (sender, e) =>
            {
                string tempLevFile = Path.GetTempFileName();
                Compiler.Compiler compiler = new();
                compiler.Compile(CurrentMap);
                File.WriteAllBytes(tempLevFile, Compiler.Compiler.output.ToArray());
                string destination = UserSettings.Instance.SteamResourcesDirectory;
                Injector injector = new(destination, tempLevFile, CurrentMapProperties, false, true, e.Chapter, e.Level);
                injectorPrompt.ConfirmationComplete -= injectionPromptComplete;
            };
            injectorPrompt.ConfirmationComplete += injectionPromptComplete;
        }
    }
}
