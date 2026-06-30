using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model.MapObjects;

namespace BobMapper.Compiler.WriteSteps
{
    internal class NavMesh
    {
        internal List<byte> navMeshOutput;
        internal NavMesh(int width, int height, List<Wall> walls, List<Door> doors, List<Prop> props) 
        {
            navMeshOutput = new List<byte>();
            navMeshOutput.AddRange([0x08, 0x00, 0x00, 0x00]); //SECTION HEAD
            byte[] navigationMeshLabel = Encoding.ASCII.GetBytes("NavigationMesh");
            navMeshOutput.AddRange(navigationMeshLabel);
            //TODO: Add all the mysterious stuff and make sure this code works with rectangular maps
            List<byte> navMeshByteBuffer = NavMeshAsBytes(width, height, walls, doors, props);
            

        }

        private List<byte> NavMeshAsBytes(int width, int height, List<Wall> walls, List<Door> doors, List<Prop> props)
        {
            List<byte> navMeshBytes = new List<byte>();
            //Maybe swap width and height?
            float startY = (float)width / -2;
            float endY = (float)width / 2;
            float currentY = startY;
            int index = 0;
            while (currentY <= endY)
            {
                float currentX = (float)height / -2;
                float endX = height / 2;
                while (currentX <= endX)
                {
                    byte[] navNodeBytes = new byte[26];
                    navNodeBytes[0] = 0x01;
                    SnapCoordinate navNodePos = new(currentX, currentY);
                    byte[] idAsBytes = BitConverter.GetBytes(index);
                    Array.Copy(idAsBytes, 0, navNodeBytes, 22, 4);
                    navMeshBytes.AddRange(navNodeBytes);
                    currentX += 0.5f;
                }
                currentY += 0.5f;
            }
            return navMeshBytes;
        }

        private byte[] NavNodeVariables(SnapCoordinate navNodePos,List<Wall> walls, List<Door> doors, List<Prop> props, List<Room> rooms)
        {
            byte roomId = 0;
            byte objectCollision = 0;
            bool isWalkable = true;
            byte lockedByDefault = 0;
            foreach (Wall wall in walls)
            {
                if(isPointOnLine(wall.Point1, wall.Point2, navNodePos))
                {
                    objectCollision = 3;
                    isWalkable = false;
                    return VariablesAsBytes();
                }
            }
            foreach (Door door in doors)
            {
                if(isPointOnLine(door.Point1, door.Point2, navNodePos))
                {
                    objectCollision = 1;
                    if(door.Locked)
                    {
                        lockedByDefault = 10;
                    }
                    return VariablesAsBytes();
                }
            }
            int intRoomId = Room.GetPointRoomId(navNodePos.SnappedXPos, navNodePos.SnappedYPos, rooms);
            roomId = Convert.ToByte(intRoomId);
            return VariablesAsBytes();
        }

        private byte[] VariablesAsBytes()
        {

        }

        private bool isPointOnLine(SnapCoordinate linePoint1, SnapCoordinate linePoint2, SnapCoordinate inputPoint)
        {
            //a b line c point
            float distance1 = Distance(linePoint1.SnappedXPos, linePoint1.SnappedYPos, inputPoint.SnappedXPos, inputPoint.SnappedYPos);
            float distance2 = Distance(linePoint2.SnappedXPos, linePoint2.SnappedYPos, inputPoint.SnappedXPos, inputPoint.SnappedYPos);
            float distance3 = Distance(linePoint1.SnappedXPos, linePoint1.SnappedYPos, linePoint2.SnappedXPos, linePoint2.SnappedYPos);
            return distance1 + distance2 == distance3;
        }

        private float Distance(float point1X, float point1Y, float point2X, float point2Y)
        {
            double part1 = Math.Pow((point1X - point2X), 2);
            double part2 = Math.Pow((point1Y - point2Y), 2);
            double sqrt = Math.Sqrt(part1 + part2);
            return (float)sqrt;
        }
    }
}
