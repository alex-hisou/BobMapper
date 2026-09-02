using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using BobMapper.Model.MapObjects;
using System.ComponentModel;

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

        private Cable currentCable;

        public event PropertyChangedEventHandler PropertyChanged;

        public Cable CurrentCable
        {
            get { return currentCable; }
            set { currentCable = value; }
        }

        public CableViewModel(Cable cable)
        {
            CurrentCable = cable;
            Coordinates = new ObservableCollection<SnapCoordinate>();
            //TODO: Coordinates sync when saving
        }
    }
}
