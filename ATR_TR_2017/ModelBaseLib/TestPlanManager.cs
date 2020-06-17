using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Symtant.GeneFunLib;
namespace ModelBaseLib
{
    public class TestPlanManager
    {
        public static TestPlan CurrentTestPlan { get; set; }
        public static Action TestPlanStarter { get; set; }
        private static IPathConfigSetter pathConfigSetter;
        public static IPathConfigSetter PathConfigSetter
        {
            get
            {
                if (pathConfigSetter == null)
                {
                    string pathConfigCls = System.Configuration.ConfigurationManager.AppSettings["PathConfig"];
                    pathConfigSetter = GeneFun.GetObjectFromClsPath(pathConfigCls) as IPathConfigSetter;

                }
                return pathConfigSetter;
            }
        }
        private static ITestPlanLocalSettingsStarter localSettingsStarter;
        public static ITestPlanLocalSettingsStarter LocalSettingsStarter
        {
            get
            {
                if (localSettingsStarter == null)
                {
                    string localStarterStr = System.Configuration.ConfigurationManager.AppSettings["LocalSettingsStarter"];
                    localSettingsStarter = GeneFun.GetObjectFromClsPath(localStarterStr) as ITestPlanLocalSettingsStarter;
                }
                return localSettingsStarter;
            }
        }

        private static ITestPlanSeq testPlanSeq;
        public static ITestPlanSeq TestPlanSeq
        {
            get
            {
                if (testPlanSeq == null)
                {
                    string testPlanSeqStr = System.Configuration.ConfigurationManager.AppSettings["TestPlanSeq"];
                    if (string.IsNullOrWhiteSpace(testPlanSeqStr))
                    {
                        testPlanSeq = null;
                    }
                    else
                    {
                        testPlanSeq = GeneFun.GetObjectFromClsPath(testPlanSeqStr) as ITestPlanSeq;
                    }
                }
                return testPlanSeq;
            }
        }
    }
    /// <summary>
    /// 测试方案开始本地设置和校准初始化调用的接口，单体模型，通过app.config配置
    /// </summary>
    public interface ITestPlanLocalSettingsStarter
    {
        void PreInit();
    }
    /// <summary>
    /// 测试方案开始初始化和测试之前和测试之后调用的接口，单体模型，通过app.config配置
    /// </summary>
    public interface ITestPlanSeq
    {
        void PreInitOnce();
        void PreRun();
        void PostRun();
    }
}
