using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BobMapper.Model.Misc;
using static BobMapper.Model.NPC;
using static BobMapper.Model.Wall;

namespace BobMapper.Model
{
    internal interface ISinglePointObject
    {
        Coordinate Coordinates { get; set; }

        void UpdatePos(Coordinate newCoordinate);
        void DeleteObject();
    }

    internal interface IDoublePointObject
    {
        public Coordinate Point1 { get; set; }
        public Coordinate Point2 { get; set; }
        public string Texture1 { get; set; }
        public string Texture2 { get; set; }
    }
}
