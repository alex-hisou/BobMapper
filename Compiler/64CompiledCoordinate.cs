using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobMapper.Compiler
{
    internal class _64CompiledCoordinate
    { 
        
        internal byte[] CompiledX { get; set; }
        internal byte[] CompiledY { get; set; }

        internal byte CompiledRotation { get; set; }

        public _64CompiledCoordinate(Coordinate coordinates, int rotation)
        {
            CompiledRotation = GetCompiledRotation(rotation);

        }

        private byte GetCompiledRotation(int rotation)
        {

        }
    }
}
