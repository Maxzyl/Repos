using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symtant.InstruDriver;
using Symtant.InstruDriver.SwitchBox;
namespace ModelBaseLib
{
    public class TestConfigSetupStep:TestStep
    {
        private MeasClsInfo clsInfo = new MeasClsInfo()
        {
            DisplayName = "开关箱",
            TypeName = "sw",
            InstruInfoList = new InstruInfo[]
               {
                   new InstruInfo()
                   {
                        DisplayName="开关箱",
                        Name="sw"
                   }
               }
        };
        private ISwitchBox sb
        {
            get
            {
                return MeasInfo.InstruInfoList[0].InstruDriver as ISwitchBox;
            }
        }
        public override MeasClsInfo[] SupportedMeasClsInfoList
        {
            get
            {
                return new MeasClsInfo[]{clsInfo};
            }
        }
        public override void Single()
        {
            if (!GeneTestSetup.Instance.IsSimulated)
            {
                sb.SetPath(ConfigName);
            }
        }
        
    }
}
