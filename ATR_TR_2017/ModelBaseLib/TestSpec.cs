using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Symtant.GeneFunLib;
namespace ModelBaseLib
{
    /// <summary>
    /// spec指被测件的一套指标，包含定义的marker和门限
    /// </summary>
    /// 
    [Serializable]
    public class TestTraceSpec
    {
        public TestTraceSpec()
        {
            TestMarkerList = new List<XYTestMarker>();
            TestLimit = new XYTestLimit();
        }
        public List<XYTestMarker> TestMarkerList { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public bool? PassFail { get; set; }
        public XYTestLimit TestLimit { get; set; }
        public double? UpLimit { get; set; }
        public double? LowLimit { get; set; }
        public string LimitDescription { get; set; }
    }
    
    public class XYTestLimit
    {
        
        public bool Enable { get; set; }
        
        public XYTestLimit()
        {
            LimitLine = new ObservableCollection<LimitLine>();
        }
        public ObservableCollection<LimitLine> LimitLine { get; set; }
    }
}
