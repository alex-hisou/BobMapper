using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Data;
using BobMapper.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BobMapper.ViewModel
{
    internal partial class PrefrencesViewModel : ViewModelBase
    {
        private string steamPath;

        public string SteamPath
        {
            get { return steamPath; }
            set { steamPath = value;
                OnPropertyChanged();
            }
        }

        private bool autoSelect;

        public bool AutoSelect
        {
            get { return autoSelect; }
            set { autoSelect = value; }
        }


        private bool newChanges;

        public bool NewChanges
        {
            get { return newChanges; }
            set { newChanges = value;
                OnPropertyChanged();
            }
        }

        public PrefrencesViewModel()
        {
            SteamPath = UserSettings.Instance.SteamResourcesDirectory;
            AutoSelect = UserSettings.Instance.AutoSelect;
        }

        [RelayCommand]
        public void Apply()
        {
            UserSettings.Instance.AutoSelect = AutoSelect;
            UserSettings.Instance.SteamResourcesDirectory = SteamPath;
            UserSettings.Instance.Save();
            NewChanges = false;
        }

        [RelayCommand]
        public void SettingChanged()
        {
            NewChanges = true;
        }

        [RelayCommand]
        public void ChangeSteamDir()
        {
            FileDialogService fileDialogService = new FileDialogService();
            SteamPath = fileDialogService.LoadFileDialog("resources.dat|resources.dat");
        }
    }
}
