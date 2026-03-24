using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using BobMapper.Model.MapObjects;
using static BobMapper.Model.MapManager;

namespace BobMapper.Model
{
    internal class Selections : INotifyPropertyChanged
    {
        private ObjectType selectedObjectType;

        public ObjectType SelectedObjectType
        {
            get { return selectedObjectType; }
            set { selectedObjectType = value; }
        }
        private string selectedTexture;
        public string SelectedTexture
        {
            get { return selectedTexture; }
            set { selectedTexture = value; OnPropertyChanged(); }
        }

        private TextureType selectedTextureType;

        public TextureType SelectedTextureType
        {
            get { return selectedTextureType; }
            set { selectedTextureType = value; OnPropertyChanged(); GetFilteredTextureSet(value, currentTileSet); }
        }


        private string[] currentTextureSet;
        public string[] CurrentTextureSet
        {
            get { return currentTextureSet; }
            set { currentTextureSet = value; OnPropertyChanged(); }
        }


        private Tools selectedTool;
        public Tools SelectedTool
        {
            get { return selectedTool; }
            set { selectedTool = value; }
        }


        private Wall selectedWall;
        public Wall SelectedWall
        {
            get { return selectedWall; }
            set
            {
                selectedWall = value;
                OnPropertyChanged();
            }
        }
        private Prop selectedProp;
        public Prop SelectedProp
        {
            get { return selectedProp; }
            set
            {
                selectedProp = value;
                OnPropertyChanged();
            }
        }
        private NPC selectedNPC;
        public NPC SelectedNPC
        {
            get { return selectedNPC; }
            set
            {
                selectedNPC = value;
                OnPropertyChanged();
            }
        }
        private PathPoint selectedPathPoint;
        public PathPoint SelectedPathPoint
        {
            get { return selectedPathPoint; }
            set
            {
                selectedPathPoint = value;
                OnPropertyChanged();
            }
        }
        private Floor selectedFloor;
        public Floor SelectedFloor
        {
            get { return selectedFloor; }
            set
            {
                selectedFloor = value;
                OnPropertyChanged();
            }
        }

        private Door selectedDoor;

        public Door SelectedDoor
        {
            get { return selectedDoor; }
            set { selectedDoor = value;
                OnPropertyChanged();
            }
        }

        private Misc selectedMisc;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Misc SelectedMisc
        {
            get { return selectedMisc; }
            set
            {
                selectedMisc = value;
                OnPropertyChanged();
            }
        }

        private Loot selectedLoot;

        public Loot SelectedLoot
        {
            get { return selectedLoot; }
            set { selectedLoot = value; OnPropertyChanged(); }
        }


        private Tilesets currentTileSet;

        public void GetFilteredTextureSet(TextureType textureType, Tilesets tileset)
        {
            List<string> temporaryTextureSet = new List<string>();
            SqliteConnection textureManifestConnection = new("Data Source=Data/TextureManifest.sqlite");
            textureManifestConnection.Open();
            var selectTexturesCommand = textureManifestConnection.CreateCommand();
            if (textureType.Equals(TextureType.All))
            {
                selectTexturesCommand.CommandText = $"SELECT ResourceName FROM Textures WHERE Tilesets LIKE '%{(int)tileset}%'";
            }
            else
            {
                selectTexturesCommand.CommandText = $"SELECT ResourceName FROM Textures WHERE Tilesets LIKE '%{(int)tileset}%' AND Type LIKE '%{(int)textureType}%'";
            }
            var reader = selectTexturesCommand.ExecuteReader();
            while (reader.Read())
            {
                string ResourceName = reader.GetString(0);
                temporaryTextureSet.Add(ResourceName);
            }
            textureManifestConnection.Close();
            CurrentTextureSet = temporaryTextureSet.ToArray();
        }
    }
}
