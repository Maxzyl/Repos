using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symtant.InstruDriver.SwitchBox;
using Symtant.InstruDriver;
namespace ModelBaseLib
{
    public interface IPathConfigSetter
    {
        void SetPathConfig(string pathName);
        string[] GetPathConfigNameList();
        void PostPathConfig(string pathName);
    }
    [UITestStepPara("开关设置",null,null)]
    public class SwitchBoxPathConfig : TestStep, IPathConfigSetter
    {
        private ISwitchBox switchBox
        {
            get
            {
                return MeasInfo.InstruInfoList[0].InstruDriver as ISwitchBox;
            }
        }
        MeasClsInfo[] measClsInfoList = new MeasClsInfo[]
        {
            new MeasClsInfo()
            {
                TypeName="pathConfig",
                DisplayName="开关箱",

                InstruInfoList=new InstruInfo[]
                {
                    new InstruInfo()
                    {
                        Name="sw",
                        DisplayName="开关箱"
                    }
                }
            }
        };
        public override MeasClsInfo[] SupportedMeasClsInfoList
        {
            get
            {
                return measClsInfoList;
            }
        }

        public void SetPathConfig(string pathName)
        {
            if (GeneTestSetup.Instance.IsSimulated)
            {
                DataUtils.LOGINFO.WriteLog("setting path " + pathName);
            }
            else
            {
                if (switchBox == null)
                {
                    throw (new Exception("can't get switch box"));
                }
                else
                {
                    switchBox.SetPath(pathName);
                }
            }
        }

        public string[] GetPathConfigNameList()
        {
            if (switchBox == null)
            {
                return null;
            }
            else
            {
                return switchBox.GetPathList();
            }
        }
        [UIDisplay("开关路径")]
        public string Path { get; set; }
        public override void Single()
        {
            SetPathConfig(Path);
        }


        public void PostPathConfig(string pathName)
        {
            
        }
    }
}
