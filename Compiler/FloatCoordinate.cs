using System;
using System.Windows.Documents;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace BobMapper.Compiler
{
    internal class FloatCoordinate
    { 

        internal byte[] CompiledBytes { get; set; }
        internal bool hasRotation { get; set; }

        public FloatCoordinate(Coordinate coordinates, int rotation)
        {
            hasRotation = true;
            byte[] compiledX = GetFloatCompiledCoordinate(coordinates.XPos);
            byte[] compiledY = GetFloatCompiledCoordinate(coordinates.YPos);
            byte[] compiledRotation = GetCompiledRotation(rotation);
            CompiledBytes = new byte[16];
            Array.Copy(compiledX, 0, CompiledBytes, 0, compiledX.Length);
            Array.Copy(compiledY, 0, CompiledBytes, 4, compiledY.Length);
            Array.Copy(compiledRotation, 0, CompiledBytes, 12, compiledRotation.Length);
        }

        public FloatCoordinate(SnapCoordinate coordinates, int rotation)
        {
            hasRotation = true;
            byte[] compiledX = GetFloatCompiledCoordinate(coordinates.SnappedXPos);
            byte[] compiledY = GetFloatCompiledCoordinate(coordinates.SnappedYPos);
            byte[] compiledRotation = GetCompiledRotation(rotation);
            CompiledBytes = new byte[16];
            Array.Copy(compiledX, 0, CompiledBytes, 0, compiledX.Length);
            Array.Copy(compiledY, 0, CompiledBytes, 4, compiledY.Length);
            Array.Copy(compiledRotation, 0, CompiledBytes, 12, compiledRotation.Length);
        }

        public FloatCoordinate(SnapCoordinate coordinates)
        {
            hasRotation = false;
            byte[] compiledX = GetFloatCompiledCoordinate(coordinates.SnappedXPos);
            byte[] compiledY = GetFloatCompiledCoordinate(coordinates.SnappedYPos);
            CompiledBytes = new byte[8];
            Array.Copy(compiledX, 0, CompiledBytes, 0, compiledX.Length );
            Array.Copy(compiledY, 0, CompiledBytes, 4, compiledY.Length );
        }

        private byte[] GetCompiledRotation(int rotation)
        {
            //TODO: Test rotation range
            float testFloat = 1;
            byte[] rotationBytes = BitConverter.GetBytes(testFloat);
            return rotationBytes;
        }

        private byte[] GetFloatCompiledCoordinate(int anyCoordinate)
        {
            float halfCoordinate = (float)anyCoordinate / 2;
            byte[] floatAsBytes = BitConverter.GetBytes(halfCoordinate);
            return floatAsBytes;
        }

        private byte[] GetFloatCompiledCoordinate(float anyCoordinate)
        {
            //float halfCoordinate = (float)anyCoordinate / 2;
            byte[] floatAsBytes = BitConverter.GetBytes(anyCoordinate);
            return floatAsBytes;
        }

        public SnapCoordinate GetSnapCoordinate()
        {
            byte[] xArray = { CompiledBytes[0], CompiledBytes[1], CompiledBytes[2], CompiledBytes[3] };
            float xFloat = BitConverter.ToSingle(xArray, 0);
            int uncompiledX = Convert.ToInt32(xFloat * 2);
            byte[] yArray = { CompiledBytes[4], CompiledBytes[5], CompiledBytes[6], CompiledBytes[7] };
            float yFloat = BitConverter.ToSingle(yArray, 0);
            int uncompiledY = Convert.ToInt32(yFloat * 2);
            SnapCoordinate snapCoordinate = new SnapCoordinate(uncompiledX, uncompiledY);
            return snapCoordinate;
        }
    }
}
