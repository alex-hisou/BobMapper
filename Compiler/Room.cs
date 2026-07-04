using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model.MapObjects;
using NetTopologySuite.Geometries;
using NetTopologySuite.Operation.Polygonize;

namespace BobMapper.Compiler
{
    internal class Room
    {
        internal int Id { get; set; }
        internal Polygon Area { get; set; }
        
        internal Room(int id, Polygon area)
        {
            Id = id;
            Area = area;
        }

        internal static List<Room> GenerateRooms(List<Wall> walls, List<Door> doors)
        {
            //Using dependency cause im lazy afffffffffffffff
            List<Room> rooms = new List<Room>();
            var polygonizer = new Polygonizer();
            foreach (Wall wall in walls)
            {
                NetTopologySuite.Geometries.Coordinate point1 = new(wall.Point1.SnappedXPos, wall.Point1.SnappedYPos);
                NetTopologySuite.Geometries.Coordinate point2 = new(wall.Point2.SnappedXPos, wall.Point2.SnappedYPos);
                LineString lineString = new LineString(new[] { point1, point2 });
                polygonizer.Add(lineString);
            }
            foreach (Door door in doors)
            {
                NetTopologySuite.Geometries.Coordinate point1 = new(door.Point1.SnappedXPos, door.Point1.SnappedYPos);
                NetTopologySuite.Geometries.Coordinate point2 = new(door.Point2.SnappedXPos, door.Point2.SnappedYPos);
                LineString lineString = new LineString(new[] { point1, point2 });
                polygonizer.Add(lineString);
            }
            var polygons = polygonizer.GetPolygons().Cast<Polygon>().ToList();
            if(polygons.Count == 0)
            {
                return rooms;
            }
            for (int i = 2; i <= polygons.Count + 1; i++)
            {
                Room room = new Room(i, polygons[i - 2]);
                rooms.Add(room);
            }
            return rooms;
        }

        internal static int GetPointRoomId(float x, float y, List<Room> rooms)
        {
            //This gets run after we know a point is not on a wall or a door, so a wall check is not needed
            Point point = new(x, y);
            Room room = rooms.FirstOrDefault(x => x.Area.Covers(point));
            if(room == null)
            {
                return 1;
            }
            return room.Id;
        }
    }
}
