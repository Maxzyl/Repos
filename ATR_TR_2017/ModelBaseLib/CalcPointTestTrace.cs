using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
namespace ModelBaseLib
{
    public class CalcPointTestTrace:TestTrace
    {
        [UIDisplay("公式")]
        public string Formula { get; set; }
    }
}
