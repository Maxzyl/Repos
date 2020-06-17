using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using Symtant.GeneFunLib;
namespace TestModelLib
{
    public class SParamTestStep:TestStep
    {
        public SParamTestStep()
        {
            StartFreq = 1e9;
            StopFreq = 2e9;
            SweepPoints = 11;
            PortNum = 2;
        }
        //ZYL add below method to test
        public override void CreateTrace(string traceTypeName)
        {
            this.ItemList.Add(new SParamTestTrace() { TypeName = traceTypeName });
        }
        [UIDisplayPara("起始频率")]
        public double StartFreq { get; set; }
        [UIDisplayPara("截止频率")]
        public double StopFreq { get; set; }
        public int SweepPoints { get; set; }
        public int PortNum { get; set; }
        public override void InitOnce()
        {
            
        }
        public override void Single()
        {
            if (GeneTestSetup.Instance.IsSimulated)
            {
                GeneSimulatedData();
            }
            else
            {

            }
        }
        public override void GeneSimulatedData()
        {
            foreach (SParamTestTrace tr in ItemList)
            {
                tr.ResultData.Clear();
                double[] freqList = GeneFun.GenerateIndexedArray(StartFreq, StopFreq, SweepPoints);
                foreach (var freq in freqList)
                {
                    tr.ResultData.Add(new XYData { X = freq, Y = GeneFun.GetRand(-1, 1) });
                }
            }
        }
        public override string[] ItemTypeNameList
        {
            get
            {
                List<string> nameList = new List<string>();
                for (int i = 1; i <= PortNum; i++)
                {
                    for (int j = 1; j <= PortNum; j++)
                    {
                        nameList.Add("S" + i + j);
                    }
                }
                return nameList.ToArray();
            }
        }

    }
    public enum TwoPortCalTypeEnum { TwoPort, OnePort1, OnePort2, Resp, EnhancedResp1 }
    public class SParamTestTrace : TestTrace
    {
        public SParamTestTrace()
        {
            TypeName = SParamTestTraceType.S11;
        }
        public string Format { get; set; }
    }
    public class SParamTestTraceType
    {
        public const string S11 = "S11";
        public const string S21 = "S21";
        public const string S12 = "S12";
        public const string S22 = "S22";
    }
    public class SParamTestTraceFormatType
    {
        public const string LogMag = "LogMag";
        public const string phase = "phase";
        public const string SWR = "SWR";
    }
}
