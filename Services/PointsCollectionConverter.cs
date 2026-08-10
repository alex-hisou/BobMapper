using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace BobMapper.Services
{
    internal class PointsCollectionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            PointCollection points = new PointCollection();
            foreach (var value in values)
            {
                if(value is SnapCoordinate snapCoordinate)
                {
                    points.Add(snapCoordinate);
                }
            }
            return points;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
