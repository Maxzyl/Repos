using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestResultMarkerDip
{
    public class TestPowerResult : NotifyBase
    {
        private double _Power1;
        private const string Power1PropertyName = "Power1";
        public double Power1
        {
            get
            {
                return _Power1;
            }
            set
            {
                _Power1 = value;
                NotifyPropertyChanged(Power1PropertyName);
            }
        }
        private double _Power2;
        private const string Power2PropertyName = "Power2";
        public double Power2
        {
            get
            {
                return _Power2;
            }
            set
            {
                _Power2 = value;
                NotifyPropertyChanged(Power2PropertyName);
            }
        }
    }
}
