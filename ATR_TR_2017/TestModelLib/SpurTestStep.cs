using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using Symtant.InstruDriver.SpectrumAnalyzer;
using Symtant.InstruDriver;
namespace TestModelLib
{
    public class SpurTestStep:TestStep
    {
        public SpurTestStep()
        {
            IsSignalON = true;
            SignalFreq = 2e9;
            RefLevel = 0;
            StartFreq = 1e9;
            StopFreq = 3e9;
            SweepTimeAutoEnable = true;
            SweepPoints = 1001;
            RBW = 1e6;
            SignalIdentifySpan = 10e6;
        }
        
        private ISpecAnalyzer SA
        {
            get
            {
                return MeasInfo.InstruInfoList[0].InstruDriver as ISpecAnalyzer;
            }
        }
        /// <summary>
        /// 存在信号
        /// </summary>
        [UIDisplay("存在信号")]
        public bool IsSignalON { get; set; }
        /// <summary>
        /// 信号频率
        /// </summary>
        [UIDisplay("信号频率",typeof(DataUtils.FreqStringConverter))]
        [UIDisplayPara("信号频率")]
        public double SignalFreq { get; set; }
        /// <summary>
        /// 参考电平
        /// </summary>
        [UIDisplay("参考电平")]
        public double RefLevel { get; set; }
        /// <summary>
        /// 频谱仪起始频率
        /// </summary>
        [UIDisplay("频谱仪起始频率", typeof(DataUtils.FreqStringConverter))]
        [UIDisplayPara("频谱仪起始频率")]
        public double StartFreq { get; set; }
        /// <summary>
        /// 频谱仪截止频率
        /// </summary>
        [UIDisplay("频谱仪截止频率", typeof(DataUtils.FreqStringConverter))]
        [UIDisplayPara("频谱仪截止频率")]
        public double StopFreq { get; set; }
        /// <summary>
        /// RBW
        /// </summary>
        [UIDisplay("RBW", typeof(DataUtils.FreqStringConverter))]
        public double RBW { get; set; }
        /// <summary>
        /// 扫描点数
        /// </summary>
        [UIDisplay("扫描点数")]
        public int SweepPoints { get; set; }
        /// <summary>
        /// 扫描时间
        /// </summary>
        [UIDisplay("扫描时间")]
        public double SweepTime { get; set; }
        /// <summary>
        /// 自动扫描时间
        /// </summary>
        [UIDisplay("自动扫描时间")]
        public bool SweepTimeAutoEnable { get; set; }
        /// <summary>
        /// 信号识别带宽
        /// </summary>
        [UIDisplay("信号识别带宽", typeof(DataUtils.FreqStringConverter))]
        public double SignalIdentifySpan { get; set; }

        private int sweepTime;
        
        public override void Single()
        {
            if (GeneTestSetup.Instance.IsSimulated)
            {
                GeneSimulatedData();
            }
            else
            {
                ///set sa
                SA.ResetInterface();
                SA.StartFreq = StartFreq;
                SA.StopFreq = StopFreq;
                SA.DetType = SpecAnalyzerDetTypeEnum.POS;
                SA.RBW = RBW;
                SA.VBW = RBW;
                SA.RefLevel = RefLevel;
                SA.SweepPoints = SweepPoints;
                SA.InitContEnable = false;
                if (!SweepTimeAutoEnable)
                {
                    SA.SweepTime = SweepTime;
                }
                (SA as InstruDriver).Wait(1000);
                sweepTime = (int)SA.SweepTime * 1000;
                ///single
                SA.Init();

                (SA as InstruDriver).Wait(sweepTime * 2 + 1000);
                double spur = GetSpur();
                (ItemList[0] as PointTestItem).Y = spur;
                (ItemList[0] as PointTestItem).X = SignalFreq;
            }
        }
        public override void GeneSimulatedData()
        {
            foreach(PointTestItem item in ItemList)
            {
                item.X = SignalFreq;
                item.Y = Symtant.GeneFunLib.GeneFun.GetRand(-50, -60);
            }
        }
        public override void CreateTrace(string traceTypeName)
        {
            ItemList.Add(new PointTestItem() { TypeName = traceTypeName });
        }
        private double GetSpur()
        {

            double[] traceData = SA.GetTraceData();
            double Peak = traceData.Max();
            if (IsSignalON)
            {
                double deltaF = (StopFreq - StartFreq) / SA.SweepPoints;
                int startn = (int)Math.Ceiling(StartFreq / SignalFreq);
                if (startn == 0)
                    startn = 1;
                int stopn = (int)Math.Floor(StopFreq / SignalFreq);
                int idSpan = (int)Math.Round(SignalIdentifySpan / deltaF / 2);
                for (int i = startn; i <= stopn; i++)
                {
                    int temp = (int)Math.Round((i * SignalFreq - StartFreq) / deltaF);
                    for (int j = temp - idSpan; j < temp + idSpan; j++)
                    {
                        if (j > -1 && j < SA.SweepPoints)
                        {
                            traceData[j] = -200;
                        }
                    }
                }
                double NextPeak = traceData.Max();
                return NextPeak - Peak;
            }
            else
            {
                return Peak;
            }
        }
    }
}
