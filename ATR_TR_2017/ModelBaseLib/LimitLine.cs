using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    public class LimitLine
    {
        public LimitLine()
        {
            Type = LimitLineTypeEnum.None;
            X1 = 0;
            X2 = 0;
            Y1 = 0;
            Y2 = 0;
        }

        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }
        public LimitLineTypeEnum Type { get; set; }
    }

    public enum LimitLineTypeEnum
    {
        None, Max, Min
    }
}
