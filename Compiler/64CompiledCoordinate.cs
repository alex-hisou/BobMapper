using System;

namespace BobMapper.Compiler
{
    internal class _64CompiledCoordinate
    { 

        internal byte[] CompiledBytes { get; set; }

        public _64CompiledCoordinate(Coordinate coordinates, int rotation)
        {
            byte[] compiledX = Get64CompiledCoordinate(coordinates.XPos);
            byte[] compiledY = Get64CompiledCoordinate(coordinates.YPos);
            byte compiledRotation = GetCompiledRotation(rotation);
            CompiledBytes = new byte[14];
            CompiledBytes[0] = compiledX[0];
            CompiledBytes[1] = compiledX[1];
            CompiledBytes[4] = compiledY[0];
            CompiledBytes[5] = compiledY[1];
            CompiledBytes[13] = compiledRotation;

        }

        private byte GetCompiledRotation(int rotation)
        {
            //TODO: Test rotation range
            return (byte)0;
        }

        private byte[] Get64CompiledCoordinate(int anyCoordinate)
        {
            //Voodoo magic code, don't look too deep into it
            byte[] compiled64coordinate = new byte[2];
            compiled64coordinate[0] = (byte)Math.Abs(anyCoordinate % 256);
            int virtualQuadrant = anyCoordinate / 256;
            int preProcessedQuadrant = Math.Abs(virtualQuadrant);
            //Converts normal quadrant into a power-of-2 quadrant the game uses
            preProcessedQuadrant = Convert.ToInt32(Math.Floor(Math.Log2(preProcessedQuadrant)));

            //the lowest quadrants from which the game starts counting, -65 for negative coordinates, 63 for positive
            int compiledQuadrant = anyCoordinate < 0 ? -65 : 63; 
            compiledQuadrant += preProcessedQuadrant;
            compiled64coordinate[1] = (byte)compiledQuadrant;
            return compiled64coordinate;
        }
    }
}
