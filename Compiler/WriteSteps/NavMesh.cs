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
        internal NavMesh(int width, int height, List<Wall> walls) 
        {
            navMeshOutput = new List<byte>();
            navMeshOutput.AddRange([0x08, 0x00, 0x00, 0x00]); //SECTION HEAD
            byte[] navigationMeshLabel = Encoding.ASCII.GetBytes("NavigationMesh");
            navMeshOutput.AddRange(navigationMeshLabel);
            int navmeshSize = (width + height) + 1;
            

        }

        private bool DoesIntersect(NavLine navLine, List<Wall> walls)
        {
            foreach (Wall wall in walls)
            {
                int o1 = Orientation(navLine.Point1, navLine.Point2, wall.Point1);
                int o2 = Orientation(navLine.Point1, navLine.Point2, wall.Point2);
                int o3 = Orientation(wall.Point1, wall.Point2, navLine.Point1);
                int o4 = Orientation(wall.Point1, wall.Point2, navLine.Point2);

                if (o1 != o2 && o3 != o4)
                    return true;
            }
            return false;
        }

        private int Orientation(SnapCoordinate point1, SnapCoordinate point2, SnapCoordinate point3)
        {
            double value = (point2.SnappedXPos - point1.SnappedXPos) * (point3.SnappedYPos - point1.SnappedYPos) -
                (point2.SnappedYPos - point1.SnappedYPos) * (point3.SnappedXPos - point1.SnappedXPos);
            if (value > 0)
                return 1; //cw
            else if (value < 0)
                return -1; //ccw
            else return 0; //colin.
        }

        private class NavLine
        {
            internal SnapCoordinate Point1 { get; set; }
            internal SnapCoordinate Point2 { get; set; }

            internal NavLine(SnapCoordinate point1, SnapCoordinate point2) 
            {
                Point1 = point1;
                Point2 = point2;
            }
        }

    }
}
