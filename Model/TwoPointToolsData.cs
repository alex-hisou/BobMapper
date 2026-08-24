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
    public partial class TwoPointToolsData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public SnapCoordinate StartCoordinate { get; set; }

        public SnapCoordinate EndCoordinate { get; set; }

        private bool isVisible;

        public bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsDragging { get; set; }

        public TwoPointToolsData()
        {
            StartCoordinate = new(0, 0);
            EndCoordinate = new(0, 0);
        }

        public void HandleMouseMove(SnapCoordinate mousePos)
        {
            if (!IsDragging)
            {
                StartCoordinate.SnappedXPos = mousePos.SnappedXPos;
                StartCoordinate.SnappedYPos = mousePos.SnappedYPos;
            }
            EndCoordinate.SnappedXPos = mousePos.SnappedXPos;
            EndCoordinate.SnappedYPos = mousePos.SnappedYPos;
        }

    }
}
