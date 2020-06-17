using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using Symtant.GeneFunLib;

namespace TestModelLib
{
    public class AvgPowerTestStep:TestStep
    {
        public AvgPowerTestStep()
        {
            ChannelNum = 1;
            SignalFreq = 1e9;
            
        }
        private Symtant.InstruDriver.PowerMeter.IAvgPowerMeter PM
        {
            get
            {
                return MeasInfo.InstruInfoList[0].InstruDriver as Symtant.InstruDriver.PowerMeter.IAvgPowerMeter;
            }
        }
        /// <summary>
        /// 通道号
        /// </summary>
        [UIDisplay("通道")]
        public int ChannelNum { get; set; }
        /// <summary>
        /// 频率
        /// </summary>
        [UIDisplay("频率", typeof(DataUtils.FreqStringConverter))]
        [UIDisplayPara("频率")]
        public double SignalFreq { get; set; }
        /// <summary>
        /// 是否平均
        /// </summary>
        [UIDisplay("是否平均")]
        public bool AvgEnable { get; set; }
        /// <summary>
        /// 平均次数
        /// </summary>
        [UIDisplay("平均次数")]
        public int AvgCount { get; set; }
        /// <summary>
        /// 自动平均次数
        /// </summary>
        [UIDisplay("自动平均次数")]
        public bool AvgCountAutoEnable { get; set; }

        public override void Single()
        {
            if (GeneTestSetup.Instance.IsSimulated)
            {
                GeneSimulatedData();
            }
            else
            {
                PM.ResetInterface();
                PM.SetFreq(ChannelNum, SignalFreq);
                PM.SetAvgEnable(ChannelNum, AvgEnable);
                PM.SetAvgCount(ChannelNum, AvgCount);
                PM.SetAvgCountAutoEnable(ChannelNum, AvgCountAutoEnable);
                PM.SetInitContEnable(ChannelNum, false);
                PM.SetMeasChannel(1, 1);
                PM.Init();
                double pow = PM.GetMeasData(1);
                foreach (PointTestItem item in ItemList)
                {
                    item.X = SignalFreq;
                    item.Y = pow;
                }
            }
        }
        public override void GeneSimulatedData()
        {
            foreach (PointTestItem item in ItemList)
            {
                item.X = SignalFreq;
                item.Y = Symtant.GeneFunLib.GeneFun.GetRand(20, 10);
            }
        }
        public override void CreateTrace(string traceTypeName)
        {
            ItemList.Add(new PointTestItem() { TypeName = traceTypeName });
        }

    }
    
}
