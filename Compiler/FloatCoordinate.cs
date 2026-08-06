using System;
using System.Windows.Documents;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace BobMapper.Compiler
{
    internal class FloatCoordinate
    { 

        internal byte[] CompiledBytes { get; set; }
        internal bool HasRotation { get; set; }

        private bool isShortened;

        public FloatCoordinate(SnapCoordinate coordinates, float rotation, bool isShortened)
        {
            HasRotation = true;
            this.isShortened = isShortened;
            byte[] compiledX = GetFloatCompiledCoordinate(coordinates.SnappedXPos);
            byte[] compiledY = GetFloatCompiledCoordinate(coordinates.SnappedYPos);
            byte[] compiledRotation = GetCompiledRotation(rotation);
            int length;
            int rotationIndex;
            if(isShortened)
            {
                length = 12;
                rotationIndex = 8;
            }
            else 
            {
                length = 16;
                rotationIndex = 12;
            }
            CompiledBytes = new byte[length];
            Array.Copy(compiledX, 0, CompiledBytes, 0, compiledX.Length);
            Array.Copy(compiledY, 0, CompiledBytes, 4, compiledY.Length);
            Array.Copy(compiledRotation, 0, CompiledBytes, rotationIndex, compiledRotation.Length);
        }

        public FloatCoordinate(SnapCoordinate coordinates)
        {
            HasRotation = false;
            byte[] compiledX = GetFloatCompiledCoordinate(coordinates.SnappedXPos);
            byte[] compiledY = GetFloatCompiledCoordinate(coordinates.SnappedYPos);
            CompiledBytes = new byte[8];
            Array.Copy(compiledX, 0, CompiledBytes, 0, compiledX.Length );
            Array.Copy(compiledY, 0, CompiledBytes, 4, compiledY.Length );
        }

        private byte[] GetCompiledRotation(float rotation)
        {
            float radians = rotation * (float)Math.PI / 180;
            if(isShortened)
            {
                radians *= -1;
            }
            if(radians == 0)
            {
                radians = 0.00000001f;
            }
            byte[] rotationBytes = BitConverter.GetBytes(radians);
            return rotationBytes;
        }

        private byte[] GetFloatCompiledCoordinate(float anyCoordinate)
        {
            //float halfCoordinate = (float)anyCoordinate / 2;
            byte[] floatAsBytes = BitConverter.GetBytes(anyCoordinate);
            return floatAsBytes;
        }
    }
}
