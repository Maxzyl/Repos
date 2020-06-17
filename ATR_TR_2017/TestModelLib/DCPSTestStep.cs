using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;

using Symtant.InstruDriver;
using Symtant.InstruDriver.DCPowerSupply;

namespace TestModelLib
{
    public class DCPSTestStep : TestStep
    {
        public DCPSTestStep()
        {
            DCPSInfo1 = new DCPSInfo();
            DCPSInfo2 = new DCPSInfo();
            DCPSInfo3 = new DCPSInfo();
            DCPSInfo4 = new DCPSInfo();
        }

        [System.Xml.Serialization.XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public IMultiChannelDCPowerSupply DCPS
        {
            get
            {
                return MeasInfo.InstruInfoList[0].InstruDriver as IMultiChannelDCPowerSupply;
            }
        }
        [UIDisplayPara("通道1电压")]
        public double Voltage1
        {
            get
            {
                return DCPSInfo1.Voltage;
            }
            set
            {
                DCPSInfo1.Voltage = value;
            }
        }
        [UIDisplayPara("通道2电压")]
        public double Voltage2
        {
            get
            {
                return DCPSInfo2.Voltage;
            }
            set
            {
                DCPSInfo2.Voltage = value;
            }
        }
        [UIDisplayPara("通道3电压")]
        public double Voltage3
        {
            get
            {
                return DCPSInfo3.Voltage;
            }
            set
            {
                DCPSInfo3.Voltage = value;
            }
        }
        [UIDisplayPara("通道4电压")]
        public double Voltage4
        {
            get
            {
                return DCPSInfo4.Voltage;
            }
            set
            {
                DCPSInfo4.Voltage = value;
            }
        }

        public DCPSInfo DCPSInfo1 { get; set; }
        public DCPSInfo DCPSInfo2 { get; set; }
        public DCPSInfo DCPSInfo3 { get; set; }
        public DCPSInfo DCPSInfo4 { get; set; }

        public override void Single()
        {
            if (GeneTestSetup.Instance.IsSimulated)
            {
                

            }
            else
            {
                if(DCPSInfo1.IsTest)
                {
                    DCPS.SetVoltage(1,DCPSInfo1.Voltage);
                    DCPS.SetCurrentLimit(1,DCPSInfo1.Current);
                    DCPS.SetOutputEnable(1, true);
                }

                if(DCPSInfo2.IsTest)
                {
                    DCPS.SetVoltage(2,DCPSInfo2.Voltage);
                    DCPS.SetCurrentLimit(2,DCPSInfo2.Current);
                    DCPS.SetOutputEnable(2, true);
                }

                if(DCPSInfo3.IsTest)
                {
                    DCPS.SetVoltage(3,DCPSInfo3.Voltage);
                    DCPS.SetCurrentLimit(3,DCPSInfo3.Current);
                    DCPS.SetOutputEnable(3, true);
                }

                if(DCPSInfo4.IsTest)
                {
                    DCPS.SetVoltage(4,DCPSInfo4.Voltage);
                    DCPS.SetCurrentLimit(4,DCPSInfo4.Current);
                    DCPS.SetOutputEnable(4, true);
                }

            }
        }
        public override void InitOnce()
        {
            base.InitOnce();
        }

        public override void CleanUp()
        {
            if (!GeneTestSetup.Instance.IsSimulated)
            {
                if (DCPSInfo1.IsTest)
                {
                    if (DCPSInfo1.IsAutoOff)
                    {
                        DCPS.SetOutputEnable(1, false);
                    }
                }

                if (DCPSInfo2.IsTest)
                {
                    if (DCPSInfo2.IsAutoOff)
                    {
                        DCPS.SetOutputEnable(2, false);
                    }
                }

                if (DCPSInfo3.IsTest)
                {
                    if (DCPSInfo3.IsAutoOff)
                    {
                        DCPS.SetOutputEnable(3, false);
                    }
                }

                if (DCPSInfo4.IsTest)
                {
                    if (DCPSInfo4.IsAutoOff)
                    {
                        DCPS.SetOutputEnable(4, false);
                    }
                }
            }
        }
        //public override TestTrace GetDefaultTestTrace()
        //{
        //    return null;
        //}
    }

    public class DCPSInfo
    {
        public DCPSInfo()
        {
            OutType = OutTypeEnum.电压;
            Voltage = 0;
            Current = 0;
            IsAutoOff = true;
            IsTest = false;
        }
        public OutTypeEnum OutType { get; set; }
        public double Voltage { get; set; }
        public double Current { get; set; }
        public bool OutputEnable { get; set; }
        public bool IsAutoOff { get; set; }
        public bool IsTest { get; set; }
    }

    public enum OutTypeEnum { 电压, 电流 };
}
