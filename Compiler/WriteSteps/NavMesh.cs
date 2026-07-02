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
        private const int navNodeLength = 26; 
        internal NavMesh(int width, int height, List<Wall> walls, List<Door> doors, List<Prop> props) 
        {
            navMeshOutput = new List<byte>();
            navMeshOutput.AddRange([0x08, 0x00, 0x00, 0x00]); //SECTION HEAD
            byte[] navigationMeshLabel = Encoding.ASCII.GetBytes("NavigationMesh");
            navMeshOutput.AddRange(navigationMeshLabel);
            //TODO: Add all the mysterious stuff and make sure this code works with rectangular maps
            List<byte> navMeshByteBuffer = NavMeshAsBytes(width, height, walls, doors, props);
            navMeshOutput.AddRange(BitConverter.GetBytes(navMeshByteBuffer.Count + 8)); //Add 8 bytes due to the order of steps
            navMeshOutput.AddRange(BitConverter.GetBytes(navMeshByteBuffer.Count / navNodeLength));
            navMeshOutput.AddRange(BitConverter.GetBytes((int)56)); //Dafuq?!?!?!
            navMeshOutput.AddRange(navMeshByteBuffer);

        }

        private List<byte> NavMeshAsBytes(int width, int height, List<Wall> walls, List<Door> doors, List<Prop> props)
        {
            List<byte> navMeshBytes = new List<byte>();
            List<Room> rooms = Room.GenerateRooms(walls, doors);
            //Maybe swap width and height?
            float startY = (float)height / -2;
            float endY = (float)height / 2;
            float currentY = startY;
            int index = 0;
            while (currentY <= endY)
            {
                float currentX = (float)width / -2;
                float endX = width / 2;
                while (currentX <= endX)
                {
                    byte[] navNodeBytes = new byte[navNodeLength];
                    navNodeBytes[0] = 0x01;
                    byte[] xBytes = BitConverter.GetBytes(currentX);
                    Array.Copy(xBytes, 0, navNodeBytes, 2, xBytes.Length);
                    byte[] yBytes = BitConverter.GetBytes(currentY);
                    Array.Copy(yBytes, 0, navNodeBytes, 6, yBytes.Length);
                    SnapCoordinate navNodePos = new(currentX, currentY);
                    byte[] navNodeVariables = NavNodeVariables(navNodePos, walls, doors, props, rooms);
                    Array.Copy(navNodeVariables, 0, navNodeBytes, 10, navNodeVariables.Length);
                    byte[] idAsBytes = BitConverter.GetBytes(index);
                    Array.Copy(idAsBytes, 0, navNodeBytes, 22, 4);
                    navMeshBytes.AddRange(navNodeBytes);
                    currentX += 0.5f;
                    index++;
                }
                currentY += 0.5f;
            }
            return navMeshBytes;
        }

        private byte[] NavNodeVariables(SnapCoordinate navNodePos,List<Wall> walls, List<Door> doors, List<Prop> props, List<Room> rooms)
        {
            byte roomId = 1; //outside default
            byte objectCollision = 0;
            bool isNonWalkable = false;
            byte lockedByDefault = 0;
            foreach (Wall wall in walls)
            {
                if(isPointOnLine(wall.Point1, wall.Point2, navNodePos))
                {
                    objectCollision = 3;
                    isNonWalkable = true;
                    roomId = 0;
                    return VariablesAsBytes(roomId, objectCollision, isNonWalkable, lockedByDefault);
                }
            }
            foreach (Door door in doors)
            {
                if(isPointOnLine(door.Point1, door.Point2, navNodePos))
                {
                    objectCollision = 1;
                    if(door.Locked || door.PermLocked)
                    {
                        lockedByDefault = 10;
                    }
                    roomId = 0;
                    return VariablesAsBytes(roomId, objectCollision, isNonWalkable, lockedByDefault);
                }
            }
            int intRoomId = Room.GetPointRoomId(navNodePos.SnappedXPos, navNodePos.SnappedYPos, rooms);
            roomId = Convert.ToByte(intRoomId);
            return VariablesAsBytes(roomId, objectCollision, isNonWalkable, lockedByDefault);
        }

        private byte[] VariablesAsBytes(byte roomId, byte objectCollision, bool isWalkable, byte lockedByDefault)
        {
            byte[] variablesAsBytes = new byte[12];
            variablesAsBytes[4] = roomId;
            variablesAsBytes[6] = objectCollision;
            variablesAsBytes[8] = Convert.ToByte(isWalkable);
            variablesAsBytes[10] = lockedByDefault;
            return variablesAsBytes;
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
