using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model.MapObjects;
using CommunityToolkit.Mvvm.Input;

namespace BobMapper.Model
{
    public class TwoPointToolsData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public SnapCoordinate StartCoordinate { get; set; } = new(0,0);

        public SnapCoordinate EndCoordinate { get; set; } = new(0,0);

        public float ConnectionDeltaX => EndCoordinate.XPos - StartCoordinate.XPos;
        public float ConnectionDeltaY => EndCoordinate.YPos - StartCoordinate.YPos;

        private bool isVisible;

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsDragging { get; set; }

        public void HandleMouseMove(SnapCoordinate mousePos)
        {
            if (!IsDragging)
            {
                StartCoordinate.SnappedXPos = mousePos.SnappedXPos;
                StartCoordinate.SnappedYPos = mousePos.SnappedYPos;
            }
            EndCoordinate.SnappedXPos = mousePos.SnappedXPos;
            EndCoordinate.SnappedYPos = mousePos.SnappedYPos;
            OnPropertyChanged(nameof(ConnectionDeltaX));
            OnPropertyChanged(nameof(ConnectionDeltaY));
        }

    }
}
