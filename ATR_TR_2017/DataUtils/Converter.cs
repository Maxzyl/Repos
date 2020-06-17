using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace DataUtils
{
    public class FreqStringConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                double freq;
                if (double.TryParse(value.ToString(), out freq))
                {
                    int n=0;
                    if (freq != 0)
                    {
                        n = (int)Math.Floor(Math.Log10(freq) / 3);
                    }
                    string preStr = (freq / Math.Pow(10, 3 * n)).ToString();
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
                else
                {
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

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class TimeStringConverter :IValueConverter
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

    public class DebugDataBindingConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            Debugger.Break();
            return value;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            Debugger.Break();
            return value;
        }
    }

    public class SIPrefixConverter : MarkupExtension, IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
             try
             {
                 double d = System.Convert.ToDouble(value);

                 int n = 0;
                 double m = 0;
                 if (d == 0)
                 {
                     n = 0;
                 }
                 else
                 {
                     m = Math.Log10(Math.Abs(d)) / 3;
                 }
                 //int n =m>0? (int)Math.Floor(m):(int)Math.Ceiling(m);
                 if (m < 0 && m > -1)
                 {
                     n = 0;
                 }
                 else
                 {
                     n = (int)Math.Floor(m);
                 }
                 string preStr = (d / Math.Pow(10, 3 * n)).ToString();
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
                     case -1:
                         return preStr + "m";
                     case -2:
                         return preStr + "u";
                     case -3:
                         return preStr + "n";
                     case -4:
                         return preStr + "p";
                     default:
                         return value;
                 }
             }
            catch
            {
                return value;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double d;
                string str = value.ToString();
                if (double.TryParse(str, out d))
                    return d;
                else
                {
                    if (str.EndsWith("K") || str.EndsWith("k"))
                    {
                        string temp = str.TrimEnd('K', 'k');
                        if (double.TryParse(temp, out d))
                        {
                            return d * 1e3;
                        }
                    }
                    if (str.EndsWith("G") || str.EndsWith("g"))
                    {
                        string temp = str.TrimEnd('G', 'g');
                        if (double.TryParse(temp, out d))
                        {
                            return d * 1e9;
                        }
                    }
                    if (str.EndsWith("M"))
                    {
                        string temp = str.TrimEnd('M');
                        if (double.TryParse(temp, out d))
                        {
                            return d * 1e6;
                        }
                    }
                    if (str.EndsWith("m"))
                    {
                        string temp = str.TrimEnd('m');
                        if (double.TryParse(temp, out d))
                        {
                            return d * 1e-3;
                        }
                    }
                    if (str.EndsWith("u"))
                    {
                        string temp = str.TrimEnd('u');
                        if (double.TryParse(temp, out d))
                        {
                            return d * 1e-6;
                        }
                    }
                    if (str.EndsWith("n"))
                    {
                        string temp = str.TrimEnd('n');
                        if (double.TryParse(temp, out d))
                        {
                            return d * 1e-9;
                        }
                    }
                    if (str.EndsWith("p"))
                    {
                        string temp = str.TrimEnd('p');
                        if (double.TryParse(temp, out d))
                        {
                            return d * 1e-12;
                        }
                    }
                    return value;
                }
            }
            catch
            {
                return value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
