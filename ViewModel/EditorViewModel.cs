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
        public TwoPointToolsData TwoPointToolsData { get; set; }

        public EditorViewModel(string filename)
        {
            FileName = filename;
            CurrentMap = JsonMapParse.LoadData(filename);
            CurrentMapProperties = CurrentMap.mapProperties;
            CurrentMapProperties.TilesetChanged += ChangeTileset;
            CurrentMapProperties.IsApartmentChanged += UpdateFloorOpacities;
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
            TwoPointToolsData = new TwoPointToolsData();
            CurrentEditingInteractions = new(CurrentObjectCollection, CurrentSelections, CurrentMapProperties);
            CurrentEditingInteractions.AttachAllPathPointHandlers();
            CurrentSelections.CurrentTileSet = CurrentMapProperties.Tileset;
            CurrentSelections.SelectedTextureType = TextureType.All;
        }

        private void ChangeTileset(object sender, EventArgs e)
        {
            CurrentSelections.CurrentTileSet = CurrentMapProperties.Tileset;
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

        public void UpdateFloorOpacities(object sender, EventArgs e)
        {
            foreach(var floorRow in CurrentObjectCollection.CurrentFloors)
            {
                foreach (var floor in floorRow)
                {
                    floor.SetOpacity(CurrentMapProperties.IsApartment);
                }
            }
        }

        internal void ShiftObjects(int northOffset, int eastOffset, int westOffset, int southOffset)
        {
            //Man-made horrors beyond comprehension lay here
            CurrentObjectCollection.CurrentFloors = new ObservableCollection<ObservableCollection<Floor>>(FlattenFloors(CurrentMap.floors));
            CurrentViewportData.ViewOffsetX = CurrentMapProperties.Width / -2;
            CurrentViewportData.ViewOffsetY = CurrentMapProperties.Height / -2;
            foreach(Prop prop in CurrentObjectCollection.CurrentProps)
                ShiftObjectCoordinates(prop.Coordinates, northOffset, eastOffset, westOffset, southOffset);
            foreach(PathPoint pathPoint in  CurrentObjectCollection.CurrentPathPoints)
                ShiftObjectCoordinates(pathPoint.Coordinates, northOffset, eastOffset, westOffset, southOffset);
            foreach(Misc misc in CurrentObjectCollection.CurrentMiscs)
                ShiftObjectCoordinates(misc.Coordinates, northOffset, eastOffset, westOffset, southOffset);
            foreach(NPC nPC in CurrentObjectCollection.CurrentNPCs)
                ShiftObjectCoordinates(nPC.Coordinates, northOffset, eastOffset, westOffset, southOffset);
            foreach(Loot loot in CurrentObjectCollection.CurrentLoots)
                ShiftObjectCoordinates(loot.Coordinates, northOffset, eastOffset, westOffset, southOffset);
            foreach (Wall wall in CurrentObjectCollection.CurrentWalls)
            {
                ShiftObjectCoordinates(wall.Point1, northOffset, eastOffset, westOffset, southOffset);
                ShiftObjectCoordinates(wall.Point2, northOffset, eastOffset, westOffset, southOffset);
            }
            foreach(Door door in CurrentObjectCollection.CurrentDoors)
            {
                ShiftObjectCoordinates(door.Point1, northOffset, eastOffset, westOffset, southOffset);
                ShiftObjectCoordinates(door.Point2, northOffset, eastOffset, westOffset, southOffset);
            }
            foreach(ExitZone exitZone in CurrentObjectCollection.CurrentExitZones)
            {
                ShiftObjectCoordinates(exitZone.Point1, northOffset, eastOffset, westOffset, southOffset);
                ShiftObjectCoordinates(exitZone.Point2, northOffset, eastOffset, westOffset, southOffset);
                ShiftObjectCoordinates(exitZone.Point3, northOffset, eastOffset, westOffset, southOffset);
                ShiftObjectCoordinates(exitZone.Point4, northOffset, eastOffset, westOffset, southOffset);
            }
        }

        [RelayCommand]
        public void SelectTool(Tools tool)
        {
            TwoPointToolsData.IsVisible = false;
            if (CurrentSelections.SelectedTool != tool)
            {
                CurrentSelections.SelectedTool = tool;
            }
            else { CurrentSelections.SelectedTool = Tools.None;  }
            switch (CurrentSelections.SelectedTool)
            {
                case Tools.AddWall:
                    CurrentSelections.SelectedTextureType = TextureType.Wall;
                    TwoPointToolsData.IsVisible = true;
                    break;
                case Tools.AddProp:
                    CurrentSelections.SelectedTextureType = TextureType.Prop;
                    break;
                case Tools.AddLoot:
                    CurrentSelections.SelectedTextureType = TextureType.Loot;
                    break;
                case Tools.ChangeFloor:
                    CurrentSelections.SelectedTextureType = TextureType.Floor;
                    break;
                case Tools.AddDoor:
                    CurrentSelections.SelectedTextureType = TextureType.Door;
                    TwoPointToolsData.IsVisible = true;
                    break;
            }
        }

        public void ClickEmpty(Coordinate placementPos)
        {
            if(TwoPointToolsData.IsVisible)
            {
                TwoPointToolsData.IsDragging = true;
                return;
            }
            float unsnappedX = (placementPos.XPos - CurrentViewportData.CameraX) / (float)CurrentViewportData.ZoomX;
            float unsnappedY = (placementPos.YPos + CurrentViewportData.CameraY) / (float)CurrentViewportData.ZoomX;
            placementPos = new(unsnappedX, unsnappedY);
            CurrentEditingInteractions.HandleClickEmpty(placementPos);
        }

        public void MoveMouse(Coordinate mousePos)
        {
            if (!TwoPointToolsData.IsVisible)
                return;
            float unsnappedX = (mousePos.XPos - CurrentViewportData.CameraX) / (float)CurrentViewportData.ZoomX;
            float unsnappedY = (mousePos.YPos + CurrentViewportData.CameraY) / (float)CurrentViewportData.ZoomX;
            SnapCoordinate snapCoordinate = SnapCoordinate.UnsnappedCoordinateFactory(unsnappedX, unsnappedY);
            TwoPointToolsData.HandleMouseMove(snapCoordinate);
        }

        [RelayCommand]
        public void ReleaseMouse()
        {
            if (!TwoPointToolsData.IsVisible)
                return;
            SnapCoordinate startCoordinate = new(TwoPointToolsData.StartCoordinate.SnappedXPos, TwoPointToolsData.StartCoordinate.SnappedYPos);
            SnapCoordinate endCoordinate = new(TwoPointToolsData.EndCoordinate.SnappedXPos, TwoPointToolsData.EndCoordinate.SnappedYPos);
            if (startCoordinate.XPos == endCoordinate.XPos && startCoordinate.YPos == endCoordinate.YPos)
            {
                TwoPointToolsData.IsDragging = false;
                return;
            }
            if (CurrentSelections.SelectedTool == Tools.AddWall)
            {
                string validWallTexture = ValidateTexture(CurrentSelections.SelectedTexture, TextureType.Wall, CurrentMapProperties.Tileset, true);
                Wall wall = new Wall(startCoordinate, endCoordinate, Wall.WallType.Normal, validWallTexture, validWallTexture);
                CurrentObjectCollection.CurrentWalls.Add(wall);
                if (UserSettings.Instance.AutoSelect)
                {
                    CurrentEditingInteractions.SelectObject(wall);
                }
            }
            else
            {
                string validDoorTexture = ValidateTexture(CurrentSelections.SelectedTexture, TextureType.Door, CurrentMapProperties.Tileset, true);
                Door door = new Door(startCoordinate, endCoordinate, CurrentSelections.SelectedTexture, false, false, false);
                CurrentObjectCollection.CurrentDoors.Add(door);
                if (UserSettings.Instance.AutoSelect)
                {
                    CurrentEditingInteractions.SelectObject(door);
                }
            }
            TwoPointToolsData.StartCoordinate.SnappedXPos = endCoordinate.SnappedXPos;
            TwoPointToolsData.StartCoordinate.SnappedYPos = endCoordinate.SnappedYPos;
            TwoPointToolsData.IsDragging = false;
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
                FileName = fileDialogService.SaveFileDialog("BobMapper Map File (.bobmap)|*.bobmap",
                    ".bobmap", $"{CurrentMap.mapProperties.Name}.bobmap");
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
            string msgboxtext = "This option has been depricated. New features, such as apartment levels, cannot be generated with it. Continue?";
            var result = MessageBox.Show(msgboxtext, "Warning", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }
            CurrentMap.floors = SaveFloor();
            FileDialogService fileDialogService = new FileDialogService();
            string compileFilePath = fileDialogService.SaveFileDialog("Compiled map (*.lev)|*.lev", ".lev", $"{CurrentMapProperties.Name}.lev");
            if (string.IsNullOrEmpty(compileFilePath))
            {
                return;
            }
            Compiler.Compiler compiler = new Compiler.Compiler();
            compiler.Compile(CurrentMap);
            File.Delete(compileFilePath);
            File.WriteAllBytes(compileFilePath, compiler.output.ToArray());
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
                File.Delete(tempLevFile);
                File.WriteAllBytes(tempLevFile, compiler.output.ToArray());
                string filter = "resources.dat|resources.dat|Moddable Robbery Bob 1.zip|Moddable Robbery Bob 1.zip|All files (*.*)|*.*";
                if(buildApk)
                {
                    filter = "Moddable Robbery Bob 1.zip|Moddable Robbery Bob 1.zip|All files (*.*)|*.*";
                }
                FileDialogService fileDialogService = new FileDialogService();
                string destination = fileDialogService.LoadFileDialog(filter);
                Injector injector = new(destination, tempLevFile, CurrentMapProperties, buildApk, false, e.Chapter, e.Level);
                if (!injector.Success)
                {
                    MessageBox.Show("Something went wrong during injection. Make sure you selected the right file. If this problem presists with the right file selected, contact the developer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
                File.WriteAllBytes(tempLevFile, compiler.output.ToArray());
                string destination = UserSettings.Instance.SteamResourcesDirectory;
                Injector injector = new(destination, tempLevFile, CurrentMapProperties, false, true, e.Chapter, e.Level);
                if(!injector.Success)
                {
                    MessageBox.Show("Something went wrong during injection. Make sure you selected the right file. If this problem presists with the right file selected, contact the developer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                injectorPrompt.ConfirmationComplete -= injectionPromptComplete;
            };
            injectorPrompt.ConfirmationComplete += injectionPromptComplete;
        }
    }
}
