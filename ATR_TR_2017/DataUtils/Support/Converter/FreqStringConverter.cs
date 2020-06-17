using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Text.RegularExpressions;
namespace TestUI_New
{
    public class FreqStringConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                int n = (int)Math.Floor(Math.Log10((double)value) / 3);
                string preStr = ((double)value / Math.Pow(10, 3 * n)).ToString();
                switch (n)
                {
                    case 0:
                        return preStr;
                    case 1:
                        return preStr + "K";
                    case 2:
                        return preStr + "M";
                    case 3:
                        return preStr + "G";
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
                double freq = -1;
                if (double.TryParse(s, out d))
                {
                    return d;
                }
                s = s.ToUpper();
                s = s.Replace(" ", "");
                string reg = @"^\d*\.?\d*K";
                Match m = Regex.Match(s, reg);
                if (m.Success)
                {
                    d = 1e3;
                }
                else
                {
                    reg = @"^\d*\.?\d*M";
                    m = Regex.Match(s, reg);
                    if (m.Success)
                        d = 1e6;
                    else
                    {
                        reg = @"^\d*\.?\d*G";
                        m = Regex.Match(s, reg);
                        if (m.Success)
                            d = 1e9;
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
                        freq = inputvalue * d;
                        return freq;
                    }
                    else
                        return value;
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
