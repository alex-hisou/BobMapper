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

		private bool propRotateGizmoEnabled;

		public bool PropRotateGizmoEnabled
		{
			get { return propRotateGizmoEnabled; }
			set { propRotateGizmoEnabled = value;
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

		private bool npcRotateGizmoEnabled;

		public bool NPCRotateGizmoEnabled
		{
			get { return npcRotateGizmoEnabled; }
			set { npcRotateGizmoEnabled = value;
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

		private bool pathPointRotateGizmoEnabled;

		public bool PathPointRotateGizmoEnabled
		{
			get { return pathPointRotateGizmoEnabled; }
			set { pathPointRotateGizmoEnabled = value;
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
			set { doorGizmosEnabled = value;
                OnPropertyChanged();
            }
		}

		private bool lootGizmoEnabled;

		public bool LootGizmoEnabled
		{
			get { return lootGizmoEnabled; }
			set { lootGizmoEnabled = value;
                OnPropertyChanged();
            }
		}

		private bool lootRotateGizmoEnabled;

		public bool LootRotateGizmoEnabled
		{
			get { return lootRotateGizmoEnabled; }
			set { lootRotateGizmoEnabled = value;
                OnPropertyChanged();
            }
		}


		private bool miscGizmoEnabled;

		public bool MiscGizmoEnabled
		{
			get { return miscGizmoEnabled; }
			set { miscGizmoEnabled = value;
                OnPropertyChanged();
            }
		}
		public GizmoData(Selections selections)
		{
			CurrentSelections = selections;
			selections.SelectedToolChanged += SelectedToolChangedHandler;
		}

		private void SelectedToolChangedHandler(object sender, EventArgs e)
		{
            DisableAllGizmos();
            Selections selections = (Selections)sender;
			if (selections.SelectedTool == Tools.Move)
			{
				switch (selections.SelectedObjectType)
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
			else if (selections.SelectedTool == Tools.Rotate)
			{
				switch (selections.SelectedObjectType)
				{
					default:
						break;
					case MapManager.ObjectType.Prop:
						PropRotateGizmoEnabled = true;
						break;
					case MapManager.ObjectType.NPC:
						NPCRotateGizmoEnabled = true;
						break;
					case MapManager.ObjectType.PathPoint:
						PathPointRotateGizmoEnabled = true;
						break;
					case MapManager.ObjectType.Loot:
						LootRotateGizmoEnabled = true;
						break;
				}
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
			PropRotateGizmoEnabled = false;
			NPCRotateGizmoEnabled = false;
			PathPointRotateGizmoEnabled = false;
			LootRotateGizmoEnabled = false;
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
            CurrentSelections.SelectedWall.Point1.SnappedXPos += (float)Math.Ceiling(newCoordinate.SnappedXPos * 2) / 2;
            CurrentSelections.SelectedWall.Point1.SnappedYPos += (float)Math.Ceiling(newCoordinate.SnappedYPos * 2) / 2;
        }

        [RelayCommand]
        public void WallGizmo2Moved(SnapCoordinate newCoordinate)
        {
            CurrentSelections.SelectedWall.Point2.SnappedXPos += (float)Math.Ceiling(newCoordinate.SnappedXPos * 2) / 2;
            CurrentSelections.SelectedWall.Point2.SnappedYPos += (float)Math.Ceiling(newCoordinate.SnappedYPos * 2) / 2;
        }

        [RelayCommand]
        public void DoorGizmo1Moved(SnapCoordinate newCoordinate)
        {
            CurrentSelections.SelectedDoor.Point1.SnappedXPos += (float)Math.Ceiling(newCoordinate.SnappedXPos * 2) / 2;
            CurrentSelections.SelectedDoor.Point1.SnappedYPos += (float)Math.Ceiling(newCoordinate.SnappedYPos * 2) / 2;
        }

        [RelayCommand]
        public void DoorGizmo2Moved(SnapCoordinate newCoordinate)
        {
            CurrentSelections.SelectedDoor.Point2.SnappedXPos += (float)Math.Ceiling(newCoordinate.SnappedXPos * 2) / 2;
            CurrentSelections.SelectedDoor.Point2.SnappedYPos += (float)Math.Ceiling(newCoordinate.SnappedYPos * 2) / 2;
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

		[RelayCommand]
		public void PropRotateGizmoMoved(float angle)
		{
			CurrentSelections.SelectedProp.Rotation -= angle;
		}

        [RelayCommand]
        public void NPCRotateGizmoMoved(float angle)
        {
            CurrentSelections.SelectedNPC.Rotation -= angle;
        }

        [RelayCommand]
        public void PathPointRotateGizmoMoved(float angle)
        {
            CurrentSelections.SelectedPathPoint.Rotation -= angle;
        }

        [RelayCommand]
        public void LootRotateGizmoMoved(float angle)
        {
            CurrentSelections.SelectedLoot.Rotation -= angle;
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
