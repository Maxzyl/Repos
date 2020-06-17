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
    public class HarmTestStep:TestStep
    {
        public HarmTestStep()
        {
            SignalFreq = 1e9;
            RefLevel = 0;
            SweepTime = 1e-3;
            RecvBW = 1e6;
        }
        private ISpecAnalyzer SA
        {
            get
            {
                return MeasInfo.InstruInfoList[0].InstruDriver as ISpecAnalyzer;
            }
        }
        /// <summary>
        /// 信号频率
        /// </summary>
        [UIDisplay("信号频率", typeof(DataUtils.FreqStringConverter))]
        [UIDisplayPara("信号频率")]
        public double SignalFreq { get; set; }
        /// <summary>
        /// 参考电平
        /// </summary>
        [UIDisplay("参考电平")]
        public double RefLevel { get; set; }
        /// <summary>
        /// 扫描时间
        /// </summary>
        [UIDisplay("扫描时间")]
        public double SweepTime { get; set; }
        /// <summary>
        /// 接收带宽
        /// </summary>
        [UIDisplay("接收带宽", typeof(DataUtils.FreqStringConverter))]
        public double RecvBW { get; set; }
        private double SignalPower;
        public override void Single()
        {
            if (GeneTestSetup.Instance.IsSimulated)
            {
                GeneSimulatedData();
            }
            else
            {
                SA.ResetInterface();
                SA.Span = 0;
                SA.CenterFreq = SignalFreq;
                SA.RefLevel = RefLevel;
                SA.SweepTime = SweepTime;
                SA.RBW = RecvBW;
                SA.DetType = SpecAnalyzerDetTypeEnum.POS;
                SA.VBW = RecvBW;
                SA.InitContEnable = false;

                (SA as InstruDriver).Wait(1000);
                SA.Init();

                (SA as InstruDriver).Wait(1000+(int)(SweepTime*1000));
                SA.MarkerMaxSearch(1);
                (SA as InstruDriver).Wait(1000);
                SignalPower = SA.GetMarkerY(1);
                foreach (HarmTestItem item in ItemList)
                {
                    SA.CenterFreq = SignalFreq * item.nHarm;
                    SA.Init();

                    (SA as InstruDriver).Wait(1000 + (int)(SweepTime * 1000));
                    SA.MarkerMaxSearch(1);
                    (SA as InstruDriver).Wait(1000);
                    double p = SA.GetMarkerY(1);
                    if (item.nHarm == 1)
                    {
                        item.X = SignalFreq;
                        item.Y = p;
                    }
                    else
                    {
                        item.X = SignalFreq;
                        item.Y = p-SignalPower;
                    }
                }
            }
        }
        public override void GeneSimulatedData()
        {
            foreach (PointTestItem item in ItemList)
            {
                item.X = SignalFreq;
                item.Y = Symtant.GeneFunLib.GeneFun.GetRand(-30, -40);
            }
        }
        public override void CreateTrace(string traceTypeName)
        {
            ItemList.Add(new HarmTestItem() { TypeName = traceTypeName });
        }
    }
    public class HarmTestItem:PointTestItem
    {
        public HarmTestItem()
        {
            nHarm = 2;
        }
        [UIDisplay("谐波次数", null, 1,2, 3, 4, 5, 6, 7, 8, 9, 10)]
        public int nHarm { get; set; }
    }
}
