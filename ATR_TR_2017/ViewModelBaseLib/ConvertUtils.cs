using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ViewModelBaseLib
{
    public class TableRowColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool rowColor = (bool)value;
            //SolidColorBrush scb1 = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            //SolidColorBrush scb2 = new SolidColorBrush(Color.FromRgb(241, 252, 255));
            SolidColorBrush scb1 =new SolidColorBrush( System.Windows.Media.Colors.White);

            SolidColorBrush scb2 = new SolidColorBrush(System.Windows.Media.Colors.LightGray);
            if (rowColor)
            {
                return scb1;
            }
            else
            {
                return scb2;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
