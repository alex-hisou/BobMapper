using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BobMapper.Model.MapObjects;

namespace BobMapper.Compiler
{
    internal static class CompilerServices
    {
        internal static Dictionary<Misc.MiscObjects, byte> CompiledMiscDict = new Dictionary<Misc.MiscObjects, byte>
            {
                {Misc.MiscObjects.Loot, 0x06},
                {Misc.MiscObjects.MainLoot, 0x07},
                {Misc.MiscObjects.Key,  0x08},
                {Misc.MiscObjects.SoundPoint, 0x0C},
                {Misc.MiscObjects.DisguisePoint, 0x0B}


            };


    }
}
