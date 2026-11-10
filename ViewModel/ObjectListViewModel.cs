using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model;

namespace BobMapper.ViewModel
{
    internal class ObjectListViewModel : ViewModelBase
    {
        public ObjectListViewModel()
        {

        }

        internal ObservableCollection<MapObject> mapObjects { get; set; }

        internal class MapObject
        {

            private MapManager.ObjectType objectType;

            public MapManager.ObjectType ObjectType
            {
                get { return objectType; }
                set { objectType = value; }
            }


            private SnapCoordinate coordinate1;

            public SnapCoordinate Coordinate1
            {
                get { return coordinate1; }
                set { coordinate1 = value; }
            }

            private string texture;

            public string Texture
            {
                get { return texture; }
                set { texture = value; }
            }



        }
    }
}
