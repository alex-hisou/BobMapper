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

namespace BobMapper.ViewModel
{
    internal partial class EditorViewModel : ViewModelBase
    {
        private ObjectType selectedObjectType;

        public ObjectType SelectedObjectType
        {
            get { return selectedObjectType; }
            set { selectedObjectType = value; }
        }


        private Tools selectedTool;

        public Tools SelectedTool
        {
            get { return selectedTool; }
            set { selectedTool = value; }
        }


        private Wall selectedWall;

        public Wall SelectedWall
        {
            get { return selectedWall; }
            set
            {
                selectedWall = value;
                OnPropertyChanged();
            }
        }
        private Prop selectedProp;

        public Prop SelectedProp
        {
            get { return selectedProp; }
            set
            {
                selectedProp = value;
                OnPropertyChanged();
            }
        }
        private NPC selectedNPC;

        public NPC SelectedNPC
        {
            get { return selectedNPC; }
            set
            {
                selectedNPC = value;
                OnPropertyChanged();
            }
        }

        private PathPoint selectedPathPoint;

        public PathPoint SelectedPathPoint
        {
            get { return selectedPathPoint; }
            set
            {
                selectedPathPoint = value;
                OnPropertyChanged();
            }
        }

        private Floor selectedFloor;

        public Floor SelectedFloor
        {
            get { return selectedFloor; }
            set
            {
                selectedFloor = value;
                OnPropertyChanged();
            }
        }

        private Misc selectedMisc;

        public Misc SelectedMisc
        {
            get { return selectedMisc; }
            set
            {
                selectedMisc = value;
                OnPropertyChanged();
            }
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
        public Floor[,] CurrentFloors { get { return currentFloors; } set { currentFloors = value; } }
        private Floor[,] currentFloors;

        private Map currentMap;

        public Map CurrentMap
        {
            get { return currentMap; }
            set { currentMap = value; }
        }
        

        public EditorViewModel()
        {
            Map saveMap = new Map(0);
            saveMap.props.Add(new Prop(new Coordinate(-5, -100), 45, "/Resources/PropTextures/box.png"));
            saveMap.props.Add(new Prop(new Coordinate(100, -10), 45, "/Resources/PropTextures/toilet.png"));
            saveMap.walls.Add(new Wall(new Coordinate(100, -10), new Coordinate(100, 0), Wall.WallType.Normal, "/Resources/WallTextures/PlainGreen.png", "/Resources/WallTextures/PlainGreen.png"));
            saveMap.walls.Add(new Wall(new Coordinate(90, 40), new Coordinate(80, 20), Wall.WallType.Normal, "/Resources/WallTextures/PlainGreen.png", "/Resources/WallTextures/PlainGreen.png"));
            CurrentMap = saveMap;
            CurrentProps = new ObservableCollection<Prop>(CurrentMap.props);
            CurrentWalls = new ObservableCollection<Wall>(CurrentMap.walls);
            JsonMapParse.SaveData(saveMap);
        }

        [RelayCommand]
        public void ClickObject(object sender)
        {
            
            switch(SelectedTool)
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
            switch (SelectedObjectType)
            {
                case ObjectType.Wall:
                    {
                        SelectedWall = null;
                        break;
                    }
                case ObjectType.Prop:
                    {
                        SelectedProp = null;
                        break;
                    }
                case ObjectType.NPC:
                    {
                        SelectedNPC = null;
                        break;
                    }
                case ObjectType.PathPoint:
                    {
                        SelectedPathPoint = null;
                        break;
                    }
                case ObjectType.Floor:
                    {
                        SelectedFloor = null;
                        break;
                    }
                case ObjectType.Misc:
                    {
                        SelectedMisc = null;
                        break;
                    }
            }
            SelectedObjectType = ObjectType.None;
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
                    SelectedWall = CurrentWalls[selectedObjectIndex];
                    SelectedObjectType = ObjectType.Wall;
                    break;
                case 1: //Prop
                    selectedObjectIndex = CurrentProps.IndexOf((Prop)sender);
                    SelectedProp = CurrentProps[selectedObjectIndex];
                    SelectedObjectType = ObjectType.Prop;
                    break;
                case 2: //NPC
                    selectedObjectIndex = CurrentNPCs.IndexOf((NPC)sender);
                    SelectedNPC = CurrentNPCs[selectedObjectIndex];
                    SelectedObjectType = ObjectType.NPC;
                    break;
                case 3: //PathPoint
                    selectedObjectIndex = CurrentPathPoints.IndexOf((PathPoint)sender);
                    SelectedPathPoint = CurrentPathPoints[selectedObjectIndex];
                    SelectedObjectType = ObjectType.PathPoint;
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
                    SelectedMisc = CurrentMiscs[selectedObjectIndex];
                    SelectedObjectType = ObjectType.Misc;
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

        internal enum Tools
        {
            None,
            Select,
            Move,
            Rotate,
            AddWall,
            AddProp,
            AddNPC,
            AddPathPoint,
            AddMisc
        }
    }
}
