using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Text.RegularExpressions;
namespace TestUI_New
{
    public class TimeStringConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                double RealValue = (double)value;
                int n = (int)Math.Floor(Math.Log10(RealValue) / 3);
                string preStr = (RealValue / Math.Pow(10, 3 * n)).ToString();

                switch (n)
                {
                    case 0:
                        return preStr;
                    case -1:
                        return preStr + "m";
                    case -2:
                        return preStr + "u";
                    case -3:
                        return preStr + "n";
                    default:
                        return value;
                }
            }
            catch
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string s = value as string;
            if (s != null)
            {
                
                double d = 1;
                double inputvalue;
                double time = -1;
                if (double.TryParse(s, out d))
                {
                    return d;
                }
                s = s.Replace(" ", "");
                string reg = @"^\d*\.?\d*m";
                Match m = Regex.Match(s, reg);
                if (m.Success)
                {
                    d = 1e-3;
                }
                else
                {
                    reg = @"^\d*\.?\d*u";
                    m = Regex.Match(s, reg);
                    if (m.Success)
                        d = 1e-6;
                    else
                    {
                        reg = @"^\d*\.?\d*n";
                        m = Regex.Match(s, reg);
                        if (m.Success)
                            d = 1e-9;
                        else
                        {
                            reg = @"^\d*\.?\d*";
                            m = Regex.Match(s, reg);
                            if (m.Success)
                                d = 1;
                        }
                    }
                }

                if (m.Success)
                {
                    reg = @"^\d*\.?\d*";
                    m = Regex.Match(m.Value, reg);
                    if (double.TryParse(m.Value, out inputvalue))
                    {
                        time = inputvalue * d;
                        return time;
                    }
                    else
                    {
                        return value;
                    }
                }
                else
                {
                    return value;
                }
            }
            else
            {
                return value;
            }
        }
    }
}
