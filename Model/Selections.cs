using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using static BobMapper.Model.MapObjects;

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

        public void InitializeCurrentTextureSet(Tilesets tileset)
        {
            List<string> temporaryTextureSet = new List<string>();
            SqliteConnection textureManifestConnection = new("Data Source=Data/TextureManifest.sqlite");
            textureManifestConnection.Open();
            var selectTexturesCommand = textureManifestConnection.CreateCommand();
            selectTexturesCommand.CommandText = $"SELECT ResourceName FROM Textures WHERE Tilesets LIKE '%{(int)tileset}%'";
            var reader = selectTexturesCommand.ExecuteReader();
            while (reader.Read())
            {
                string textureName = reader.GetString(0);
                temporaryTextureSet.Add(textureName);
            }
            textureManifestConnection.Close();
            CurrentTextureSet = temporaryTextureSet.ToArray();
        }
    }
}
