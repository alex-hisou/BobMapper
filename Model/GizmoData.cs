using System;
using System.Collections.Generic;
using System.ComponentModel;
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

		public GizmoData(Selections selections)
		{
			CurrentSelections = selections;
			selections.SelectedPropChanged += SelectedPropChangedHandler;
		}

		private void SelectedPropChangedHandler(object sender, EventArgs e)
		{
			Selections selections = (Selections)sender;
			if(selections.SelectedProp is null)
			{ 
				PropGizmoEnabled = false;
				return; 
			}
			PropGizmoEnabled = true;
		}


		[RelayCommand]
		public void PropGizmoMoved(SnapCoordinate newCoordinate)
		{
			CurrentSelections.SelectedProp.Coordinates.SnappedXPos = newCoordinate.SnappedXPos;
			CurrentSelections.SelectedProp.Coordinates.SnappedYPos = newCoordinate.SnappedYPos;
		}


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
