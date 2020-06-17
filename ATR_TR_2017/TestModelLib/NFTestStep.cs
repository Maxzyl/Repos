using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using Symtant.GeneFunLib;
using Symtant.InstruDriver.NFAnalyzer;
using Symtant.InstruDriver;
namespace TestModelLib
{

    public enum DUTTypeEnum { 放大器 }
    
    public class NFTestStep:TestStep

    {
        public NFTestStep()
        {
            CorrData = new NFCorrData();
            StartFreq = 1e9;
            StopFreq = 2e9;
            SweepPoints = 11;
            MeasBandwidth = 1e6;
        }
        private INFAnalyzer nfa
        {
            get
            {
                return MeasInfo.InstruInfoList[0].InstruDriver as INFAnalyzer;
            }
        }

        public string DUTType { get; set; }

        
        public double StartFreq { get; set; }
        
        public double StopFreq { get; set; }
        
        public int SweepPoints { get; set; }
        
        public double MeasBandwidth { get; set; }
        
        public bool AverageEnable { get; set; }
        
        public int AverageCount { get; set; }
        //public override string[] GetTableRow()
        //{
        //    List<string> temp = new List<string>();
        //    temp.Add(StartFreq.ToString());
        //    temp.Add(StopFreq.ToString());
        //    temp.Add(SweepPoints.ToString());
        //    return temp.ToArray();
        //}
        private static string CurrentStateFile;
        public override void Single()
        {
            
            if (GeneTestSetup.Instance.IsSimulated)
            {
                foreach (TestTrace tr in ItemList)
                {
                    tr.ResultData.Clear();

                    double[] freqList = GeneFun.GenerateIndexedArray(StartFreq, StopFreq, SweepPoints);
                    foreach (var freq in freqList)
                    {
                        tr.ResultData.Add(new XYData { X = freq, Y = GeneFun.GetRand(1, 2) });
                    }
                    tr.XAxisInfo.Start = StartFreq.ToRFFreqStr();
                    tr.XAxisInfo.Stop = StopFreq.ToRFFreqStr();
                    tr.XAxisInfo.Unit = "Hz";
                    tr.XAxisInfo.Center = MeasBandwidth.ToString();
                }

            }
            else
            {
                if (CorrectionEnable && (CorrData.StateFile != null))
                {
                    //nfa.ResetInterface();
                    //(nfa as InstruDriver).Wait(10000);
                    //nfa.SendFileToInstru(CorrData.StateFile, "symt");
                    //(nfa as InstruDriver).Wait(10000);
                    //如果当前加载的状态文件没有变化，那么不重新加载
                    if (CurrentStateFile != "symt" + IndexInSeq)
                    {
                        nfa.RecallState("symt" + IndexInSeq);
                        (nfa as InstruDriver).Wait(10000);
                        CurrentStateFile = "symt" + IndexInSeq;
                    }
                    
                }
                else//without correction set nfa with settings
                {
                    InitSettings();
                }
                //load correction
                nfa.Init();
                (nfa as Symtant.InstruDriver.InstruDriver).Wait(SweepPoints * 1000);
                double[] nfRes;
                if (CorrectionEnable)
                {
                    nfRes = nfa.GetCorrNF();
                }
                else
                {
                    nfRes = nfa.GetUnCorrNF();
                }
                
                //add user correction to test result

                foreach (TestTrace tr in ItemList)
                {
                    tr.ResultData.Clear();

                    double[] freqList = GeneFun.GenerateIndexedArray(StartFreq, StopFreq, SweepPoints);
                    if (freqList.Count() == nfRes.Count())
                    {
                        for (int i = 0; i < freqList.Count(); i++)
                        {
                            tr.ResultData.Add(new XYData { X = freqList[i], Y = nfRes[i] });
                        }
                        
                    }
                    else
                    {
                        throw (new Exception("wrong nf test result"));
                    }
                    

                }
            }
        }
        public override void InitOnce()
        {
            base.InitOnce();
            //在加载测试方案的时候，把所有测试步骤的状态都写入仪表，IndexInSeq是测试步骤在整个测试序列中的序号
            if (CorrectionEnable && (CorrData.StateFile != null))
            {
                nfa.ResetInterface();
                (nfa as InstruDriver).Wait(10000);
                nfa.SendFileToInstru(CorrData.StateFile, "symt"+IndexInSeq);
                (nfa as InstruDriver).Wait(10000);
                CurrentStateFile = null;
                //nfa.RecallState("symt");
                //(nfa as InstruDriver).Wait(10000);
            }
            
        }
        //public override TestTrace GetDefaultTestTrace()
        //{
        //    return new TestTrace();
        //}
        public override void CreateTrace(string traceTypeName)
        {
            ItemList.Add(new TestTrace() { TypeName = traceTypeName });
        }
        [System.Xml.Serialization.XmlIgnore]
        public NFCorrData CorrData { get; set; }
        public override object GetLocalSetting()
        {
            return CorrData;
        }
        public override void SetLocalSetting(object v)
        {
            CorrData = v as NFCorrData;
        }
        public override void InitCal()
        {
            CalStepList.Clear();
            CalStepList.Add(new CalStepInfo() { StepMsg = NFACalStepMsgType.MSG1});
        }
        private void InitSettings()
        {
            nfa.ResetInterface();
            (nfa as InstruDriver).Wait(3000);
            nfa.SweepMode = NFAnalyzerSweepModeEnum.Sweep;
            nfa.StartFreq = StartFreq;
            nfa.StopFreq = StopFreq;
            nfa.SweepPoints = SweepPoints;
            nfa.MeasBandwidth = MeasBandwidth;

            nfa.AverageEnable = AverageEnable;
            if (AverageEnable)
            {
                nfa.AverageNum = AverageCount;
            }
            nfa.InitContEnable = false;
            (nfa as InstruDriver).Wait(1000);
        }
        public override void AcquireStep(CalStepInfo stepInfo)
        {
            switch (stepInfo.StepMsg)
            {
                case NFACalStepMsgType.MSG1:
                    CalNFA();
                    break;
                default:
                    break;
            }
        }
        public void CalNFA()
        {
            InitSettings();
            nfa.Calibrate();
            (nfa as InstruDriver).Wait(SweepPoints * 4000);
            string fileName=nfa.SaveState("symt");
            (nfa as InstruDriver).Wait(10000);
            byte[] stateFile = nfa.GetFileFromInstru(fileName);
            CorrData.StateFile = stateFile;
        }
        MeasClsInfo[] _SupportedMeasClsInfoList;
        public override MeasClsInfo[] SupportedMeasClsInfoList
        {
            get
            {
                if (_SupportedMeasClsInfoList == null)
                {
                    _SupportedMeasClsInfoList = 
                        new MeasClsInfo[]{
                            new MeasClsInfo()
                            {
                                TypeName="nfa",
                                DisplayName="噪声系数分析仪",
                                InstruInfoList=new InstruInfo[]
                                {
                                    new InstruInfo()
                                    {
                                        Name="nfa",
                                        DisplayName="噪声系数分析仪",
                                    }

                                },
                                IsNeedCal=true
                            }
                        };
                }
                return _SupportedMeasClsInfoList;
            }
        }
    }
    public class NFTestTraceType
    {
        public const string NF = "NF";
        public const string Gain = "Gain";
    }
    public class NFCorrData
    {
        public NFCorrData()
        {
            LossTableBeforeDut = new XYDataList();
            LossTableAfterDut = new XYDataList();
        }

        public double LossBeforeDut { get; set; }
        public double LossAfterDut { get; set; }
        public XYDataList LossTableBeforeDut { get; set; }
        public XYDataList LossTableAfterDut { get; set; }
        public byte[] StateFile { get; set; }
        public bool LossTableBeforeDutEnable { get; set; }
        public bool LossTableAfterDutEnable { get; set; }
    }

    public class XYDataList
    {
        public XYDataList()
        {
            lstXYData = new ObservableCollection<xyData>();
        }
        public ObservableCollection<xyData> lstXYData { get; set; }
    }

    public class xyData
    {
        public xyData()
        {
            X = 0;
            Y = 0;
        }
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class NFACalStepMsgType
    {
        public const string MSG1 = "连接噪声源到噪声系数分析仪";
    }
}
