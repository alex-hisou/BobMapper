using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobMapper.Compiler
{
    internal class QueuedLocator
    {
        internal LocatorTypes LocatorType { get; set; }
        internal Coordinate Coordinates { get; set; }
        internal QueuedLocator(LocatorTypes locatorType, Coordinate coordinate) 
        {
            LocatorType = locatorType;
            Coordinates = coordinate;
        }

        internal enum LocatorTypes
        {
            Lock = 9,
            Loot = 6,
            PermanentLock = 13,
            MainLoot = 7
        }
    }
}
