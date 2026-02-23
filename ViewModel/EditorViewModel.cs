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
using static BobMapper.Model.MapObjects;
using static BobMapper.Model.Selections;

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


        public ObservableCollection<Wall> CurrentWalls { get => currentWalls; set => currentWalls = value; }
        private ObservableCollection<Wall> currentWalls;
        public ObservableCollection<Prop> CurrentProps { get => currentProps; set { currentProps = value; OnPropertyChanged(); } }
        private ObservableCollection<Prop> currentProps;
        public ObservableCollection<NPC> CurrentNPCs { get => currentNPCs; set => currentNPCs = value; }
        private ObservableCollection<NPC> currentNPCs;
        public ObservableCollection<PathPoint> CurrentPathPoints { get => currentPathPoints; set => currentPathPoints = value; }
        private ObservableCollection<PathPoint> currentPathPoints;
        public ObservableCollection<Misc> CurrentMiscs { get => currentMiscs; set => currentMiscs = value; }
        private ObservableCollection<Misc> currentMiscs;
        public ObservableCollection<Floor> CurrentFloors { get => currentFloors; set => currentFloors = value; }
        private ObservableCollection<Floor> currentFloors;

        


        private Map currentMap;

        public Map CurrentMap
        {
            get { return currentMap; }
            set { currentMap = value; }
        }

        private void InitaializeTextureSchema()
        {
            //TODO: Look at the tileset and put all the textures in there
            //For the texture selector, use linq
            
        }
        

        public EditorViewModel()
        {
            CurrentSelections = new Selections();
            Map saveMap = new Map(0);
            saveMap.props.Add(new Prop(new Coordinate(-5, -100), 45, "/Resources/PropTextures/boombox.png"));
            saveMap.props.Add(new Prop(new Coordinate(100, -10), 45, "/Resources/PropTextures/toilet.png"));
            saveMap.walls.Add(new Wall(new Coordinate(100, -10), new Coordinate(100, 0), Wall.WallType.Normal, "/Resources/WallTextures/Wall_Plain_Green.png", "/Resources/WallTextures/Wall_Plain_Blue.png"));
            saveMap.walls.Add(new Wall(new Coordinate(90, 40), new Coordinate(80, 20), Wall.WallType.Normal, "/Resources/WallTextures/Wall_Plain_Green.png", "/Resources/WallTextures/Wall_Plain_Green.png"));
            saveMap.npcs.Add(new NPC(new Coordinate(0, 0), NPC.NPCType.BaldCop, 0));
            saveMap.npcs.Add(new NPC(new Coordinate(0, 0), NPC.NPCType.RedDressLady, 0));
            PathPoint pathPoint1 = new(new Coordinate(-200, -200), 1, 2);
            PathPoint pathPoint2 = new(new Coordinate(-200, -100), 2, 3);
            PathPoint pathPoint3 = new(new Coordinate(-100, -100), 3, 1);
            saveMap.pathPoints.Add(pathPoint1);
            saveMap.pathPoints.Add(pathPoint2);
            saveMap.pathPoints.Add(pathPoint3);
            CurrentMap = saveMap;
            CurrentProps = new ObservableCollection<Prop>(CurrentMap.props);
            CurrentWalls = new ObservableCollection<Wall>(CurrentMap.walls);
            CurrentNPCs = new ObservableCollection<NPC>(CurrentMap.npcs);
            CurrentPathPoints = new ObservableCollection<PathPoint>(CurrentMap.pathPoints);
            CurrentMiscs = new ObservableCollection<Misc>(CurrentMap.miscs);
            JsonMapParse.SaveData(saveMap);
            CurrentSelections.CurrentTextureSet = ["/Resources/WallTextures/Wall_Plain_Blue.png", "/Resources/WallTextures/Wall_Plain_Green.png", "/Resources/PropTextures/toilet.png", "/Resources/PropTextures/boombox.png"];
            InitaializeTextureSchema();
        }

        [RelayCommand]
        public void ClickEmpty(object sender)
        {
            ResetSelection();
        }

        [RelayCommand]
        public void SetObjectTexture(object sender)
        {
            //SUUUUUUUUPER BAAAAAAAD!!!!!
            switch (CurrentSelections.SelectedObjectType)
            {
                case ObjectType.Prop:
                    CurrentSelections.SelectedProp.PropTexture = CurrentSelections.SelectedTexture;
                    break;
            }
        }

        [RelayCommand]
        public void ClickObject(object sender)
        {
            
            switch(CurrentSelections.SelectedTool)
            {
                case Tools.None:
                    break;
                case Tools.Select:
                    break;
                case Tools.Move: 
                    break;
                case Tools.Rotate: 
                    break;
                case Tools.AddWall: 
                    break;
                case Tools.AddProp:
                    break;
                case Tools.AddNPC:
                    break;
                case Tools.AddPathPoint:
                    break;
                case Tools.AddMisc:
                    break;
                //TODO: Move code here
            }
            
            SelectObject(sender);
            //TODO: Switch on the type of sender, find it in the crapper and set the selectedobject
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
            switch (TypeSchema[sender.GetType()])
            {
                case 0: //Wall
                    selectedObjectIndex = CurrentWalls.IndexOf((Wall)sender);
                    CurrentSelections.SelectedWall = CurrentWalls[selectedObjectIndex];
                    CurrentSelections.SelectedObjectType = ObjectType.Wall;
                    break;
                case 1: //Prop
                    selectedObjectIndex = CurrentProps.IndexOf((Prop)sender);
                    CurrentSelections.SelectedProp = CurrentProps[selectedObjectIndex];
                    CurrentSelections.SelectedObjectType = ObjectType.Prop;
                    break;
                case 2: //NPC
                    selectedObjectIndex = CurrentNPCs.IndexOf((NPC)sender);
                    CurrentSelections.SelectedNPC = CurrentNPCs[selectedObjectIndex];
                    CurrentSelections.SelectedObjectType = ObjectType.NPC;
                    break;
                case 3: //PathPoint
                    selectedObjectIndex = CurrentPathPoints.IndexOf((PathPoint)sender);
                    CurrentSelections.SelectedPathPoint = CurrentPathPoints[selectedObjectIndex];
                    CurrentSelections.SelectedObjectType = ObjectType.PathPoint;
                    break;
                case 4: //Floor TODO: Implement
                    /*
                    selectedObjectIndex = CurrentFloors.IndexOf((Floor)sender);
                    SelectedFloor = CurrentFloors[selectedObjectIndex];
                    selectedObjectType = ObjectType.Floor;
                    */
                    break;
                case 5: //Misc
                    selectedObjectIndex = CurrentMiscs.IndexOf((Misc)sender);
                    CurrentSelections.SelectedMisc = CurrentMiscs[selectedObjectIndex];
                    CurrentSelections.SelectedObjectType = ObjectType.Misc;
                    break;
                default:
                    throw new Exception("Invalid object type");
            }
        }

        private Dictionary<Type, int> TypeSchema = new Dictionary<Type, int>()
        {
            {typeof(Wall), 0},
            {typeof(Prop), 1},
            {typeof(NPC), 2},
            {typeof(PathPoint), 3},
            {typeof(Floor), 4},
            {typeof(Misc), 5}
        };

        public int PathConnectX(object sender)
        {
            int x = 0;
            return x;
        }
        public int PathConnectY(object sender)
        {
            int y = 0;
            return y;
        }
    }
}
