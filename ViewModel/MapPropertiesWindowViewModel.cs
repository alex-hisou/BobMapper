using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model;
using CommunityToolkit.Mvvm.Input;

namespace BobMapper.ViewModel
{
    internal partial class MapPropertiesWindowViewModel : ViewModelBase
    {
        private MapProperties currentMapProperties;

        public MapProperties CurrentMapProperties
        {
            get { return currentMapProperties; }
            set { currentMapProperties = value; }
        }

        private Selections currentSelections;

        public Selections CurrentSelections
        {
            get { return currentSelections; }
            set { currentSelections = value; }
        }


        public Array Backgrounds => MapManager.BackGroundManifest.Keys.ToArray();

        public MapPropertiesWindowViewModel(MapProperties mapProperties, Selections selections)
        {
            CurrentMapProperties = mapProperties;
            CurrentSelections = selections;
        }

        public Array TilesetEnum => Enum.GetValues(typeof(Tilesets));

        [RelayCommand]
        public void ChangeTileset()
        {
            CurrentMapProperties.ChangeTileset();
            //TODO: Make map change all of the invalid textures
        }
    }
}
