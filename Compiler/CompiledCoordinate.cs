using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobMapper.Compiler
{
    internal class CompiledCoordinate
    {
        internal byte[] CompiledBytes;
        
        internal CompiledCoordinate(SnapCoordinate coordinate)
        {
            short convertedX = Convert.ToInt16(coordinate.XPos);
            short convertedY = Convert.ToInt16(coordinate.YPos);
            byte[] CompiledX = BitConverter.GetBytes(convertedX);
            byte[] CompiledY = BitConverter.GetBytes(convertedY);
            CompiledBytes = new byte[4];
            CompiledBytes[0] = CompiledX[0];
            CompiledBytes[1] = CompiledX[1];
            CompiledBytes[2] = CompiledY[0];
            CompiledBytes[3] = CompiledY[1];
        }
    }


}
