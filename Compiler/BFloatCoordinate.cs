using System;
using System.Windows.Documents;
using System.Numerics;
using UltimateOrb;
using System.Runtime.CompilerServices;

namespace BobMapper.Compiler
{
    internal class BFloatCoordinate
    { 

        internal byte[] CompiledBytes { get; set; }
        internal bool hasRotation { get; set; }

        public BFloatCoordinate(Coordinate coordinates, int rotation)
        {
            hasRotation = true;
            byte[] compiledX = GetBFloatCompiledCoordinate(coordinates.XPos);
            byte[] compiledY = GetBFloatCompiledCoordinate(coordinates.YPos);
            byte compiledRotation = GetCompiledRotation(rotation);
            CompiledBytes = new byte[14];
            CompiledBytes[0] = compiledX[0];
            CompiledBytes[1] = compiledX[1];
            CompiledBytes[4] = compiledY[0];
            CompiledBytes[5] = compiledY[1];
            CompiledBytes[13] = compiledRotation;

        }

        public BFloatCoordinate(Coordinate coordinates)
        {
            hasRotation = false;
            byte[] compiledX = GetBFloatCompiledCoordinate(coordinates.XPos);
            byte[] compiledY = GetBFloatCompiledCoordinate(coordinates.YPos);
            CompiledBytes = new byte[6];
            CompiledBytes[0] = compiledX[0];
            CompiledBytes[1] = compiledX[1];
            CompiledBytes[4] = compiledY[0];
            CompiledBytes[5] = compiledY[1];
        }

        private byte GetCompiledRotation(int rotation)
        {
            //TODO: Test rotation range
            return (byte)0;
        }

        private byte[] GetBFloatCompiledCoordinate(int anyCoordinate)
        {
            float halfCoordinate = (float)anyCoordinate / 2;
            //TODO: Replace with std library when upgrading to .NET 11
            BFloat16 bFloat16 = BFloat16.CreateTruncating(halfCoordinate);
            byte[] bFloatAsBytes = BitConverter.GetBytes(bFloat16);
            return bFloatAsBytes;
        }

        public SnapCoordinate GetSnapCoordinate()
        {
            //Super unsafe code, needs urgent replacement when .NET 11 arrives
            byte[] xArray = { CompiledBytes[0], CompiledBytes[1] };
            BFloat16 xBFloat = Unsafe.As<byte, BFloat16>(ref xArray[0]);
            int uncompiledX = Convert.ToInt32(xBFloat * 2);
            byte[] yArray = { CompiledBytes[4], CompiledBytes[5] };
            BFloat16 yBFloat = Unsafe.As<byte, BFloat16>(ref xArray[0]);
            int uncompiledY = Convert.ToInt32(yBFloat * 2);
            SnapCoordinate snapCoordinate = new SnapCoordinate(uncompiledX, uncompiledY);
            return snapCoordinate;
        }
    }
}
