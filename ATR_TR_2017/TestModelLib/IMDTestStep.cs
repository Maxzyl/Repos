using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using Symtant.GeneFunLib;
using Symtant.InstruDriver.RFSignalGenerator;
using Symtant.InstruDriver.SpectrumAnalyzer;
using Symtant.InstruDriver;
using Symtant.InstruDriver.PowerMeter;
namespace TestModelLib
{

    public class IMDTestStep:TestStep
    {
        private IRFSignalGenerator sg1
        {
            get
            {
                return (MeasInfo.InstruInfoList.Where(x => x.Name == "sg1").FirstOrDefault()).InstruDriver as IRFSignalGenerator;
            }
        }
        private IRFSignalGenerator sg2
        {
            get
            {
                return (MeasInfo.InstruInfoList.Where(x => x.Name == "sg2").FirstOrDefault()).InstruDriver as IRFSignalGenerator;
            }
        }
        private ISpecAnalyzer sa
        {
            get
            {
                return (MeasInfo.InstruInfoList.Where(x => x.Name == "sa").FirstOrDefault()).InstruDriver as ISpecAnalyzer;
            }
        }
        private IAvgPowerMeter pm
        {
            get
            {
                return (MeasInfo.InstruInfoList.Where(x => x.Name == "pm").FirstOrDefault()).InstruDriver as IAvgPowerMeter;
            }
        }
        [UIDisplayPara("F1")]
        public double F1 { get; set; }
        [UIDisplayPara("F2")]
        public double F2 { get; set; }
        public double F1Power { get; set; }
        public double F2Power { get; set; }
        public IMDSweepModeEnum SweepMode { get; set; }
        /// <summary>
        /// 互调阶数，例如3，5
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 接收机的衰减器
        /// </summary>
        public double RecvAtten { get; set; }
        /// <summary>
        /// 接收机带宽，比如RBW
        /// </summary>
        public double MeasBandwidth { get; set; }
        /// <summary>
        /// 上边带、下边带、还是平均值
        /// </summary>
        public IMDSidebandEnum Sideband { get; set; }
        /// <summary>
        /// 激励功率容限
        /// </summary>
        public double PowerTolerance { get; set; }
        public double PowerMeterLoss { get; set; }
        public override object GetLocalSetting()
        {
            return new double[] { PowerTolerance, PowerMeterLoss,PowerErr1,PowerErr2 };
        }
        public override void SetLocalSetting(object v)
        {
            if (v is double[])
            {
                if ((v as double[]).Count() == 2)
                {
                    PowerTolerance = (v as double[])[0];
                    PowerMeterLoss = (v as double[])[1];
                    PowerErr1 = (v as double[])[2];
                    PowerErr2 = (v as double[])[3];
                }
            }
        }
        public override void Single()
        {
            if (GeneTestSetup.Instance.IsSimulated)
            {
                foreach (TestTrace tr in ItemList)
                {
                    
                    tr.ResultData.Clear();
                    tr.ResultData.Add(new XYData { X = 0, Y = GeneFun.GetRand(-100, -50) });
                    
                }
            }
            else
            {
                double fMeas1 = 1e9;
                double fMeas2 = 1e9;
                double IMD=0;
                double OIP3 = 0;
                
                switch (Order)
                {
                    case 3:
                        fMeas1 = 2 * F1 - F2;
                        fMeas2 = 2 * F2 - F1;
                        break;
                    case 5:
                        fMeas1 = 3 * F1 - 2 * F2;
                        fMeas2 = 3 * F2 - 2 * F1;
                        break;
                    default:
                        break;
                }
                double measSpan = (F2 - F1) / 4;
                sg1.ResetInterface();
                sg2.ResetInterface();
                sa.ResetInterface();
                sg1.Freq = F1;
                sg2.Freq = F2;
                //sg1.PowerLevel = F1Power;
                //sg2.PowerLevel = F2Power;
                sg1.PowerLevel = F1Power + PowerErr1;
                sg2.PowerLevel = F2Power + PowerErr2;
                sg1.OutputEnable = true;
                sg2.OutputEnable = true;
                (sg1 as InstruDriver).Wait();
                (sg2 as InstruDriver).Wait();
                (sa as InstruDriver).Wait();
                switch (Sideband)
                {
                    case IMDSidebandEnum.Low:

                        double res1 = MeasPeak(fMeas1, measSpan);
                        double p1 = MeasPeak(F1, measSpan);
                        //double p2 = MeasPeak(F2, measSpan);
                        IMD = (res1 - p1) / 2;
                        OIP3 = p1 - IMD;
                        break;
                    case IMDSidebandEnum.High:
                        double res2 = MeasPeak(fMeas2, measSpan);
                        double p2 = MeasPeak(F2, measSpan);
                        //double p2 = MeasPeak(F2, measSpan);
                        IMD = (res2 - p2) / 2;
                        OIP3 = p2 - IMD;
                        break;
                    case IMDSidebandEnum.Avg:
                        res1 = MeasPeak(fMeas1, measSpan);
                        res2 = MeasPeak(fMeas2, measSpan);
                        p1 = MeasPeak(F1, measSpan);
                        p2 = MeasPeak(F2, measSpan);
                        //double p2 = MeasPeak(F2, measSpan);
                        IMD = (res1+res2 -p1- p2) / 4;
                        OIP3 = (p1 + p2) / 2 - IMD;
                        break;
                    case IMDSidebandEnum.Worst:
                        res1 = MeasPeak(fMeas1, measSpan);
                        res2 = MeasPeak(fMeas2, measSpan);
                        if (res1 >= res2)
                        {
                            p1 = MeasPeak(F1, measSpan);
                            IMD = (res1 - p1) / 2;
                            OIP3 = p1 - IMD;
                        }
                        else
                        {
                            p2 = MeasPeak(F2, measSpan);
                            IMD = (res2 - p2) / 2;
                            OIP3 = p2 - IMD;
                        }
                        break;
                    default:
                        break;
                }
                double IIP3 = (F1Power + F2Power) / 2 - IMD;
                foreach (TestTrace tr in ItemList)
                {
                    var tRes = tr;
                    if (tRes != null)
                    {
                        tRes.ResultData.Clear();
                        switch (tr.TypeName)
                        {
                            case IMDTestTraceType.IMD:
                                tRes.ResultData.Add(new XYData() { X = (F1 + F2) / 2, Y = IMD });
                                break;
                            case IMDTestTraceType.IIP3:
                                tRes.ResultData.Add(new XYData() { X = (F1 + F2) / 2, Y = IIP3 });
                                break;
                            case IMDTestTraceType.OIP3:
                                tRes.ResultData.Add(new XYData() { X = (F1 + F2) / 2, Y = OIP3 });
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        private double MeasPeak(double cf,double span)
        {
            sa.CenterFreq = cf;
            sa.Span = span;
            sa.AttAutoEnable = false;
            sa.Attenuation = RecvAtten;
            sa.RBW = MeasBandwidth;
            sa.InitContEnable = false;
            (sa as InstruDriver).Wait();
            sa.Init();
            (sa as InstruDriver).Wait();
            sa.MarkerMaxSearch(1);
            (sa as InstruDriver).Wait();
            double res=sa.GetMarkerY(1);
            return res;
        }
        //public override TestTrace GetDefaultTestTrace()
        //{
        //    return new TestTrace();
        //}
        private const int PowerCalCount = 10;
        private double PowerErr1;
        private double PowerErr2;
        public void CalIMDPower()
        {
            sg1.ResetInterface();
            sg2.ResetInterface();
            pm.ResetInterface();
            pm.SetInitContEnable(1, false);
            pm.SetMeasChannel(1, 1);
            sg1.Freq = F1;
            sg2.Freq = F2;
            sg1.OutputEnable = true;
            (sg1 as InstruDriver).Wait();
            PowerErr1 = 0;
            pm.SetFreq(1, F1);

            sg1.PowerLevel = F1Power;
            for (int i = 0; i < PowerCalCount; i++)
            {
                //sg1.PowerLevel = F1Power - PowerErr1;
                (sg1 as InstruDriver).Wait();
                pm.Init();
                double res=pm.GetMeasData(1);
                PowerErr1 = res - F1Power;
                if (Math.Abs(res - F1Power) < PowerTolerance)
                {
                    PowerErr1 = sg1.PowerLevel - F1Power;
                    break;
                }
                else
                {
                    sg1.PowerLevel -= PowerErr1;
                }
                    
            }
            sg1.OutputEnable = false;
            (sg1 as InstruDriver).Wait();

            sg2.OutputEnable = true;
            (sg2 as InstruDriver).Wait();
            PowerErr2 = 0;
            pm.SetFreq(1, F2);

            sg2.PowerLevel = F2Power;
            for (int i = 0; i < PowerCalCount; i++)
            {
                //sg2.PowerLevel = F2Power - PowerErr2;
                (sg2 as InstruDriver).Wait();
                pm.Init();
                double res = pm.GetMeasData(1);
                PowerErr2 = res - F2Power;
                if (Math.Abs(res - F2Power) < PowerTolerance)
                {
                    PowerErr2 = sg2.PowerLevel - F2Power;
                    break;
                }
                else
                {
                    sg2.PowerLevel -= PowerErr2;
                }
            }
            sg2.OutputEnable = false;
            (sg2 as InstruDriver).Wait();
            
            
        }

        public override void CreateTrace(string traceTypeName)
        {

            ItemList.Add(new TestTrace() { TypeName = traceTypeName });


        }
        
    }
    public enum IMDSweepModeEnum { Point, Sweep };
    public enum IMDSidebandEnum { Low, High, Avg, Worst }
    //public class IMDTestTrace : TestTrace
    //{
    //    public IMDTestTrace()
    //    {
    //        TypeName = IMDTestTraceType.IMD;
    //        TestResult = new XYTraceResult();
    //    }
    //    //public PIMSweepSourceEnum SweepSource { get; set; }
    //    public override TestMarker GetDefaultMarker()
    //    {
    //        return new XYTestMarker();
    //    }
    //}
    /// <summary>
    /// IIP3=Pin-IMD/2
    /// OIP3=Pout-IMD/2
    /// </summary>
    public class IMDTestTraceType
    {
        public const string IMD = "IMD";
        public const string IIP3 = "IIP3";
        public const string OIP3 = "OIP3";
    }
}
