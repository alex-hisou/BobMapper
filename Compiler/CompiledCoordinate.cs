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
        
        internal CompiledCoordinate(SnapCoordinate coordinate)
        {
            short convertedX = Convert.ToInt16(coordinate.XPos);
            short convertedY = Convert.ToInt16(coordinate.YPos);
            CompiledX = BitConverter.GetBytes(convertedX);
            CompiledY = BitConverter.GetBytes(convertedY);
        }
    }


}
