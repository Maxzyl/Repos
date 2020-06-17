using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using Symtant.InstruDriver.RFSignalGenerator;
using Symtant.InstruDriver;
namespace TestModelLib
{
    public class SignalGeneSetupStep:TestStep
    {
        public SignalGeneSetupStep()
        {
            Freq = 1e9;
            PowerLevel = 0;
            OutputEnable = true;
            PulseWidth = 1e-4;
            PulsePeriod = 1e-3;

        }
        private IRFSignalGenerator SG
        {
            get
            {
                return MeasInfo.InstruInfoList[0].InstruDriver as IRFSignalGenerator;
            }
        }
        /// <summary>
        /// 频率
        /// </summary>
        [UIDisplayPara("频率")]
        [UIDisplay("频率", typeof(DataUtils.FreqStringConverter))]
        public double Freq { get; set; }
        /// <summary>
        /// 功率
        /// </summary>
        [UIDisplayPara("功率")]
        [UIDisplay("功率")]
        public double PowerLevel { get; set; }
        /// <summary>
        /// 射频输出
        /// </summary>
        /// 
        [UIDisplay("射频输出")]
        public bool OutputEnable { get; set; }
        /// <summary>
        /// 脉冲宽度
        /// </summary>
        public double PulseWidth { get; set; }
        /// <summary>
        /// 脉冲周期
        /// </summary>
        public double PulsePeriod { get; set; }
        /// <summary>
        /// 脉冲触发源
        /// </summary>
        public RFSignalGeneratorPulseTriggerSourceEnum PulseTriggerSource { get; set; }
        /// <summary>
        /// 触发延时
        /// </summary>
        public double PulseTriggerDelay { get; set; }
        /// <summary>
        /// 触发沿
        /// </summary>
        public PulseTriggerSlopeEnum PulseTriggerSlope { get; set; }
        /// <summary>
        /// 脉冲使能
        /// </summary>
        public bool PulseEnable { get; set; }
        public override void Single()
        {
            if (GeneTestSetup.Instance.IsSimulated)
            {
                
            }
            else
            {
                SG.ResetInterface();
                SG.Freq = Freq;
                SG.PowerLevel = PowerLevel;
                SG.OutputEnable = OutputEnable;
                (SG as InstruDriver).Wait(3000);
            }
        }
    }
}
