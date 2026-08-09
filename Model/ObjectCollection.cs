using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model.MapObjects;

namespace BobMapper.Model
{
    public class ObjectCollection : INotifyPropertyChanged
    {
        public ObservableCollection<Wall> CurrentWalls { get => currentWalls; set => currentWalls = value; }
        private ObservableCollection<Wall> currentWalls;
        public ObservableCollection<Prop> CurrentProps { get => currentProps; set => currentProps = value; }
        private ObservableCollection<Prop> currentProps;
        public ObservableCollection<NPC> CurrentNPCs { get => currentNPCs; set => currentNPCs = value; }
        private ObservableCollection<NPC> currentNPCs;
        public ObservableCollection<PathPoint> CurrentPathPoints { get => currentPathPoints; set => currentPathPoints = value; }
        private ObservableCollection<PathPoint> currentPathPoints;
        public ObservableCollection<Misc> CurrentMiscs { get => currentMiscs; set => currentMiscs = value; }
        private ObservableCollection<Misc> currentMiscs;

        public ObservableCollection<ObservableCollection<Floor>> CurrentFloors
        {
            get => currentFloors;
            set
            {
                currentFloors = value;
                OnPropertyChanged(nameof(CurrentFloors));
            }
        }
        private ObservableCollection<ObservableCollection<Floor>> currentFloors;
        public ObservableCollection<Door> CurrentDoors { get => currentDoors; set => currentDoors = value; }
        private ObservableCollection<Door> currentDoors;
        public ObservableCollection<Loot> CurrentLoots { get => currentLoots; set => currentLoots = value; }
        private ObservableCollection<Loot> currentLoots;
        public ObservableCollection<ExitZone> CurrentExitZones { get => currentExitZones; set => currentExitZones = value; }
        private ObservableCollection<ExitZone> currentExitZones;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
