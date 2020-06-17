using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    public class CalcPointTestItem:PointTestItem
    {
        [UIDisplay("公式")]
        public string Formula { get; set; }
    }
}
