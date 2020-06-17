using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    /// <summary>
    /// 测量类所需仪表的信息，一个测量类中会用到一个到几个仪表驱动的接口,
    /// 每一个仪表驱动对应一个仪表地址
    /// </summary>
    public class MeasClsInstruInfo
    {
        public string MeasClsName { get; set; }
        public string[] InstruTypeNameList { get; set; }
    }
    public class InstruInfo
    {
        public string TypeName { get; set; }
        public string Address { get; set; }
    }
}
