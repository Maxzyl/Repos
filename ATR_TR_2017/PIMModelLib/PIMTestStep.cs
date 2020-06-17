using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;

using Symtant.InstruDriver.PIMTester;
using Symtant.InstruDriver;
using Symtant.GeneFunLib;
namespace PIMModelLib
{
    

    public enum PIMSweepToneEnum { Tone1, Tone2 }
    //public enum PIMMeasMode { Point, Sweep }
    public enum PIMMeasMode { TRAN, REFL }
    public enum PIMSidebandTypeEnum { Low, High, Dual }
    public enum PIMCalType { Point, Sweep, TimeDomain }
    public enum ResultUnit { dBm, dBc }
    public enum Port { Port1, Port2 }

    public class Range
    {
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
    }
    public class PIMTestStep : TestStep
    {
        public PIMTestStep()
        {
            FreqRange = "";
            CWFreq1 = 0;
            CWFreq2 = 0;
            CWPOW1 = 43;
            CWPOW2 = 43;
            TestStepper = 1000000;
            TestTime = 10;
            StartFreq1 = 0;
            StopFreq1 = 0;
            StartFreq2 = 0;
            StopFreq2 = 0;
            PIMOrder = 3;
            SideBand = PIMSidebandTypeEnum.Low;
            CalType = PIMCalType.Point;
            isEnable = true;
            PortName = Port.Port1;
        }

        [System.Xml.Serialization.XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public IPIMTester PIMTester
        {
            get
            {
                return MeasInfo.InstruInfoList[0].InstruDriver as IPIMTester;
            }
        }
        
        public string FreqRange { get; set; }
        public double TXFreq1 { get; set; }
        public double TXFreq2 { get; set; }
        public double RXFreq1 { get; set; }
        public double RXFreq2 { get; set; }

        public double CWFreq1 { get; set; }
        public double CWFreq2 { get; set; }
        public double CWPOW1 { get; set; }
        public double CWPOW2 { get; set; }

        public double StartFreq1 { get; set; }
        public double StopFreq1 { get; set; }

        public double StartFreq2 { get; set; }
        public double StopFreq2 { get; set; }

        public double TestStepper { get; set; }//步进
        public double TestTime { get; set; }//测试时间-秒

        private string SweepFlag { get; set; }
        public int PIMOrder { get; set; }

        public double GainOffset { get; set; }//PM——GainOffset

        public PIMMeasMode MeasMode { get; set; }
        public PIMSidebandTypeEnum SideBand { get; set; }
        public PIMSweepToneEnum SweepTone { get; set; }
        public PIMCalType CalType { get; set; }
        public ResultUnit ResultUnit { get; set; }
        public Port PortName { get; set; }

        

        public double CalPowTolerance { get; set; }
        public DateTime CalTime { get; set; }
        public int CalInterval { get; set; }
        public bool CalRestrict { get; set; }

        public Range ReceiverRng { get; set; }
        public Range SourceRng { get; set; }
        public bool isEnable { get; set; }
        public bool isImageSave { get; set; }
        /// <summary>
        /// 测试结果放在这里
        /// </summary>
        public XYDataArr PIMTestResult1 { get; set; }
        public XYDataArr PIMTestResult2 { get; set; }

        public string SingleDBTime { get; set; }

        public double PIMPower1 { get; set; }
        public double PIMPower2 { get; set; }

        public PIMMeasCls MeasObj { get; set; }
        private int points1
        {
            get
            {
                return (int)Math.Round((StopFreq1 - StartFreq1) / TestStepper + 1);
            }
        }
        private int points2
        {
            get
            {
                return (int)Math.Round((StopFreq2 - StartFreq2) / TestStepper + 1);
            }
        }
        public override void InitOnce()
        {
            base.InitOnce();
        }
        public override void Single()
        {
            foreach (PIMTestTrace tr in ItemList)
            {
                (tr.TestResult as XYTraceResult).ResultData.Clear();
            }

            if (GeneTestSetup.Instance.IsSimulated)
            {

                switch (CalType)
                {
                    case PIMCalType.Point:
                        foreach (PIMTestTrace tr in ItemList)
                        {
                            (tr.TestResult as XYTraceResult).ResultData.Clear();
                            if (tr.TypeName == PIMTestTraceType.PIMTrace1)
                            {
                                (tr.TestResult as XYTraceResult).ResultData.Add(new XYData { X = 0, Y = GeneFun.GetRand(-140, -100) });
                            }
                            if (tr.TypeName == PIMTestTraceType.PIMTrace2)
                            {
                                (tr.TestResult as XYTraceResult).ResultData.Add(new XYData { X = 0, Y = GeneFun.GetRand(-140, -100) });
                            }
                        }

                        PIMPower1 = GeneFun.GetRand(42, 44);
                        PIMPower2 = GeneFun.GetRand(42, 44);
                        break;
                    case PIMCalType.Sweep:
                        foreach (PIMTestTrace tr in ItemList)
                        {
                            (tr.TestResult as XYTraceResult).ResultData.Clear();
                            if (tr.SweepSource == PIMSweepSourceEnum.F1)
                            {
                                double[] freqList = GeneFun.GenerateIndexedArray(StartFreq1, StopFreq1,points1);
                                foreach (var freq in freqList)
                                {
                                    (tr.TestResult as XYTraceResult).ResultData.Add(new XYData { X = freq, Y = GeneFun.GetRand(-140, -100) });
                                }
                            }
                            if (tr.SweepSource == PIMSweepSourceEnum.F2)
                            {
                                double[] freqList = GeneFun.GenerateIndexedArray(StartFreq2, StopFreq2, points2);
                                foreach (var freq in freqList)
                                {
                                    (tr.TestResult as XYTraceResult).ResultData.Add(new XYData { X = freq, Y = GeneFun.GetRand(-140, -100) });
                                }
                            }
                        }

                        PIMPower1 = GeneFun.GetRand(42, 44);
                        PIMPower2 = GeneFun.GetRand(42, 44);
                        break;
                    case PIMCalType.TimeDomain:
                        int num = PIMOrder / 2;
                    double dblIMF = (((num + 1) * CWFreq1) - (num * CWFreq2));

                        foreach (PIMTestTrace tr in ItemList)
                        {
                            (tr.TestResult as XYTraceResult).ResultData.Clear();
                            if (tr.SweepSource == PIMSweepSourceEnum.F1)
                            {
                                for (int i = 0; i < 50; i++ )
                                {
                                    (tr.TestResult as XYTraceResult).ResultData.Add(new XYData { X = i, Y = GeneFun.GetRand(-140, -100) });
                                }
                            }
                            if (tr.SweepSource == PIMSweepSourceEnum.F2)
                            {
                                for (int i = 0; i < 50; i++)
                                {
                                    (tr.TestResult as XYTraceResult).ResultData.Add(new XYData { X = i, Y = GeneFun.GetRand(-140, -100) });
                                }
                            }
                        }

                        PIMPower1 = GeneFun.GetRand(42, 44);
                        PIMPower2 = GeneFun.GetRand(42, 44);
                        break;
                    default:
                        break;
                }
                
            }
            else
            {
                //setting to instru
                MeasInfo.InstruInfoList[0].InstruDriver.Reset();

                PIMTester.setFreq(CWFreq1, CWFreq2);

                PIMTester.setPow(CWPOW1, CWPOW2);

                PIMTester.setMeasMode(MeasMode.ToString());

                PIMTester.setUnit(ResultUnit.ToString());

                PIMTester.setIMOrder(PIMOrder);

                PIMTester.setPort(PortName.ToString());

                if (CalType == PIMCalType.Point)
                {
                    int num = PIMOrder / 2;
                    double dblIMF = ((num + 1) * CWFreq1) - (num * CWFreq2);

                    PIMTester.setModeStandard(dblIMF);

                    PIMTester.setPowOn(true, true);

                    List<XYData> lstRes = new List<XYData>();

                    double[] strRes = new double[3];
                    if (ResultUnit == ResultUnit.dBm)
                    {
                        strRes = PIMTester.getPointIMPow(0);
                    }
                    else
                    {
                        strRes = PIMTester.getPointIMPow(CWPOW1);
                    }

                    PIMPower1 = strRes[0];
                    PIMPower2 = strRes[1];

                    //XYData xyData = new XYData();
                    ////赋值
                    //xyData.X = dblIMF;
                    //xyData.Y = strRes[2];

                    //lstRes.Add(xyData);

                    //PIMTestResult1 = (XYDataArr)lstRes;
                    
                    foreach (PIMTestTrace tr in ItemList)
                    {
                        if (tr.TypeName == PIMTestTraceType.PIMTrace1)
                        {
                            (tr.TestResult as XYTraceResult).ResultData.Add(new XYData { X = dblIMF, Y = strRes[2] });
                        }
                        if (tr.TypeName == PIMTestTraceType.PIMTrace2)
                        {
                            (tr.TestResult as XYTraceResult).ResultData.Add(new XYData { X = dblIMF, Y = strRes[2] });
                        }
                    }

                }
                else if (CalType == PIMCalType.Sweep)
                {
                    SweepFlag = PIMTester.getSweepFlag();

                    //int number = (PIMOrder / 2);
                    //double dblTestStep1 = TestStepper * (number + 1f);
                    //double dblTestStep2 = TestStepper * number;

                    int intTotalCount1 = (SweepFlag == "F1UP" ? (int)((StopFreq1 - StartFreq1) / TestStepper) : (int)((StopFreq2 - StartFreq2) / TestStepper));
                    int intTotalCount2 = (SweepFlag == "F1UP" ? (int)((StopFreq2 - StartFreq2) / TestStepper) : (int)((StopFreq1 - StartFreq1) / TestStepper));

                    PIMTester.setModeSweepTx(TestStepper, CWFreq1, CWFreq2);
                    PIMTester.setPowOn(true, true);

                    double newData1 = -200;
                    double newData2 = -200;
                    double dblIMF1 = 0;
                    double dblIMF2 = 0;

                    for (int i = 0; i < (intTotalCount1 + intTotalCount2 + 2); i++)
                    {
                        bool bRunState = GetRunState();
                        if (!bRunState)
                        {
                            PIMTester.setPowOn(false, false);

                            foreach (PIMTestTrace tr in ItemList)
                            {
                                (tr.TestResult as XYTraceResult).ResultData.Clear();
                            }

                            return;
                        }

                        double dblNowFreq1 = 0;
                        double dblNowFreq2 = 0;

                        int j = 0;
                        if (i <= intTotalCount1)
                        {
                            j = i;

                            dblNowFreq1 = (SweepFlag == "F1UP" ? (StartFreq1 + i * TestStepper) : StartFreq1);
                            dblNowFreq2 = (SweepFlag == "F1UP" ? StopFreq2 : (StopFreq2 - j * TestStepper));

                        }
                        else
                        {
                            j = (i - intTotalCount1 - 1);

                            dblNowFreq1 = (SweepFlag == "F1UP" ? StartFreq1 : (StartFreq1 + i * TestStepper));
                            dblNowFreq2 = (SweepFlag == "F1UP" ? (StopFreq2 - j * TestStepper) : StopFreq2);

                        }

                        PIMTester.setPowATT("CARR1", CWPOW1);
                        PIMTester.setPowATT("CARR2", CWPOW2);

                        double[] strRes = new double[4];
                        if (ResultUnit == ResultUnit.dBm)
                        {
                            strRes = PIMTester.getSweepIMPow(PIMOrder, dblNowFreq1, dblNowFreq2, 0);
                        }
                        else
                        {
                            strRes = PIMTester.getSweepIMPow(PIMOrder, dblNowFreq1, dblNowFreq2, CWPOW1);
                        }

                        PIMPower1 = strRes[0];
                        PIMPower2 = strRes[1];

                        if (i <= intTotalCount1)
                        {
                            if (SweepFlag == "F1UP")
                            {
                                foreach (PIMTestTrace tr in ItemList)
                                {
                                    if (tr.TypeName == PIMTestTraceType.PIMTrace1)
                                    {
                                        (tr.TestResult as XYTraceResult).ResultData.Add(new XYData { X = strRes[2], Y = strRes[3] });
                                    }
                                }
                                if (newData1 < strRes[3])
                                {
                                    dblIMF1 = strRes[2];
                                    newData1 = strRes[3];
                                }
                            }
                            else
                            {
                                foreach (PIMTestTrace tr in ItemList)
                                {
                                    if (tr.TypeName == PIMTestTraceType.PIMTrace2)
                                    {
                                        (tr.TestResult as XYTraceResult).ResultData.Add(new XYData { X = strRes[2], Y = strRes[3] });
                                    }
                                }
                                if (newData2 < strRes[3])
                                {
                                    dblIMF2 = strRes[2];
                                    newData2 = strRes[3];
                                }
                            }

                        }
                        else
                        {
                            if (SweepFlag == "F1UP")
                            {
                                foreach (PIMTestTrace tr in ItemList)
                                {
                                    if (tr.TypeName == PIMTestTraceType.PIMTrace2)
                                    {
                                        (tr.TestResult as XYTraceResult).ResultData.Add(new XYData { X = strRes[2], Y = strRes[3] });
                                    }
                                }
                                if (newData2 < strRes[3])
                                {
                                    dblIMF2 = strRes[2];
                                    newData2 = strRes[3];
                                }
                            }
                            else
                            {
                                foreach (PIMTestTrace tr in ItemList)
                                {
                                    if (tr.TypeName == PIMTestTraceType.PIMTrace1)
                                    {
                                        (tr.TestResult as XYTraceResult).ResultData.Add(new XYData { X = strRes[2], Y = strRes[3] });
                                    }
                                }
                                if (newData1 < strRes[3])
                                {
                                    dblIMF1 = strRes[2];
                                    newData1 = strRes[3];
                                }
                            }
                        }
                    }

                }
                else
                {
                    int num = PIMOrder / 2;
                    double dblIMF = (((num + 1) * CWFreq1) - (num * CWFreq2));

                    PIMTester.setModeStandard(dblIMF);

                    PIMTester.setPowOn(true, true);

                    List<XYData> lstRes = new List<XYData>();

                    DateTime TimeOld = System.DateTime.Now;

                    int i = 0;
                    while (System.DateTime.Now.Subtract(TimeOld).TotalSeconds < TestTime)
                    {
                        bool bRunState = GetRunState();
                        if (!bRunState)
                        {
                            PIMTester.setPowOn(false, false);
                            return;
                        }

                        PIMTester.setPowATT("CARR1", CWPOW1);
                        PIMTester.setPowATT("CARR2", CWPOW2);

                        double[] strRes = new double[3];
                        if (ResultUnit == ResultUnit.dBm)
                        {
                            strRes = PIMTester.getPointIMPow(0);
                        }
                        else
                        {
                            strRes = PIMTester.getPointIMPow(CWPOW1);
                        }
                        

                        PIMPower1 = strRes[0];
                        PIMPower2 = strRes[1];

                        foreach (PIMTestTrace tr in ItemList)
                        {
                            if (tr.TypeName == PIMTestTraceType.PIMTrace1)
                            {
                                (tr.TestResult as XYTraceResult).ResultData.Add(new XYData { X = i, Y = strRes[2] });
                            }
                            if (tr.TypeName == PIMTestTraceType.PIMTrace2)
                            {
                                (tr.TestResult as XYTraceResult).ResultData.Add(new XYData { X = i, Y = strRes[2] });
                            }
                        }
                        i++;

                    }

                }

                PIMTester.setPowOn(false, false);
            }

            SingleDBTime = DataUtils.StaticInfo.DBTime;
        }
        public override TestTrace GetDefaultTestTrace()
        {
            return new PIMTestTrace();
        }

        
        
    }
    public class PIMTestTrace : TestTrace
    {
        public PIMTestTrace()
        {
            TypeName = PIMTestTraceType.PIMTrace1;
            TestResult = new XYTraceResult();
        }
        public PIMSweepSourceEnum SweepSource { get; set; }
        public override TestMarker GetDefaultMarker()
        {
            return new XYTestMarker();
        }
    }
    public class PIMTestTraceType
    {
        public const string PIMTrace1 = "互调f1";
        public const string PIMTrace2 = "互调f2";
    }
    public enum PIMSweepSourceEnum
    {
        F1,F2
    }
}
