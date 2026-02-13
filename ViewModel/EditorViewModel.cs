using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BobMapper.Model;
using BobMapper.Services;
using CommunityToolkit.Mvvm.Input;
using static BobMapper.Model.MapObjects;

namespace BobMapper.ViewModel
{
    internal partial class EditorViewModel : ViewModelBase
    {
        internal Type selectedObjectType;
        internal int selectedObjectIndex;

        public ObservableCollection<Prop> Props { get => props; set => props = value; }

        public Dictionary<Type, int> TypeSchema = new Dictionary<Type, int>()
        {
            {typeof(Wall), 0},
            {typeof(Prop), 1},
            {typeof(NPC), 2},
            {typeof(PathPoint), 3},
            {typeof(Misc), 4}
        }; 

        private Map currentMap;
        private ObservableCollection<Prop> props;

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
            saveMap.props.Add(new Prop(new Coordinate(-5, -30), 45, "/resources/Level_Strip/box.png"));
            saveMap.props.Add(new Prop(new Coordinate(40, -10), 45, "/resources/Level_Strip/toilet.png"));
            CurrentMap = saveMap;
            Props = new ObservableCollection<Prop>(saveMap.props);
            DataParse.SaveData(saveMap);
        }

        [RelayCommand]
        public void ClickObject(object sender)
        {
            selectedObjectType = sender.GetType();
            //Terrible way of selecting objects
            switch (TypeSchema[sender.GetType()])
            {
                case 0:
                    break;
                case 1:
                    selectedObjectIndex = Props.IndexOf((Prop)sender);
                    break;
            }
        }


    }
}
