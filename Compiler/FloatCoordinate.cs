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
            byte compiledRotation = GetCompiledRotation(rotation);
            CompiledBytes = new byte[16];
            Array.Copy(compiledX, 0, CompiledBytes, 0, compiledX.Length);
            Array.Copy(compiledY, 0, CompiledBytes, 4, compiledY.Length);
            CompiledBytes[15] = compiledRotation;

        }

        public FloatCoordinate(Coordinate coordinates)
        {
            hasRotation = false;
            byte[] compiledX = GetFloatCompiledCoordinate(coordinates.XPos);
            byte[] compiledY = GetFloatCompiledCoordinate(coordinates.YPos);
            CompiledBytes = new byte[8];
            Array.Copy(compiledX, 0, CompiledBytes, 0, compiledX.Length );
            Array.Copy(compiledY, 0, CompiledBytes, 4, compiledY.Length );
        }

        private byte GetCompiledRotation(int rotation)
        {
            //TODO: Test rotation range
            return (byte)0;
        }

        private byte[] GetFloatCompiledCoordinate(int anyCoordinate)
        {
            float halfCoordinate = (float)anyCoordinate / 2;
            byte[] floatAsBytes = BitConverter.GetBytes(halfCoordinate);
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
