using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model;

namespace BobMapper.ViewModel
{
    internal class MapPropertiesWindowViewModel : ViewModelBase
    {
        private MapProperties currentMapProperties;

        public MapProperties CurrentMapProperties
        {
            get { return currentMapProperties; }
            set { currentMapProperties = value; }
        }


        public MapPropertiesWindowViewModel(MapProperties mapProperties)
        {
            CurrentMapProperties = mapProperties;
        }

        public Array TilesetEnum => Enum.GetValues(typeof(Tilesets));
    }
}
