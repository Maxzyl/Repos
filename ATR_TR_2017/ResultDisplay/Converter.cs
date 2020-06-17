using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Symtant.GeneFunLib;

namespace TestResultMarkerDip
{
   
    public class ListViewItemSourceConverter : MarkupExtension, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] as ObservableCollection<ObservableCollection<XYMarkerDisp>> != null)
            {
                ObservableCollection<ObservableCollection<XYMarkerDisp>> XYMarkerDispList = values[0] as ObservableCollection<ObservableCollection<XYMarkerDisp>>;
                int index = System.Convert.ToInt32(values[1]);
                if (index == -1)
                {
                    index = 0;
                }
                return XYMarkerDispList[index];
            }
            else
            {
                return null;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class TestResultConverter2 : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                bool v = (bool)value;
                return v ? "PASS" : "FAIL";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class PassFailConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = System.Convert.ToString(value);
            if (str == "True")
            {
                return new SolidColorBrush(Colors.Green);
            }
            else if (str == "False")
            {
                return new SolidColorBrush(Colors.Red);
            }
            else
            {
                return new SolidColorBrush(Colors.Black);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class DataDisplayDigitalConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {   
            string str = value.ToString();
            double? a = str.ToNullDouble();
            if (a != null)
            {
                return System.Convert.ToDouble(a).ToDigits(GeneTestSetup.Instance.DataDisplayDigits);
            }
            else
            {
                return "";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
