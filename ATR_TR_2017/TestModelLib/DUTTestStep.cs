using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using Symtant.InstruDriver.AISG;
namespace TestModelLib
{
    public class DUTTestStep : TestStep
    {
        public DUTTestStep()
        {
            
        }
        
        
        public int SubNum { get; set; }
        public AISGTMAModeType Mode { get; set; }
        public double Gain { get; set; }
        public VendorFlagEnum VendorFlag { get; set; }
        public string AISGVersion { get; set; }
        public string WarningCurrent { get; set; }
        private new MeasClsInfo MeasInfo
        {
            get
            {
                var t = TestStepInfoMgr.Instance.TestStepInfoList.Where(x => x.TypeName == "DUTTestStep").FirstOrDefault();
                if (t != null)
                {
                    return t.MeasClsInfoList.Where(x => x.TypeName == "DCPS").FirstOrDefault();
                }
                else return null;
            }
        }
        public override void Single()
        {
            if (GeneTestSetup.Instance.IsSimulated)
            {
                

            }
            else
            {
                string dutAddr = MeasInfo.InstruInfoList[1].Address;
                //连接串口
                HWAISGCOM aisg = new HWAISGCOM();
                aisg.Open(dutAddr, 9600);
                

                
                //设置参数
                aisg.InitDut();
                aisg.SetGain(Gain.ToString());
                string gain = aisg.GetGain();
                if (gain != Gain.ToString())
                {
                    DataUtils.LOGINFO.WriteLog("set aisg gain wrong");
                }
                aisg.SetTMAMode(SubNum, Mode);
                var mode = aisg.GetTMAMode(SubNum);
                if (mode != Mode)
                {
                    DataUtils.LOGINFO.WriteLog("set aisg tma mode wrong");
                }
                //关闭串口
                aisg.Close();
            }
        }
        public override void InitOnce()
        {
            base.InitOnce();
        }

        public override void InitCal()
        {
            
        }
    }

    public enum ModelEnum { Normal, Bypass };
    public enum VendorFlagEnum { HW };

    public class AISGDeviceInfo
    {
        public string DeviceUID { get; set; }
        public string DeviceAdress { get; set; }
        public string DeviceName { get; set; }
        public string SubNum { get; set; }
        public string VendorID { get; set; }
        public string ProductName { get; set; }
        public string DeviceSN { get; set; }
        public string HardwareVersion { get; set; }
        public string SoftwareVersion { get; set; }
        public string RXBand { get; set; }
        public string TXBand { get; set; }
        public string GainRange { get; set; }
        public string GainStep { get; set; }
        public string AISGVersion { get; set; }
        
    }

}
