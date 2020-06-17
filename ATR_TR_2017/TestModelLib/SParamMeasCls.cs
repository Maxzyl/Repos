using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symtant.InstruDriver.NetworkAnalyzer;
using ModelBaseLib;
using System.Text.RegularExpressions;
namespace TestModelLib
{
    public class SParamMeasCls
    {
        public SParamMeasCls(int portCount)
        {
            _PortCount = portCount;
            TestPortsPower = new double[portCount];
        }
        private int _PortCount;
        public int PortCount { get { return _PortCount; } }
        public double StartFreq { get; set; }
        public double StopFreq { get; set; }
        public int SweepPoints { get; set; }
        public double[] TestPortsPower { get; set; }
        public bool IsPortsPowerCouple { get; set; }
        public double IFBW { get; set; }
        public NASweepTypeEnum SweepType { get; set; }
        public void Setup()
        {
            
        }
    }
    public class SParamCalHelper
    {
        private KtPNA0980 na;
        private int ChannelNum;
        public SParamCalHelper(int portCount,KtPNA0980 pna,int chNum)
        {
            _PortCount = portCount;
            CalPorts = new bool[portCount];
            PortsConnectorType = new string[portCount];
            PortsCalKit = new string[portCount];
            na = pna;
            ChannelNum = chNum;
            PortsName = new string[portCount];
        }
        private int _PortCount;
        public int PortCount { get { return _PortCount; } }
        public bool[] CalPorts { get; set; }
        
        public SParamCalTypeEnum CalType { get; set; }
        public string[] PortsConnectorType { get; set; }
        public string[] PortsCalKit { get; set; }
        public string[] PortsName { get; set; }
        public RFCalStepInfo[] GetPathCalSteps()
        {
            RFCalStepInfo[] rawStepInfos = GetCalSteps();
            foreach (var rfCalStep in rawStepInfos)
            {
                for (int i = 0; i < PortsName.Count(); i++)
                {
                    if (!string.IsNullOrWhiteSpace(PortsName[i]))
                    {
                        var tt = rfCalStep.ConnectorPairList.Where(x => x.Port1Name == i.ToString()).FirstOrDefault();
                        if (tt != null)
                        {
                            tt.Port1Name = PortsName[i];
                        }
                    }
                }
            }
            return rawStepInfos;
        }
        public RFCalStepInfo[] GetCalSteps()
        {
            for (int i=0;i<PortCount;i++)
            {
                string portConnType=PortsConnectorType[i];
                if (portConnType == null)
                {
                    na.SetPortConnectorType(ChannelNum, i + 1);
                }
                else
                {
                    na.SetPortConnectorType(ChannelNum, i + 1, portConnType);
                    na.SetCalKitType(ChannelNum, i + 1, PortsCalKit[i]);
                }
            }
            na.InitCal(ChannelNum);
            int stepCount = na.GetCalStepCount(ChannelNum);
            RFCalStepInfo[] stepInfoList = new RFCalStepInfo[stepCount];
            List<string> stepDescList = new List<string>();
            for (int i = 0; i < stepCount; i++)
            {
                string res = na.GetCalStepDescription(ChannelNum, i+1);
                ConnectorPair[] cplist= GetRFConnList(res);
                stepInfoList[i] = new RFCalStepInfo();
                stepInfoList[i].ConnectorPairList = cplist;
                stepInfoList[i].StepAction = () =>
                    {
                        na.AcquireStep(ChannelNum, i + 1);
                    };
            }
            na.AbortGuidedCal(ChannelNum);
            return stepInfoList;
        }
        
        private ConnectorPair[] GetRFConnList(string msg)
        {
            ConnectorPair[] cpList = null;
            string msga = msg.ToLower();
            string openPatten = @"open to port (\d+)";
            string shortPatten = @"short to port (\d+)";
            string loadPatten = @"load to port (\d+)";
            string thruPatten1=@"between port (\d+) and port (\d+)";
            string thruPatten2 = @"port (\d+) to port (\d+)";
            Match m = Regex.Match(msga, openPatten);
            if (m.Success)
            {
                cpList = new ConnectorPair[1];
                cpList[0] = new ConnectorPair();
                cpList[0].Port1Name = m.Groups[1].Value.ToString();
                cpList[0].Port2Name = CalStdDef.Open;
                return cpList;
            }
            m = Regex.Match(msga, shortPatten);
            if (m.Success)
            {
                cpList = new ConnectorPair[1];
                cpList[0] = new ConnectorPair();
                cpList[0].Port1Name = m.Groups[1].Value.ToString();
                cpList[0].Port2Name = CalStdDef.Short;
                return cpList;
            }
            m = Regex.Match(msga, loadPatten);
            if (m.Success)
            {
                cpList = new ConnectorPair[1];
                cpList[0] = new ConnectorPair();
                cpList[0].Port1Name = m.Groups[1].Value.ToString();
                cpList[0].Port2Name = CalStdDef.Load;
                return cpList;
            }
            m = Regex.Match(msga, thruPatten1);
            if (m.Success)
            {
                cpList = new ConnectorPair[2];
                cpList[0] = new ConnectorPair();
                cpList[0].Port1Name = m.Groups[1].Value.ToString();
                cpList[0].Port2Name = CalStdDef.Thru;
                cpList[1] = new ConnectorPair();
                cpList[1].Port1Name = m.Groups[2].Value.ToString();
                cpList[1].Port2Name = CalStdDef.Thru;
                return cpList;
            }
            m = Regex.Match(msga, thruPatten2);
            if (m.Success)
            {
                cpList = new ConnectorPair[1];
                cpList[0] = new ConnectorPair();
                cpList[0].Port1Name = m.Groups[1].Value.ToString();
                cpList[0].Port2Name = m.Groups[2].Value.ToString();
                return cpList;
            }
            return null;
        }
    }
    public enum SParamCalTypeEnum { FullNPort, OnePort, Resp1, Resp2, EnhResp1, EnhResp2 }
    public class CalStdDef
    {
        public const string Power = "Power";
        public const string Open = "Open";
        public const string Short = "Short";
        public const string Load = "Load";
        public const string Thru = "Thru";
        public const string Ecal = "Ecal";
    }
    public class RFCalStepInfo:CalStepInfo
    {
        public ConnectorPair[] ConnectorPairList { get; set; }
        
        public string GetGuideMsg()
        {
            string res=null;
            if (ConnectorPairList == null)
                return null;
            else
            {
                foreach (var cp in ConnectorPairList)
                {
                    res += "connect ";
                    res += cp.Port1Name;
                    res += " to ";
                    res += cp.Port2Name;
                    res += " ";
                }
                return res;
            }
        }
        private string _StepMsg;
        public override string StepMsg
        {
            get
            {
                return GetGuideMsg();
            }
            set
            {
                _StepMsg = value;
            }
        }
    }
}
