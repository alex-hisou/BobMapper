using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BobMapper.Model.MapObjects.Misc;
using static BobMapper.Model.MapObjects.NPC;
using static BobMapper.Model.MapObjects.Wall;

namespace BobMapper.Model
{
    internal interface ISinglePointObject
    {
        Coordinate Coordinates { get; set; }
        void DeleteObject();
    }

    internal interface IDoublePointObject
    {
        public SnapCoordinate Point1 { get; set; }
        public SnapCoordinate Point2 { get; set; }
        public string Texture1 { get; set; }
    }
}
