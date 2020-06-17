using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace TestUI_New
{
    public class ListStringConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is List<double>)
            {
                string str="";
                foreach (double d in (value as List<double>))
                {
                    str += d.ToString("f2");
                    str += ",";
                }
                return str;
            }
            else
            {
                return value.ToString();
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
