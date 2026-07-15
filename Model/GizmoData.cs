using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace BobMapper.Model
{
    internal partial class GizmoData : INotifyPropertyChanged
    {
		private Selections currentSelections;

		public Selections CurrentSelections
		{
			get { return currentSelections; }
			set { currentSelections = value; }
		}

		private bool propGizmoEnabled = false;
        public bool PropGizmoEnabled
		{
			get { return propGizmoEnabled; }
			set { propGizmoEnabled = value;
				OnPropertyChanged();
			}
		}

        private bool npcGizmoEnabled = false;
        public bool NPCGizmoEnabled
        {
            get { return npcGizmoEnabled; }
            set
            {
                npcGizmoEnabled = value;
                OnPropertyChanged();
            }
        }

		private bool pathPointGizmoEnabled = false;

		public bool PathPointGizmoEnabled
		{
			get { return pathPointGizmoEnabled; }
			set { pathPointGizmoEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool wallGizmosEnabled = false;

		public bool WallGizmosEnabled
		{
			get { return wallGizmosEnabled; }
			set { wallGizmosEnabled = value;
				OnPropertyChanged();
			}
		}

		private bool doorGizmosEnabled = false;

		public bool DoorGizmosEnabled
		{
			get { return doorGizmosEnabled; }
			set { doorGizmosEnabled = value; }
		}

		private bool lootGizmoEnabled;

		public bool LootGizmoEnabled
		{
			get { return lootGizmoEnabled; }
			set { lootGizmoEnabled = value; }
		}

		private bool miscGizmoEnabled;

		public bool MiscGizmoEnabled
		{
			get { return miscGizmoEnabled; }
			set { miscGizmoEnabled = value; }
		}
		public GizmoData(Selections selections)
		{
			CurrentSelections = selections;
			selections.SelectedToolChanged += SelectedToolChangedHandler;
		}

		private void SelectedToolChangedHandler(object sender, EventArgs e)
		{
			Selections selections = (Selections)sender;
			if(selections.SelectedTool != Tools.Move)
			{
				DisableAllGizmos();
				return;
			}
			switch(selections.SelectedObjectType)
			{
				case MapManager.ObjectType.Prop:
                    PropGizmoEnabled = true;
                    break;
				case MapManager.ObjectType.NPC:
					NPCGizmoEnabled = true;
					break;
				case MapManager.ObjectType.PathPoint:
					PathPointGizmoEnabled = true;
					break;
				case MapManager.ObjectType.Wall:
					WallGizmosEnabled = true;
					break;
				case MapManager.ObjectType.Loot:
					LootGizmoEnabled = true;
					break;
				case MapManager.ObjectType.Door:
					DoorGizmosEnabled = true;
					break;
				case MapManager.ObjectType.Misc:
					MiscGizmoEnabled = true;
					break;
				default: 
					break;
					
			}
		}

		private void DisableAllGizmos()
		{
			PropGizmoEnabled = false;
			NPCGizmoEnabled = false;
			PathPointGizmoEnabled = false;
			WallGizmosEnabled = false;
			LootGizmoEnabled = false;
			DoorGizmosEnabled = false;
			MiscGizmoEnabled = false;
		}

		//||||||||||||||||||||||||||||||||||||||||//
		//		THE WALL OF TERRIBLE CODE	      //
		//||||||||||||||||||||||||||||||||||||||||//


		[RelayCommand]
		public void PropGizmoMoved(SnapCoordinate newCoordinate)
		{
            CurrentSelections.SelectedProp.Coordinates.SnappedXPos += newCoordinate.SnappedXPos;
			CurrentSelections.SelectedProp.Coordinates.SnappedYPos += newCoordinate.SnappedYPos;
		}

        
		[RelayCommand]
        public void NPCGizmoMoved(SnapCoordinate newCoordinate)
        {
            CurrentSelections.SelectedNPC.Coordinates.SnappedXPos += newCoordinate.SnappedXPos;
            CurrentSelections.SelectedNPC.Coordinates.SnappedYPos += newCoordinate.SnappedYPos;
        }

        [RelayCommand]
        public void PathPointGizmoMoved(SnapCoordinate newCoordinate)
        {
            CurrentSelections.SelectedPathPoint.Coordinates.SnappedXPos += newCoordinate.SnappedXPos;
            CurrentSelections.SelectedPathPoint.Coordinates.SnappedYPos += newCoordinate.SnappedYPos;
        }

        [RelayCommand]
        public void WallGizmo1Moved(SnapCoordinate newCoordinate)
        {
            CurrentSelections.SelectedWall.Point1.SnappedXPos += newCoordinate.SnappedXPos;
            CurrentSelections.SelectedWall.Point1.SnappedYPos += newCoordinate.SnappedYPos;
        }

        [RelayCommand]
        public void WallGizmo2Moved(SnapCoordinate newCoordinate)
        {
            CurrentSelections.SelectedWall.Point2.SnappedXPos += newCoordinate.SnappedXPos;
            CurrentSelections.SelectedWall.Point2.SnappedYPos += newCoordinate.SnappedYPos;
        }

        [RelayCommand]
        public void DoorGizmo1Moved(SnapCoordinate newCoordinate)
        {
            CurrentSelections.SelectedDoor.Point1.SnappedXPos += newCoordinate.SnappedXPos;
            CurrentSelections.SelectedDoor.Point1.SnappedYPos += newCoordinate.SnappedYPos;
        }

        [RelayCommand]
        public void DoorGizmo2Moved(SnapCoordinate newCoordinate)
        {
            CurrentSelections.SelectedDoor.Point2.SnappedXPos += newCoordinate.SnappedXPos;
            CurrentSelections.SelectedDoor.Point2.SnappedYPos += newCoordinate.SnappedYPos;
        }

        [RelayCommand]
        public void LootGizmoMoved(SnapCoordinate newCoordinate)
        {
            CurrentSelections.SelectedLoot.Coordinates.SnappedXPos += newCoordinate.SnappedXPos;
            CurrentSelections.SelectedLoot.Coordinates.SnappedYPos += newCoordinate.SnappedYPos;
        }

        [RelayCommand]
        public void MiscGizmoMoved(SnapCoordinate newCoordinate)
        {
            CurrentSelections.SelectedMisc.Coordinates.SnappedXPos += newCoordinate.SnappedXPos;
            CurrentSelections.SelectedMisc.Coordinates.SnappedYPos += newCoordinate.SnappedYPos;
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
