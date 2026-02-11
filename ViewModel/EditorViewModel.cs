using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model;
using BobMapper.Services;
using static BobMapper.Model.MapObjects;

namespace BobMapper.ViewModel
{
    internal class EditorViewModel : ViewModelBase
    {
        internal MapObjects.Type selectedObjectType;
        internal int selectedObjectIndex;

        public ObservableCollection<Prop> Props { get => props; set => props = value; }

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
            Map saveMap = new Map();
            saveMap.props.Add(new Prop(new Coordinate(10, -20), 45, "box.png"));
            saveMap.props.Add(new Prop(new Coordinate(10, -20), 45, "toilet.png"));
            CurrentMap = saveMap;
            Props = new ObservableCollection<Prop>(saveMap.props);
            DataParse.SaveData(saveMap);
        }


    }
}
