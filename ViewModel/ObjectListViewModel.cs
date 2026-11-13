using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model;
using BobMapper.Model.MapObjects;
using CommunityToolkit.Mvvm.Input;

namespace BobMapper.ViewModel
{
    internal partial class ObjectListViewModel : ViewModelBase
    {
        public ObjectListViewModel(ObjectCollection objectCollection, Selections selections)
        {
            mapObjects = new();
            ObjectCollection = objectCollection;
            CurrentSelections = selections;
            for (int i = 0; i < objectCollection.CurrentWalls.Count; i++)
            {
                Wall wall = objectCollection.CurrentWalls[i];
                string procTexture = $"{wall.Texture1} || {wall.Texture2}";
                MapObject procWall = new MapObject(MapManager.ObjectType.Wall, wall.Point1, procTexture, i);
                mapObjects.Add(procWall);
            }
            for (int i = 0; i < objectCollection.CurrentDoors.Count; i++)
            {
                Door door = objectCollection.CurrentDoors[i];
                MapObject procDoor = new MapObject(MapManager.ObjectType.Door, door.Point1, door.Texture1, i);
                mapObjects.Add(procDoor);
            }
            for (int i = 0; i < objectCollection.CurrentProps.Count; i++)
            {
                Prop prop = objectCollection.CurrentProps[i];
                MapObject procProp = new MapObject(MapManager.ObjectType.Prop, prop.Coordinates, prop.PropTexture, i);
                mapObjects.Add(procProp);
            }
            for (int i = 0; i < objectCollection.CurrentNPCs.Count; i++)
            {
                NPC npc = objectCollection.CurrentNPCs[i];
                string npcType = npc.Type.ToString();
                MapObject procNPC = new MapObject(MapManager.ObjectType.NPC, npc.Coordinates, npcType, i);
                mapObjects.Add(procNPC);
            }
            for (int i = 0; i < objectCollection.CurrentPathPoints.Count; i++)
            {
                PathPoint pathPoint = objectCollection.CurrentPathPoints[i];
                string pathIDAsString = pathPoint.Id.ToString();
                MapObject procPath = new MapObject(MapManager.ObjectType.PathPoint, pathPoint.Coordinates, pathIDAsString, i);
                mapObjects.Add(procPath);
            }
            for (int i = 0; i < objectCollection.CurrentMiscs.Count; i++)
            {
                Misc misc = objectCollection.CurrentMiscs[i];
                string miscTypeAsString = misc.Type.ToString();
                MapObject procMisc = new MapObject(MapManager.ObjectType.Misc, misc.Coordinates, miscTypeAsString, i);
                mapObjects.Add(procMisc);
            }
            for (int i = 0; i < objectCollection.CurrentLoots.Count; i++)
            {
                Loot loot = objectCollection.CurrentLoots[i];
                MapObject procLoot = new(MapManager.ObjectType.Loot, loot.Coordinates, loot.Texture, i);
                mapObjects.Add(procLoot);
            }
            for (int i = 0; i < objectCollection.CurrentCables.Count; i++)
            {
                CableViewModel cable = objectCollection.CurrentCables[i];
                string color = cable.ColourBrush.ToString();
                MapObject procCable = new(MapManager.ObjectType.Cable, cable.Coordinates[0], color, i);
                mapObjects.Add(procCable);
            }
        }

        public ObservableCollection<MapObject> mapObjects { get; set; }

        public Selections CurrentSelections { get; set; }

        public ObjectCollection ObjectCollection { get; set; }

        public int SelectedId { get; set; }

        [RelayCommand]
        public void Select()
        {
            MapObject selectedObject = mapObjects[SelectedId];
            //Duplicate code from EditorViewModel. Trust me, this is better than trying to call functions from there somehow.
            switch (selectedObject.ObjectType)
            {
                case MapManager.ObjectType.Wall:
                    CurrentSelections.SelectedObjectType = MapManager.ObjectType.Wall;
                    Wall selectedWall = ObjectCollection.CurrentWalls[selectedObject.ID];
                    CurrentSelections.SelectedWall = selectedWall;
                    break;
                case MapManager.ObjectType.Prop:
                    CurrentSelections.SelectedObjectType = MapManager.ObjectType.Prop;
                    Prop selectedProp = ObjectCollection.CurrentProps[selectedObject.ID];
                    CurrentSelections.SelectedProp = selectedProp;
                    break;
                case MapManager.ObjectType.NPC:
                    CurrentSelections.SelectedObjectType = MapManager.ObjectType.NPC;
                    NPC selectedNPC = ObjectCollection.CurrentNPCs[selectedObject.ID];
                    CurrentSelections.SelectedNPC = selectedNPC;
                    break;
                case MapManager.ObjectType.PathPoint:
                    CurrentSelections.SelectedObjectType = MapManager.ObjectType.PathPoint;
                    PathPoint selectedPath = ObjectCollection.CurrentPathPoints[selectedObject.ID];
                    CurrentSelections.SelectedPathPoint = selectedPath;
                    break;
                case MapManager.ObjectType.Misc:
                    CurrentSelections.SelectedObjectType = MapManager.ObjectType.Misc;
                    Misc selectedMisc = ObjectCollection.CurrentMiscs[selectedObject.ID];
                    CurrentSelections.SelectedMisc = selectedMisc;
                    break;
                case MapManager.ObjectType.Door:
                    CurrentSelections.SelectedObjectType = MapManager.ObjectType.Door;
                    Door selectedDoor = ObjectCollection.CurrentDoors[selectedObject.ID];
                    CurrentSelections.SelectedDoor = selectedDoor;
                    break;
                case MapManager.ObjectType.Loot:
                    CurrentSelections.SelectedObjectType = MapManager.ObjectType.Loot;
                    Loot selectedLoot = ObjectCollection.CurrentLoots[selectedObject.ID];
                    CurrentSelections.SelectedLoot = selectedLoot;
                    break;
                case MapManager.ObjectType.Cable:
                    CurrentSelections.SelectedObjectType = MapManager.ObjectType.Cable;
                    CableViewModel selectedCable = ObjectCollection.CurrentCables[selectedObject.ID];
                    CurrentSelections.SelectedCable = selectedCable;
                    break;
            }
        }

        internal class MapObject
        {

            private MapManager.ObjectType objectType;

            public MapManager.ObjectType ObjectType
            {
                get { return objectType; }
                set { objectType = value; }
            }


            private SnapCoordinate coordinate1;

            public SnapCoordinate Coordinate1
            {
                get { return coordinate1; }
                set { coordinate1 = value; }
            }
            private string texture1;

            public string Texture1
            {
                get { return texture1; }
                set { texture1 = value; }
            }

            private int id;

            public int ID
            {
                get { return id; }
                set { id = value; }
            }


            public MapObject(MapManager.ObjectType objectType, SnapCoordinate coordinate1, string texture1, int id)
            {
                ObjectType = objectType;
                Coordinate1 = coordinate1;
                Texture1 = texture1;
                ID = id;
            }
        }
    }
}
