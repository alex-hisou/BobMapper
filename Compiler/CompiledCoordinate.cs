using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobMapper.Compiler
{
    internal class CompiledCoordinate
    {
        internal byte[] CompiledX;
        internal byte[] CompiledY;
        internal CompiledCoordinate(Coordinate coordinate, bool startsFrom64) 
        {
            short convertedX = Convert.ToInt16(coordinate.XPos);
            short convertedY = Convert.ToInt16(coordinate.YPos);
            CompiledX = BitConverter.GetBytes(convertedX);
            CompiledY = BitConverter.GetBytes(convertedY);
        }

        internal CompiledCoordinate(SnapCoordinate coordinate, bool startsFrom64)
        {
            short convertedX = Convert.ToInt16(coordinate.XPos);
            short convertedY = Convert.ToInt16(coordinate.YPos);
            CompiledX = BitConverter.GetBytes(convertedX);
            CompiledY = BitConverter.GetBytes(convertedY);
        }
    }
}
