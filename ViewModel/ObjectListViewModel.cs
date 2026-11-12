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
            foreach (Wall wall in objectCollection.CurrentWalls)
            {
                string procTexture = $"{wall.Texture1} || {wall.Texture2}";
                MapObject procWall = new MapObject(MapManager.ObjectType.Wall, wall.Point1, procTexture);
                mapObjects.Add(procWall);
            }
            foreach (Door door in objectCollection.CurrentDoors)
            {
                MapObject procDoor = new MapObject(MapManager.ObjectType.Door, door.Point1, door.Texture1);
                mapObjects.Add(procDoor);
            }
            foreach (Prop prop in objectCollection.CurrentProps)
            {
                MapObject procProp = new MapObject(MapManager.ObjectType.Prop, prop.Coordinates, prop.PropTexture);
                mapObjects.Add(procProp);
            }
            foreach (NPC npc in objectCollection.CurrentNPCs)
            {
                string npcType = npc.Type.ToString();
                MapObject procNPC = new MapObject(MapManager.ObjectType.NPC, npc.Coordinates, npcType);
                mapObjects.Add(procNPC);
            }
            foreach (PathPoint pathPoint in objectCollection.CurrentPathPoints)
            {
                string pathIDAsString = pathPoint.Id.ToString();
                MapObject procPath = new MapObject(MapManager.ObjectType.PathPoint, pathPoint.Coordinates, pathIDAsString);
                mapObjects.Add(procPath);
            }
            foreach (Misc misc in objectCollection.CurrentMiscs)
            {
                string miscTypeAsString = misc.Type.ToString();
                MapObject procMisc = new MapObject(MapManager.ObjectType.Misc, misc.Coordinates, miscTypeAsString);
                mapObjects.Add(procMisc);
            }
            foreach(Loot loot in objectCollection.CurrentLoots)
            {
                MapObject procLoot = new(MapManager.ObjectType.Loot, loot.Coordinates, loot.Texture);
                mapObjects.Add(procLoot);
            }
            foreach(CableViewModel cable in objectCollection.CurrentCables)
            {
                string color = cable.ColourBrush.ToString();
                MapObject procCable = new(MapManager.ObjectType.Cable, cable.Coordinates[0], color);
                mapObjects.Add(procCable);
            }
            foreach(ExitZone exitZone in objectCollection.CurrentExitZones)
            {
                MapObject procZone = new(MapManager.ObjectType.ExitZone, exitZone.Point1, "None");
                mapObjects.Add(procZone);
            }
        }

        public ObservableCollection<MapObject> mapObjects { get; set; }

        public int SelectedId { get; set; }

        [RelayCommand]
        public void Select()
        {
            MapObject selectedObject = mapObjects[SelectedId];
            switch (selectedObject.ObjectType)
            {
                case MapManager.ObjectType.None:
                    break;
                case MapManager.ObjectType.Wall:
                    break;
                case MapManager.ObjectType.Prop:
                    break;
                case MapManager.ObjectType.NPC:
                    break;
                case MapManager.ObjectType.PathPoint:
                    break;
                case MapManager.ObjectType.Floor:
                    break;
                case MapManager.ObjectType.Misc:
                    break;
                case MapManager.ObjectType.Door:
                    break;
                case MapManager.ObjectType.Loot:
                    break;
                case MapManager.ObjectType.ExitZone:
                    break;
                case MapManager.ObjectType.Cable:
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


            public MapObject(MapManager.ObjectType objectType, SnapCoordinate coordinate1, string texture1)
            {
                ObjectType = objectType;
                Coordinate1 = coordinate1;
                Texture1 = texture1;
            }
        }
    }
}
