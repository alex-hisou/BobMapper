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
        private Type selectedObjectType;


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
        private Map currentMap;

        public Map CurrentMap
        {
            get { return currentMap; }
            set { currentMap = value; }
        }
        internal enum Tools
        {
            None,
            Select,
            Move,
            Rotate,
            AddWall,
            AddProp,
            AddNPC
        }

        public EditorViewModel()
        {
            Map saveMap = new Map(0);
            saveMap.props.Add(new Prop(new Coordinate(-5, -100), 45, "/resources/Level_Strip/box.png"));
            saveMap.props.Add(new Prop(new Coordinate(100, -10), 45, "/resources/Level_Strip/toilet.png"));
            saveMap.walls.Add(new Wall(new Coordinate(100, -10), new Coordinate(100, 0), Wall.WallType.Normal, "/resources/Level_Strip/PlainGreen.png", "/resources/Level_Strip/PlainGreen.png"));
            saveMap.walls.Add(new Wall(new Coordinate(90, 40), new Coordinate(80, 20), Wall.WallType.Normal, "/resources/Level_Strip/PlainGreen.png", "/resources/Level_Strip/PlainGreen.png"));
            CurrentMap = saveMap;
            CurrentProps = new ObservableCollection<Prop>(CurrentMap.props);
            CurrentWalls = new ObservableCollection<Wall>(CurrentMap.walls);
            JsonMapParse.SaveData(saveMap);
        }

        [RelayCommand]
        public void ClickObject(object sender)
        {
            //TODO: Switch on the type of sender, find it in the crapper and set the selectedobject
        }


    }
}
