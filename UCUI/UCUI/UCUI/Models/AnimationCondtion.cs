using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UCUI.Models
{
    class AnimationCondition : IMultiValueConverter
    {
        //[0]:Name, [1]:ButtonKey, [2]:IsPressed, [3]:IsMouseOver, [4]:IsHover
        public object Convert(object[] values, Type targetType,
               object parameter, System.Globalization.CultureInfo culture)
        {
            bool result =
                (System.Convert.ToString(values[0])).Equals(System.Convert.ToString(values[1]));
            result=result|| (System.Convert.ToString(values[2])).Equals("True")||((System.Convert.ToString(values[3])).Equals("True") && ((System.Convert.ToString(values[4])).Equals("True")));

            return result.ToString();
        }
        public object[] ConvertBack(object value, Type[] targetTypes,
               object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Cannot convert back");
        }
    }
}
