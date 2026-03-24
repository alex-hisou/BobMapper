using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BobMapper.Model;
using BobMapper.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BobMapper.Model.MapObjects;
using static BobMapper.Model.MapManager;
using System.IO;

namespace BobMapper.ViewModel
{
    internal partial class EditorViewModel : ViewModelBase
    {
        private Selections currentSelections;

        public Selections CurrentSelections
        {
            get { return currentSelections; }
            set { currentSelections = value; }
        }

        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }


        public ObservableCollection<Wall> CurrentWalls { get => currentWalls; set => currentWalls = value; }
        private ObservableCollection<Wall> currentWalls;
        public ObservableCollection<Prop> CurrentProps { get => currentProps; set => currentProps = value; }
        private ObservableCollection<Prop> currentProps;
        public ObservableCollection<NPC> CurrentNPCs { get => currentNPCs; set => currentNPCs = value; }
        private ObservableCollection<NPC> currentNPCs;
        public ObservableCollection<PathPoint> CurrentPathPoints { get => currentPathPoints; set => currentPathPoints = value; }
        private ObservableCollection<PathPoint> currentPathPoints;
        public ObservableCollection<Misc> CurrentMiscs { get => currentMiscs; set => currentMiscs = value; }
        private ObservableCollection<Misc> currentMiscs;
        public ObservableCollection<ObservableCollection<Floor>> CurrentFloors { get => currentFloors; set => currentFloors = value; }
        private ObservableCollection<ObservableCollection<Floor>> currentFloors;
        public ObservableCollection<Door> CurrentDoors { get => currentDoors; set => currentDoors = value; }
        private ObservableCollection<Door> currentDoors;
        public ObservableCollection<Loot> CurrentLoots { get => currentLoots; set => currentLoots = value; }
        private ObservableCollection<Loot> currentLoots;


        private Map currentMap;

        public Map CurrentMap
        {
            get { return currentMap; }
            set { currentMap = value; }
        }
        

        public EditorViewModel(string filename)
        {
            FileName = filename;
            CurrentMap = JsonMapParse.LoadData(filename);
            CurrentSelections = new Selections();
            CurrentProps = new ObservableCollection<Prop>(CurrentMap.props);
            CurrentWalls = new ObservableCollection<Wall>(CurrentMap.walls);
            CurrentNPCs = new ObservableCollection<NPC>(CurrentMap.npcs);
            CurrentPathPoints = new ObservableCollection<PathPoint>(CurrentMap.pathPoints);
            CurrentMiscs = new ObservableCollection<Misc>(CurrentMap.miscs);
            CurrentFloors = new ObservableCollection<ObservableCollection<Floor>>(FlattenFloors(CurrentMap.floors));
            CurrentDoors = new ObservableCollection<Door>(CurrentMap.doors);
            CurrentLoots = new ObservableCollection<Loot>(CurrentMap.loots);
            CurrentSelections.GetFilteredTextureSet(TextureType.All, CurrentMap.tileset);
            CurrentSelections.SelectedTextureType = TextureType.All;
        }

        //[RelayCommand]
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
            switch (CurrentSelections.SelectedTool)
            {
                case Tools.AddWall:
                    SnapCoordinate snappedPlacementPos = SnapCoordinate.UnsnappedCoordinateFactory(placementPos.XPos, placementPos.YPos);
                    SnapCoordinate shiftedSnappedPlacementPos = new SnapCoordinate(snappedPlacementPos.SnappedXPos + 1, snappedPlacementPos.SnappedYPos);
                    Wall wall = new Wall(snappedPlacementPos, shiftedSnappedPlacementPos, Wall.WallType.Normal, CurrentSelections.SelectedTexture, CurrentSelections.SelectedTexture);
                    CurrentWalls.Add(wall);
                    break;
                case Tools.AddProp:
                    Prop prop = new Prop(placementPos, 0, CurrentSelections.SelectedTexture);
                    CurrentProps.Add(prop);
                    break;
                case Tools.AddNPC:
                    NPC npc = new NPC(placementPos, NPC.NPCType.BulkyCop, 0, false, false);
                    CurrentNPCs.Add(npc);
                    break;
                case Tools.AddPathPoint:
                    PathPoint pathPoint = new PathPoint(placementPos, 0, 0);
                    CurrentPathPoints.Add(pathPoint);
                    break;
                case Tools.AddMisc:
                    Misc misc = new Misc(placementPos, Misc.MiscObjects.Key, 0);
                    CurrentMiscs.Add(misc);
                    break;
                case Tools.AddDoor:
                    SnapCoordinate snappedDoorPlacementPos = SnapCoordinate.UnsnappedCoordinateFactory(placementPos.XPos, placementPos.YPos);
                    SnapCoordinate shiftedSnappedDoorPlacementPos = new SnapCoordinate(snappedDoorPlacementPos.SnappedXPos + 1, snappedDoorPlacementPos.SnappedYPos);
                    Door door = new Door(snappedDoorPlacementPos, shiftedSnappedDoorPlacementPos, CurrentSelections.SelectedTexture, false, false);
                    CurrentDoors.Add(door);
                    break;
                case Tools.AddLoot:
                    Loot loot = new Loot(CurrentSelections.SelectedTexture, placementPos, 0);
                    CurrentLoots.Add(loot);
                    break;
                default:
                    break;
            }
        }

        [RelayCommand]
        public void SetObjectTexture(object sender)
        {
            //SUUUUUUUUPER BAAAAAAAD!!!!!
            string parsedSender = (string)sender;
            switch (parsedSender)
            {
                case "PropTexture":
                    CurrentSelections.SelectedProp.PropTexture = CurrentSelections.SelectedTexture;
                    break;
                case "LootTexture":
                    CurrentSelections.SelectedLoot.Texture = CurrentSelections.SelectedTexture;
                    break;
                case "WallTexture1":
                    CurrentSelections.SelectedWall.Texture1 = CurrentSelections.SelectedTexture;
                    break;
                case "WallTexture2":
                    CurrentSelections.SelectedWall.Texture2 = CurrentSelections.SelectedTexture;
                    break;
                case "DoorTexture":
                    CurrentSelections.SelectedDoor.Texture1 = CurrentSelections.SelectedTexture; 
                    break;

            }
        }

        [RelayCommand]
        public void ClickObject(object sender)
        {
            if(CurrentSelections.SelectedTool == Tools.Select)
            {
                SelectObject(sender);
            }
            if(CurrentSelections.SelectedTool == Tools.ChangeFloor && sender is Floor)
            {
                Floor floor = (Floor)sender;
                floor.Texture1 = CurrentSelections.SelectedTexture;
            }
        }

        private void ResetSelection()
        {
            switch (CurrentSelections.SelectedObjectType)
            {
                case ObjectType.Wall:
                    {
                        CurrentSelections.SelectedWall = null;
                        break;
                    }
                case ObjectType.Prop:
                    {
                        CurrentSelections.SelectedProp = null;
                        break;
                    }
                case ObjectType.NPC:
                    {
                        CurrentSelections.SelectedNPC = null;
                        break;
                    }
                case ObjectType.PathPoint:
                    {
                        CurrentSelections.SelectedPathPoint = null;
                        break;
                    }
                case ObjectType.Floor:
                    {
                        CurrentSelections.SelectedFloor = null;
                        break;
                    }
                case ObjectType.Misc:
                    {
                        CurrentSelections.SelectedMisc = null;
                        break;
                    }
                case ObjectType.Door:
                    {
                        CurrentSelections.SelectedDoor = null;
                        break;
                    }
                case ObjectType.Loot:
                    {
                        CurrentSelections.SelectedLoot = null;
                        break;
                    }
            }
            CurrentSelections.SelectedObjectType = ObjectType.None;
        }

        [RelayCommand]
        public void SetTexture(object sender)
        {
            CurrentSelections.SelectedTexture = (string)sender;
        }

        private void SelectObject(object sender)
        {
            //Not the best code, but this will do
            ResetSelection();
            int selectedObjectIndex;
            CurrentSelections.SelectedObjectType = TypeSchema[sender.GetType()];
            switch (TypeSchema[sender.GetType()])
            {
                case ObjectType.Wall: 
                    selectedObjectIndex = CurrentWalls.IndexOf((Wall)sender);
                    CurrentSelections.SelectedWall = CurrentWalls[selectedObjectIndex];
                    break;
                case ObjectType.Prop: //Prop
                    selectedObjectIndex = CurrentProps.IndexOf((Prop)sender);
                    CurrentSelections.SelectedProp = CurrentProps[selectedObjectIndex];
                    break;
                case ObjectType.NPC: //NPC
                    selectedObjectIndex = CurrentNPCs.IndexOf((NPC)sender);
                    CurrentSelections.SelectedNPC = CurrentNPCs[selectedObjectIndex];
                    break;
                case ObjectType.PathPoint: //PathPoint
                    selectedObjectIndex = CurrentPathPoints.IndexOf((PathPoint)sender);
                    CurrentSelections.SelectedPathPoint = CurrentPathPoints[selectedObjectIndex];
                    break;
                case ObjectType.Floor: //Floor TODO: Implement
                    /*
                    selectedObjectIndex = CurrentFloors.IndexOf((Floor)sender);
                    SelectedFloor = CurrentFloors[selectedObjectIndex];
                    selectedObjectType = ObjectType.Floor;
                    */
                    break;
                case ObjectType.Misc: //Misc
                    selectedObjectIndex = CurrentMiscs.IndexOf((Misc)sender);
                    CurrentSelections.SelectedMisc = CurrentMiscs[selectedObjectIndex];
                    break;
                case ObjectType.Door: 
                    selectedObjectIndex = CurrentDoors.IndexOf((Door)sender);
                    CurrentSelections.SelectedDoor = CurrentDoors[selectedObjectIndex];
                    break;
                case ObjectType.Loot:
                    selectedObjectIndex = CurrentLoots.IndexOf((Loot)sender);
                    CurrentSelections.SelectedLoot = CurrentLoots[selectedObjectIndex];
                    break;
                default:
                    throw new Exception("Invalid object type");
            }
        }

        [RelayCommand]
        public void DeleteObject()
        {
            int toDeleteId;
            switch (CurrentSelections.SelectedObjectType)
            {
                case ObjectType.Wall:
                    {
                        toDeleteId = CurrentWalls.IndexOf(CurrentSelections.SelectedWall);
                        CurrentSelections.SelectedWall = null;
                        CurrentWalls.RemoveAt(toDeleteId);
                        break;
                    }
                case ObjectType.Prop:
                    {
                        toDeleteId = CurrentProps.IndexOf(CurrentSelections.SelectedProp);
                        CurrentSelections.SelectedProp = null;
                        CurrentProps.RemoveAt(toDeleteId);
                        break;
                    }
                case ObjectType.NPC:
                    {
                        toDeleteId = CurrentNPCs.IndexOf(CurrentSelections.SelectedNPC);
                        CurrentSelections.SelectedNPC = null;
                        CurrentNPCs.RemoveAt(toDeleteId);
                        break;
                    }
                case ObjectType.PathPoint:
                    {
                        toDeleteId = CurrentPathPoints.IndexOf(CurrentSelections.SelectedPathPoint);
                        CurrentSelections.SelectedPathPoint = null;
                        CurrentPathPoints.RemoveAt(toDeleteId);
                        break;
                    }
                case ObjectType.Misc:
                    {
                        toDeleteId = CurrentMiscs.IndexOf(CurrentSelections.SelectedMisc);
                        CurrentSelections.SelectedMisc = null;
                        CurrentMiscs.RemoveAt(toDeleteId);
                        break;
                    }
                case ObjectType.Door:
                    {
                        toDeleteId = CurrentDoors.IndexOf(CurrentSelections.SelectedDoor);
                        CurrentSelections.SelectedDoor = null;
                        CurrentDoors.RemoveAt(toDeleteId);
                        break;
                    }
                case ObjectType.Loot:
                    {
                        toDeleteId = CurrentLoots.IndexOf(CurrentSelections.SelectedLoot);
                        CurrentSelections.SelectedLoot = null;
                        CurrentLoots.RemoveAt(toDeleteId); 
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
            CurrentSelections.SelectedObjectType = ObjectType.None;
        }

        /* internal object ReturnSelectedObject()
        {

        }
        */

        private Dictionary<Type, ObjectType> TypeSchema = new Dictionary<Type, ObjectType>()
        {
            {typeof(Wall), ObjectType.Wall},
            {typeof(Prop), ObjectType.Prop},
            {typeof(NPC), ObjectType.NPC},
            {typeof(PathPoint), ObjectType.PathPoint},
            {typeof(Floor), ObjectType.Floor},
            {typeof(Misc), ObjectType.Misc},
            {typeof(Door), ObjectType.Door },
            {typeof(Loot), ObjectType.Loot }
        };

        [RelayCommand]
        internal void Save(bool saveNewFile)
        {
            CurrentMap.walls = CurrentWalls.ToList();
            CurrentMap.doors = CurrentDoors.ToList();
            CurrentMap.props = CurrentProps.ToList();
            CurrentMap.pathPoints = CurrentPathPoints.ToList();
            CurrentMap.npcs = CurrentNPCs.ToList();
            CurrentMap.miscs = CurrentMiscs.ToList();
            CurrentMap.loots = CurrentLoots.ToList();
            if(saveNewFile)
            {

            }
            else
            {
                JsonMapParse.SaveData(CurrentMap, FileName);
            }
        }
    }
}
