using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using BobMapper.Model.MapObjects;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BobMapper.ViewModel
{
    public class CableViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SnapCoordinate> coordinates;

        public ObservableCollection<SnapCoordinate> Coordinates
        {
            get { return coordinates; }
            set { coordinates = value; }
        }

        private SolidColorBrush colourBrush;

        public SolidColorBrush ColourBrush
        {
            get { return colourBrush; }
            set { colourBrush = value; OnPropertyChanged(); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CableViewModel(Cable cable)
        {
            
            Coordinates = new ObservableCollection<SnapCoordinate>();
            BrushConverter converter = new BrushConverter();
            ColourBrush = (SolidColorBrush)converter.ConvertFromString(cable.ColourHex);
        }

        public static ObservableCollection<CableViewModel> CableViewModelFactory(List<Cable> cables)
        {
            ObservableCollection<CableViewModel> cableViewModels = new ObservableCollection<CableViewModel>();
            foreach (var cable in cables)
            {
                CableViewModel cableViewModel = new CableViewModel(cable);
                cableViewModels.Add(cableViewModel);
            }
            return cableViewModels;
        }

        public static List<Cable> ModelCablesFactory(ObservableCollection<CableViewModel> cableViewModels)
        {
            List<Cable> cables = new List<Cable>();
            foreach (var cableViewModel in cableViewModels)
            {
                List<SnapCoordinate> coordinates = cableViewModel.Coordinates.ToList();
                Color colour = cableViewModel.ColourBrush.Color;
                string colourHex = $"#{colour.R:X2}{colour.G:X2}{colour.B:X2}";
                Cable cable = new Cable(colourHex, coordinates);
            }
            return cables;
        }
    }
}
