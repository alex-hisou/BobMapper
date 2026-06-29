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
                    byte[] navNode = new byte[26];
                    navNode[0] = 0x01;
                    byte[] idAsBytes = BitConverter.GetBytes(index);
                    Array.Copy(idAsBytes, 0, navNode, 22, 4);
                    navMeshBytes.AddRange(navNode);
                    currentX += 0.5f;
                }
                currentY += 0.5f;
            }
            return navMeshBytes;
        }

        private byte[] NavNodeVariablesAsBytes(List<Wall> walls, List<Door> doors, List<Prop> props)
        {
            byte[] NodeVariablesAsBytes = new byte[99999999];
            foreach (Wall wall in walls)
            {

            }
            foreach (Door door in doors)
            {

            }

            //check wall, door or prop collisions and their respective properties and early return them
            //check room id and return
            return NodeVariablesAsBytes;
        }

        private bool isPointOnLine()
        {

        }

        private float Distance(Coordinate point1, Coordinate point2)
        {
            
        }
    }
}
