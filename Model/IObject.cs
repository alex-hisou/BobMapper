using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BobMapper.Model
{
    internal interface IObject
    {
        Coordinate Coordinates { get; set; }
        bool IsSelected { get; set; }

        void UpdatePos(Coordinate newCoordinate);
        void DeleteObject();
    }
}
