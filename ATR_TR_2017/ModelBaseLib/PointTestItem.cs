using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    public class PointTestItem : TestItem
    {
        public PointTestItem()
        {
            TestSpecList = new List<PointTestSpec>();
        }
        public double X { get; set; }
        public double Y { get; set; }
        public List<PointTestSpec> TestSpecList { get; set; }
        public void CalcInfo()
        {

            PassFail = null;
            Y = Y + Compensation;
            foreach (var spec in TestSpecList)
            {
                spec.PassFail = Symtant.GeneFunLib.GeneFun.IsPassFail(X, spec.Limit.XMax, spec.Limit.XMin) &&
                Symtant.GeneFunLib.GeneFun.IsPassFail(Y, spec.Limit.YMax, spec.Limit.YMin);
                PassFail = Symtant.GeneFunLib.GeneFun.NullBoolAnd(PassFail, spec.PassFail);
            }
        }
    }
    public class PointTestSpec
    {
        public PointTestSpec()
        {
            Limit = new XYMakerLimit();
        }
        [System.Xml.Serialization.XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool? PassFail { get; set; }
        public XYMakerLimit Limit { get; set; }
        public string LimitDescription { get; set; }
    }
}
