using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BobMapper.Model
{
    internal class LayerData : INotifyPropertyChanged
    {
		private bool wallsVisible = true;

		public bool WallsVisible
		{
			get { return wallsVisible; }
			set { wallsVisible = value;
                OnPropertyChanged();
            }
		}

        private bool propsVisible = true;

        public bool PropsVisible
        {
            get { return propsVisible; }
            set
            {
                propsVisible = value;
                OnPropertyChanged();
            }
        }

        private bool npcsVisible = true;

        public bool NPCsVisible
        {
            get { return npcsVisible; }
            set { npcsVisible = value;
                OnPropertyChanged();
            }
        }

        private bool pathPointsVisible = true;

        public bool PathPointsVisible
        {
            get { return pathPointsVisible; }
            set { pathPointsVisible = value;
                OnPropertyChanged();
            }
        }

        private bool lootsVisible = true;

        public bool LootsVisible
        {
            get { return lootsVisible; }
            set { lootsVisible = value;
                OnPropertyChanged();
            }
        }

        private bool doorsVisible = true;

        public bool DoorsVisible
        {
            get { return doorsVisible; }
            set { doorsVisible = value;
                OnPropertyChanged();
            }
        }

        private bool miscsVisible = true;

        public bool MiscsVisible
        {
            get { return miscsVisible; }
            set { miscsVisible = value;
                OnPropertyChanged();
            }
        }

        private bool floorsVisible = true;

        public bool FloorsVisible
        {
            get { return floorsVisible; }
            set { floorsVisible = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
