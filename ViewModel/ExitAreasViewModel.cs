using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model.MapObjects;
using CommunityToolkit.Mvvm.Input;

namespace BobMapper.ViewModel
{
    internal partial class ExitAreasViewModel : ViewModelBase
    {
        private ObservableCollection<ExitZone> exitZones;

        public ObservableCollection<ExitZone> ExitZones
        {
            get { return exitZones; }
            set { exitZones = value; }
        }

        private int selectedId;

        public int SelectedId
        {
            get { return selectedId; }
            set { selectedId = value; }
        }

        public ExitAreasViewModel(ObservableCollection<ExitZone> exitZones)
        {
            ExitZones = exitZones;
        }

        [RelayCommand]
        public void Edit()
        {
            foreach (var exitZone in exitZones)
            {
                exitZone.Selected = false;
            }
            ExitZones[SelectedId].Selected = true;
        }

        [RelayCommand]
        public void Delete()
        {
            ExitZones.RemoveAt(SelectedId);
        }

        [RelayCommand]
        public void AddExitZone()
        {
            SnapCoordinate point1 = new(0, 0);
            SnapCoordinate point2 = new(1, 0);
            SnapCoordinate point3 = new(1, 1);
            SnapCoordinate point4 = new(0, 1);
            ExitZone exitZone = new(point1, point2, point3, point4);
            ExitZones.Add(exitZone);
        }
    }
}
