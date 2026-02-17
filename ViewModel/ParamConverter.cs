using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BobMapper.ViewModel
{
    internal class ParameterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var propertyDefinition = new PropertyDefinition();
            propertyDefinition.sender = (object)values[0];
            propertyDefinition.value = (string)values[1];
            propertyDefinition.property = (string)values[2];
            return propertyDefinition;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class PropertyDefinition
    {
        internal object sender { get; set; }
        internal string value { get; set; }

        internal string property { get; set; }

    }
}
