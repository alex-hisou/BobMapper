using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BobMapper.Model
{
    internal class ViewportData : INotifyPropertyChanged
    {
        private int viewOffsetX;

        public int ViewOffsetX
        {
            get { return viewOffsetX; }
            set { viewOffsetX = value; }
        }

        private int viewOffsetY;

        public int ViewOffsetY
        {
            get { return viewOffsetY; }
            set { viewOffsetY = value; }
        }

        private int cameraX;

        public int CameraX
        {
            get { return cameraX; }
            set { cameraX = value;
                OnPropertyChanged();
            }
        }

        private int cameraY;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CameraY
        {
            get { return cameraY; }
            set { cameraY = value;
                OnPropertyChanged();
            }
        }

        private double zoomX;

        public double ZoomX
        {
            get { return zoomX; }
            set {
                if (value > 2.5)
                    zoomX = 2.5;
                else if (value < 0.5)
                    zoomX = 0.5;
                else
                    zoomX = value;
                OnPropertyChanged();
            }
        }

        private double zoomY;

        public double ZoomY
        {
            get { return zoomY; }
            set { if(value > -0.5)
                    zoomY = -0.5;
                else if (value < -2.5)
                    zoomY = -2.5;
                else
                zoomY = value;
                OnPropertyChanged();
            }
        }


    }
}
