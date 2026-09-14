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
    public class PlaceObjectPreviewData : INotifyPropertyChanged
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

        private bool isTwoPointVisible;

        public bool IsTwoPointVisible
        {
            get { return isTwoPointVisible; }
            set { isTwoPointVisible = value;
                OnPropertyChanged();
            }
        }

        private bool isSinglePointVisible;

        public bool IsSinglePointVisible
        {
            get { return isSinglePointVisible; }
            set { isSinglePointVisible = value; OnPropertyChanged(); }
        }


        private string previewTexture;

        public string PreviewTexture
        {
            get { return previewTexture; }
            set { previewTexture = value; OnPropertyChanged(); }
        }

        private SnapCoordinate singlePointCoordinate = new(0,0);

        public SnapCoordinate SinglePointCoordinate
        {
            get { return singlePointCoordinate; }
            set { singlePointCoordinate = value; }
        }


        public bool IsDragging { get; set; }

        public void HandleTwoPointMouseMove(SnapCoordinate mousePos)
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

        public void HandleSinglePointMouseMove(SnapCoordinate mousePos)
        {
            SinglePointCoordinate.SnappedXPos = mousePos.SnappedXPos;
            SinglePointCoordinate.SnappedYPos = mousePos.SnappedYPos;
        }

    }
}
