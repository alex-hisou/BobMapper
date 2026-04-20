using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobMapper.Model
{
    internal interface ISinglePointObject
    {
        Coordinate Coordinates { get; set; }
        
    }

    internal interface IDoublePointObject
    {
        public SnapCoordinate Point1 { get; set; }
        public SnapCoordinate Point2 { get; set; }
        public string Texture1 { get; set; }

        public string InternalTexture1 { get; set; }
    }
}
