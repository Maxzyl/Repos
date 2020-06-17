using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace ATS_TR
{
        public class StatusBarStaticItem : MarkupExtension, IValueConverter
        {

            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                if (value == null) return null;
                string str = "";
                switch (System.Convert.ToString(parameter))
                {
                    case "Project":
                        str = "已加载测试方案: {0}";
                        break;
                    case "Eqp":
                        str = "连接状态: {0}";
                        break;
                    case "Cal":
                        str = "校准状态: {0}";
                        break;
                    case "User":
                        str = "用户: {0}";
                        break;
                    default:
                        str = "{0}";
                        break;
                }
                return string.Format(str, value);
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return value;
            }

            public override object ProvideValue(IServiceProvider serviceProvider)
            {
                return this;
            }
        }
}
