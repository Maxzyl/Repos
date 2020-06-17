using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    //move icalable interface to teststep to make structure more simple
    public interface ICalable
    {
        CalInfo CalInfo { get; set; }
        void InitCal();
        void AcquireStep(CalStepInfo stepInfo);
        void FinishCal();
        List<CalStepInfo> CalStepList { get; set; }

        bool CorrectionEnable { get; set; }
        int CalInterval { get; set; }
        CalWarningTypeEnum CalWarning { get; set; }
        bool IsCalEachTest { get; set; }
    }
    public class CalStepInfo
    {
        public virtual string StepMsg { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public Action StepAction { get; set; }
    }
    public class CalInfo
    {
        

    }
    public class CorrectionData
    {

    }
    public enum CalWarningTypeEnum{强制校准,提醒校准,不提醒};

    public class ConnectorPair
    {
        public string Port1Name { get; set; }
        public string Port2Name { get; set; }
        //public string ConcatStr
        //{
        //    get
        //    {
        //        return string.Compare(Port1Name,Port2Name)<0 ? Port1Name + Port2Name : Port2Name + Port1Name;
        //    }
        //}
        //public override int GetHashCode()
        //{
        //    return ConcatStr.GetHashCode();
            
        //}
        //public override bool Equals(object obj)
        //{
        //    if (obj is RFConnectorPair)
        //    {
        //        RFConnectorPair cp = obj as RFConnectorPair;
        //        return cp.ConcatStr == ConcatStr;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //public static bool operator ==(RFConnectorPair cp1, RFConnectorPair cp2)
        //{
        //    return cp1.ConcatStr == cp2.ConcatStr;
        //}
        //public static bool operator !=(RFConnectorPair cp1, RFConnectorPair cp2)
        //{
        //    return cp1.ConcatStr != cp2.ConcatStr;
        //}
    }
    public class TestPath
    {
        public string Name { get; set; }
        public ConnectorPair[] ConnectorPairList { get; set; }
    }
    public interface IMoveNext
    {
        void MoveNext();
    }
}
