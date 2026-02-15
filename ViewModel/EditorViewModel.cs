using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using BobMapper.Model;
using BobMapper.Services;
using CommunityToolkit.Mvvm.Input;
using static BobMapper.Model.MapObjects;

namespace BobMapper.ViewModel
{
    internal partial class EditorViewModel : ViewModelBase
    {

        public static class SelectedObject
        {
            internal static Type selectedObjectType;
            internal static int selectedObjectIndex;
            public static Coordinate coordinates;


        }

        public ObservableCollection<Prop> CurrentProps { get => currentProps; set => currentProps = value; }
        private Map currentMap;
        private ObservableCollection<Prop> currentProps;

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
            CurrentMap = saveMap;
            CurrentProps = new ObservableCollection<Prop>(CurrentMap.props);
            DataParse.SaveData(saveMap);
        }

        [RelayCommand]
        public void ClickObject(object sender)
        {
            SelectNew(sender);
        }

        public void SelectNew(object sender)
        {
            SelectedObject.selectedObjectType = sender.GetType();
            //Terrible way of selecting objects
            switch (TypeSchema[sender.GetType()])
            {
                case 0:
                    break;
                case 1:
                    SelectedObject.selectedObjectIndex = CurrentProps.IndexOf((Prop)sender);
                    break;
                default:
                    return;
            }
            int newX;
            int newY;
            SelectedObject.coordinates = new Coordinate();
        }

        public Dictionary<Type, int> TypeSchema = new Dictionary<Type, int>()
        {
            {typeof(Wall), 0},
            {typeof(Prop), 1},
            {typeof(NPC), 2},
            {typeof(PathPoint), 3},
            {typeof(Misc), 4}
        };


    }
}
