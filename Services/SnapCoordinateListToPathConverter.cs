using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace BobMapper.Services
{
    class SnapCoordinateListToPathConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is not IEnumerable<SnapCoordinate> coordinates)
                return Geometry.Empty;

            var points = coordinates.Select(c => new Point(c.XPos, c.YPos)).ToList();

            if (points.Count == 0)
                return Geometry.Empty;

            var figure = new PathFigure
            {
                StartPoint = points[0],
                IsClosed = false,
                IsFilled = false
            };
            if (points.Count > 1)
            {
                figure.Segments.Add(new PolyLineSegment(points.Skip(1), true));
            }
            return new PathGeometry(new[] { figure });
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
