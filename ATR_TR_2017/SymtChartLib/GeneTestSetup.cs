using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymtChartLib
{
    public class GeneTestSetup:NotifyBase
    {
        private static GeneTestSetup instance = new GeneTestSetup();
        public static GeneTestSetup Instance
        {
            get
            {
                return instance;
            }
        }
        private int _DataDisplayDigits;
        private const string DataDisplayDigitsPropertyName = "DataDisplayDigits";
        public int DataDisplayDigits
        {
            get
            {
                return _DataDisplayDigits;
            }
            set
            {
                _DataDisplayDigits = value;
                NotifyPropertyChanged(DataDisplayDigitsPropertyName);
            }
        }
    }
}
