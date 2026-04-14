using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model.MapObjects;

namespace BobMapper.Compiler
{
    internal static class CompilerServices
    {

        internal static int[] GetConnectFromIds(List<PathPoint> pathPoints)
        {
            int[] connectFromIds = new int[pathPoints.Count];
            for (int i = 0; i < connectFromIds.Length; i++)
            {
                connectFromIds[i] = pathPoints.First(x => x.ConnectToId == pathPoints[i].Id).Id;
            }
            return connectFromIds;
        }


    }
}
