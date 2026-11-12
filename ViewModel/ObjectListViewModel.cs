using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model;
using BobMapper.Model.MapObjects;

namespace BobMapper.ViewModel
{
    internal class ObjectListViewModel : ViewModelBase
    {
        public ObjectListViewModel(Map map)
        {
            mapObjects = new();
            foreach (Wall wall in map.walls)
            {
                string procTexture = $"{wall.Texture1} || {wall.Texture2}";
                MapObject procWall = new MapObject(MapManager.ObjectType.Wall, wall.Point1, procTexture);
                mapObjects.Add(procWall);
            }
            foreach (Door door in map.doors)
            {
                MapObject procDoor = new MapObject(MapManager.ObjectType.Door, door.Point1, door.Texture1);
                mapObjects.Add(procDoor);
            }
            foreach (Prop prop in map.props)
            {
                MapObject procProp = new MapObject(MapManager.ObjectType.Prop, prop.Coordinates, prop.PropTexture);
                mapObjects.Add(procProp);
            }
            foreach (NPC npc in map.npcs)
            {
                string npcType = npc.Type.ToString();
                MapObject procNPC = new MapObject(MapManager.ObjectType.NPC, npc.Coordinates, npcType);
                mapObjects.Add(procNPC);
            }
            foreach (PathPoint pathPoint in map.pathPoints)
            {
                string pathIDAsString = pathPoint.Id.ToString();
                MapObject procPath = new MapObject(MapManager.ObjectType.PathPoint, pathPoint.Coordinates, pathIDAsString);
                mapObjects.Add(procPath);
            }
            foreach (Misc misc in map.miscs)
            {
                string miscTypeAsString = misc.Type.ToString();
                MapObject procMisc = new MapObject(MapManager.ObjectType.Misc, misc.Coordinates, miscTypeAsString);
                mapObjects.Add(procMisc);
            }
            foreach(Loot loot in map.loots)
            {
                MapObject procLoot = new(MapManager.ObjectType.Loot, loot.Coordinates, loot.Texture);
                mapObjects.Add(procLoot);
            }
            foreach(Cable cable in map.cables)
            {
                MapObject procCable = new(MapManager.ObjectType.Cable, cable.Coordinates[0], cable.ColourHex);
                mapObjects.Add(procCable);
            }
            foreach(ExitZone exitZone in map.exitZones)
            {
                MapObject procZone = new(MapManager.ObjectType.ExitZone, exitZone.Point1, "None");
                mapObjects.Add(procZone);
            }
        }

        public ObservableCollection<MapObject> mapObjects { get; set; }

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
