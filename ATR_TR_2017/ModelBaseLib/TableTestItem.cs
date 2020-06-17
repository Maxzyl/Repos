using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    public class TableTestItem:TestItem
    {

    }
    public class TableTestItemSpec
    {
        public bool? PassFail { get; set; }
        public XYTestLimit TestLimit { get; set; }
        public string UpLimit { get; set; }
        public string LowLimit { get; set; }
        public string LimitDescription { get; set; }
        public string XColumnName { get; set; }
    }
}
