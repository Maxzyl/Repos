using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
namespace TestUI_New
{
    public class ListIntConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is List<int>)
            {
                return AnalysisListInt(value as List<int>);
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = value as string;
            if (str == null)
            {
                return value;
            }
            else
            {
                return AnalysisString(str);
            }
        }
        private List<int> AnalysisString(string s)
        {
            int temp1, temp2, temp3;
            List<int> res = new List<int>();
            string[] strs = s.Split(new char[] { ',' });
            foreach (string str in strs)
            {
                string[] substrs = str.Split(new char[] { ':' });
                if (substrs.Count() == 1)
                {
                    if (int.TryParse(substrs[0], out temp1))
                    {
                        res.Add(temp1);
                    }

                }
                if (substrs.Count() == 2)
                {
                    if (int.TryParse(substrs[0], out temp1) && int.TryParse(substrs[1], out temp2))
                    {
                        for (int i = temp1; i <= temp2; i++)
                        {
                            res.Add(i);
                        }
                    }
                }
                if (substrs.Count() == 3)
                {
                    if (int.TryParse(substrs[0], out temp1) && int.TryParse(substrs[1], out temp2) && int.TryParse(substrs[2], out temp3))
                    {
                        for (int i = temp1; i <= temp3; i += temp2)
                        {
                            res.Add(i);
                        }
                    }
                }
            }
            return res;
        }
        private string AnalysisListInt(List<int> intarray)
        {
            int len=intarray.Count;
            string str="";
            if (intarray == null)
            {
                return null;
            }
            else
            {
                if (intarray.Count > 2)
                {
                    int start = 0;
                    while(start<len)
                    {
                        if (start < len - 2)
                        {
                            int delta = intarray[start + 1] - intarray[start];
                            if ((intarray[start + 2] - intarray[start + 1]) == delta)
                            {
                                for (int i = start; i < len - 2; i++)
                                {
                                    if ((intarray[i + 2] - intarray[i + 1]) == delta)
                                    {
                                        if (i == len - 3)
                                        {
                                            str += intarray[start];
                                            str += ":";
                                            str += delta;
                                            str += ":";
                                            str += intarray[i + 2];
                                            return str;
                                        }
                                    }
                                    else
                                    {
                                        str += intarray[start];
                                        str += ":";
                                        str += delta;
                                        str += ":";
                                        str += intarray[i + 1];
                                        str += ",";
                                        start = i + 2;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                str += intarray[start];
                                str += ",";
                                start += 1;
                            }
                        }
                        else
                        {
                            if (start == len - 1)
                            {
                                str += intarray[start];
                                
                                start += 1;
                            }
                            else
                            {
                                str += intarray[start];
                                str += ",";
                                start += 1;
                            }
                        }
                    }
                    return str;
                }
                else
                {
                    switch (intarray.Count)
                    {
                        case 0:
                            return "";
                        case 1:
                            return intarray[0].ToString();
                        case 2:
                            return intarray[0].ToString() + "," + intarray[1].ToString();
                        default:
                            return null;
                    }
                }
            }

        }
    }
}
