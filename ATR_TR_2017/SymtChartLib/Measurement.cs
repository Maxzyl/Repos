using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymtChartLib
{
    public class Measurement
    {
        public string TypeName { get; set; }
        public MeasFormatEnum Format { get; set; }
    }
    public enum MeasFormatEnum { LogMag, Phase, Delay }
}
