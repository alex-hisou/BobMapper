using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model;

namespace BobMapper.ViewModel
{
    internal class EditorViewModel
    {
        internal MapObjects.Type selectedObjectType;
        internal int selectedObjectIndex;
        internal enum Tools
        {
            None,
            Select,
            Move,
            Rotate,
            AddWall,
            AddProp,
            AddNPC
        }
    }
}
