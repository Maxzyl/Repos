using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using Symtant.InstruDriver;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Xml.Linq;
using DevExpress.Xpf.Core;
using System.Collections.Concurrent;
using System.Windows;
using System.Collections;

namespace ModelBaseLib
{
    public class TestStepType
    {
        public const string PIM = "互调测试";
        public const string SParam = "S参数测试";
        public const string IP3 = "交调测试";
        public const string NF = "噪声系数测试";
        public const string DCPS = "电源设置";
        public const string DUT = "被测件设置";
        public const string DUTInfo = "被测件信息读取";
        public const string ASGSetup = "模拟信号源设置";
        public const string Loop = "循环测试";
        public const string AvgPower = "点频平均功率测试";
        public const string Cal = "计算公式";
        public const string Spur = "杂散测试";
        public const string Harm = "谐波测试";
        public const string Manual = "手动循环测试";
        public const string Link = "绑定步骤";
        public const string Switch = "路径设置";
    }
    public class TestStepFactory
    {

        public static void InitTestStepInfo()
        {   
            
            List<TestStepInfo> stepInfoList = new List<TestStepInfo>();
            List<TestStepInfo> stepInfoListFromXml = GetTestStepInfoFromXml();
            getTypeFromAssemble();
            foreach (var type in StepTypelist)
            {
                Assembly assembly = Assembly.LoadFile(type.AssemblyPath);
                string str = type.Type.FullName;
                Type modelType = assembly.GetType(str);
                TestStep step = Activator.CreateInstance(modelType) as TestStep;
                if(step !=null)
                {
                    TestStepInfo stepInfo = new TestStepInfo() 
                    {    
                         Assembly=assembly, 
                         TypeName=step.GetType().Name,
                         StepType=step.GetType(),
                         DisplayName=type.Att.DisplayName,
                         TestStepViewModelStr=type.Att.StepTypeStr,
                         AdvUserControlStr = type.Att.UserControlType,
                         TestTraceTypeList=step.ItemTypeNameList,
                         IsFixedItem=type.Att.IsFixedItem,
                         IsNeedCal=type.Att.IsNeedCal
                    };
                    if (stepInfoListFromXml.Where(x => x.TypeName == stepInfo.TypeName).Count() > 0)
                    {
                        //if (step.SupportedMeasClsInfoList != null)
                        //{
                        //    stepInfo.MeasClsInfoList = step.SupportedMeasClsInfoList.ToList();
                        //}
                        //else
                        //{
                        stepInfo.MeasClsInfoList = stepInfoListFromXml.Where(x => x.TypeName == stepInfo.TypeName).FirstOrDefault().MeasClsInfoList;
                        //}
                        stepInfo.Settings = stepInfoListFromXml.Where(x => x.TypeName == stepInfo.TypeName).FirstOrDefault().Settings;
                        stepInfo.LocalSettingViewStr = stepInfoListFromXml.Where(x => x.TypeName == stepInfo.TypeName).FirstOrDefault().LocalSettingViewStr;
                        stepInfoList.Add(stepInfo);
                    }
                    //else 
                    //{
                    //    if (step.SupportedMeasClsInfoList != null)
                    //    {
                    //        stepInfo.MeasClsInfoList = step.SupportedMeasClsInfoList.ToList();
                    //    }
                    //}

                   // stepInfoList.Add(stepInfo);
                    if (step.LocalSettingTypeList != null)
                    {
                        foreach (var itemType in step.LocalSettingTypeList)
                        {
                            if (!localSettingTypelist.Contains(itemType))
                            {
                                localSettingTypelist.Add(itemType);
                            }
                        }
                    }
                }
            }
            stepInfoList.Add(new TestStepInfo() { TypeName = "FormulaCalcTestStep" ,DisplayName=TestStepType.Cal});
            stepInfoList.Add(new TestStepInfo() { TypeName = "LoopTestStep" ,DisplayName=TestStepType.Loop});
            stepInfoList.Add(new TestStepInfo() { TypeName = "ManualLoopTestStep" ,DisplayName=TestStepType.Manual});
            stepInfoList.Add(new TestStepInfo() { TypeName = "LinkTestStep", DisplayName=TestStepType.Link});
            
            TestStepInfoMgr.Instance.TestStepInfoList = stepInfoList.ToArray();       
            TestStepInfoMgr.Instance.LoadInstruInfoFromFile();
        }
        //从XML文件中读取MeasClsInfo的信息
        public static List<TestStepInfo> GetTestStepInfoFromXml()
        {
            
            List<TestStepInfo> stepInfoListFromXml = new List<TestStepInfo>();
            try
            {
                XElement rooEl = XElement.Load(@"./configfiles/MeasClsInfo.xml");
                
                var xmlStepInfoList = rooEl.Elements("TestStepInfo");
                foreach (var xmlstepInfo in xmlStepInfoList)
                {
                    string typeName = GetAttibute(xmlstepInfo, "TypeName");
                    string displayName = GetAttibute(xmlstepInfo, "DisplayName");
                    TestStepInfo stepInfo = new TestStepInfo() { TypeName = typeName, DisplayName = displayName };
                    try
                    {
                        var xmlMeasClsInfolist = xmlstepInfo.Elements("MeasClsInfo");
                        foreach (var xmlMeasClsInfo in xmlMeasClsInfolist)
                        {
                            string measTypeName = GetAttibute(xmlMeasClsInfo, "TypeName");
                            string measDisplayName = GetAttibute(xmlMeasClsInfo, "DisplayName");
                            string measInfo = GetAttibute(xmlMeasClsInfo, "Info");
                            MeasClsInfo measClsInfo = new MeasClsInfo() { TypeName = measTypeName, DisplayName = measDisplayName, Info = measInfo };
                            stepInfo.MeasClsInfoList.Add(measClsInfo);
                            //添加所有InstruInfo配置
                            var xmlInstruInfolist = xmlMeasClsInfo.Elements("InstruInfo");
                            List<InstruInfo> instruInfolist = new List<InstruInfo>();
                            foreach (var xmlInstruInfo in xmlInstruInfolist)
                            {
                                string instruName = GetAttibute(xmlInstruInfo, "Name");
                                string instruDispalyName = GetAttibute(xmlInstruInfo, "DisplayName");
                                bool isManualInit = false;
                                try
                                {
                                    string isManualInitStr = GetAttibute(xmlInstruInfo, "IsManualInit");
                                    isManualInit = Symtant.GeneFunLib.StringExtensions.ScpiResToBool(isManualInitStr);
                                }
                                catch
                                {
                                }
                                bool isCal = false;
                                try
                                {
                                    string isCalStr = GetAttibute(xmlInstruInfo, "IsCal");
                                    isCal = Symtant.GeneFunLib.StringExtensions.ScpiResToBool(isCalStr);
                                }
                                catch
                                {
                                }
                                InstruInfo instruInfo = new InstruInfo() { Name = instruName, DisplayName = instruDispalyName, IsManuallyInit = isManualInit, IsCal = isCal };
                                instruInfolist.Add(instruInfo);
                            }
                            measClsInfo.InstruInfoList = instruInfolist.ToArray();
                            //添加所有ConfigInfo配置
                            var xmlConfiglist = xmlMeasClsInfo.Elements("ConfigInfo");
                            List<PathConfigInfo> configList = new List<PathConfigInfo>();
                            foreach (var xmlConfig in xmlConfiglist)
                            {
                                string configName = GetAttibute(xmlConfig, "Name");
                                string configValue = GetAttibute(xmlConfig, "Path");
                                string configInfo = GetAttibute(xmlConfig, "Info");
                                PathConfigInfo pathConfigInfo = new PathConfigInfo() { Name = configName, Path = configValue, Info = configInfo };

                                var xmlConfigSettings = xmlConfig.Elements("set");
                                foreach (var xmlConfigSet in xmlConfigSettings)
                                {
                                    var k = xmlConfigSet.Attribute("key");
                                    if (k != null && k.Value != null)
                                    {
                                        var v = xmlConfigSet.Attribute("value");
                                        if (v != null && v.Value != null)
                                        {
                                            pathConfigInfo.Settings.Add(k.Value.Trim(), v.Value.Trim());
                                        }
                                    }
                                }
                                configList.Add(pathConfigInfo);
                            }
                            measClsInfo.PathConfigInfoList = configList.ToArray();
                            //添加所有其他配置
                            measClsInfo.Settings.Clear();
                            var xmlClsSettings = xmlMeasClsInfo.Elements("set");
                            foreach (var xmlClsSet in xmlClsSettings)
                            {
                                var k = xmlClsSet.Attribute("key");
                                if (k != null && k.Value != null)
                                {
                                    var v = xmlClsSet.Attribute("value");
                                    if (v != null && v.Value != null)
                                    {
                                        measClsInfo.Settings.Add(k.Value.Trim(), v.Value.Trim());
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        stepInfo.MeasClsInfoList = null;
                    }
                    try
                    {
                        stepInfo.Settings.Clear();
                        var xmlStepSettings = xmlstepInfo.Elements("set");
                        foreach (var xmlStepSet in xmlStepSettings)
                        {
                            var k = xmlStepSet.Attribute("key");
                            if (k != null && k.Value != null)
                            {
                                var v = xmlStepSet.Attribute("value");
                                if (v != null && v.Value != null)
                                {
                                    stepInfo.Settings.Add(k.Value.Trim(), v.Value.Trim());
                                }
                            }
                        }
                    }
                    catch
                    { stepInfo.Settings = null; }
                    try
                    {
                        var xmlLocalSettingViewStr = xmlstepInfo.Element("LocalSettingView");
                        stepInfo.LocalSettingViewStr = GetAttibute(xmlLocalSettingViewStr,"value");
                    }
                    catch
                    {
                        stepInfo.LocalSettingViewStr = null;
                    }
                    stepInfoListFromXml.Add(stepInfo);
                }
            }
            catch
            {

            }
            return stepInfoListFromXml;
        }
        public static string GetAttibute(XElement ele, string attibuteName)
        {
            var att = ele.Attribute(attibuteName);
            return att == null ? null : att.Value;
        }
        public static string[] GetTestStepDisplayNameList()
        {
            return TestStepInfoMgr.Instance.TestStepInfoList.Select(x => x.DisplayName).ToArray();
        }
        public static TestStep CreateTestStep(string stepDisplayName)
        {
            var stepInfo=TestStepInfoMgr.Instance.TestStepInfoList.Where(x => x.DisplayName == stepDisplayName).FirstOrDefault();           
            TestStep step = null;
            if (stepDisplayName == TestStepType.Loop)
            {
                step = new LoopTestStep();
            }
            else if (stepDisplayName == TestStepType.Manual)
            {
                step = new ManualLoopTestStep();
            }
            else if(stepDisplayName==TestStepType.Cal)
            {
                step = new FormulaCalcTestStep();
            }
            else if(stepDisplayName==TestStepType.Link)
            {
                step = new LinkTestStep();
            }
            else
            {
                Type stepType = stepInfo.Assembly.GetType(stepInfo.StepType.ToString());
                step = Activator.CreateInstance(stepType) as TestStep;
            }
           
            return step;
        }
        public static string getDisplayName(string step)
        {
            string displayName = TestStepInfoMgr.Instance.TestStepInfoList.Where(x => x.TypeName == step).Select(x => x.DisplayName).FirstOrDefault();
            return displayName;
        }
        public static List<StepTypeInfo> StepTypelist = new List<StepTypeInfo>();
        public static List<ItemTypeInfo> ItemTypelist = new List<ItemTypeInfo>();
        public static List<UITypeInfo> UITypelist = new List<UITypeInfo>();
        public static List<Type> localSettingTypelist = new List<Type>();
        public static List<UserControl> CalibUClist = new List<UserControl>();
        public static List<ResultDisplayType> ResultDisplaylist = new List<ResultDisplayType>();
        public static void getTypeFromAssemble()
        {
            StepTypelist.Clear();
            ItemTypelist.Clear();
            CalibUClist.Clear();
            string[] resultStrs = DataUtils.StaticInfo.ResultString.Split(',');
            string folderName = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo info = new DirectoryInfo(folderName);
            string fileName;
            foreach (FileInfo file in info.GetFiles())
            {
                fileName = file.FullName.ToLower();
                if (fileName.EndsWith(".dll"))
                {
                    try
                    {
                        Assembly ass = Assembly.LoadFile(fileName);
                        Type[] ts = ass.GetTypes();
                        foreach (Type type in ts)
                        {
                            var attStep = type.GetCustomAttributes(typeof(ModelBaseLib.UITestStepParaAttribute), false).FirstOrDefault() as ModelBaseLib.UITestStepParaAttribute;
                            if (attStep != null && type.IsSubclassOf(typeof(TestStep)))
                            {
                                StepTypelist.Add(new StepTypeInfo() { Type = type, Att = attStep, AssemblyPath = fileName });
                            }
                            var attTrace = type.GetCustomAttributes(typeof(ModelBaseLib.UITestItemParaAttribute), false).FirstOrDefault() as ModelBaseLib.UITestItemParaAttribute;
                            if (attTrace != null && type.IsSubclassOf(typeof(TestItem)))
                            {
                                ItemTypelist.Add(new ItemTypeInfo() { Type = type, Att = attTrace, AssemblyPath = fileName });
                            }
                            var attUI = type.GetCustomAttributes(typeof(ModelBaseLib.UIUserControlParaAttribute), false).FirstOrDefault() as ModelBaseLib.UIUserControlParaAttribute;
                            if (attUI != null)
                            {
                                UITypelist.Add(new UITypeInfo() { Type = type, Att = attUI, AssemblePath = fileName });
                            }
                             var att = type.GetCustomAttributes(typeof(ModelBaseLib.UIDisplayParaAttribute), false).FirstOrDefault() as ModelBaseLib.UIDisplayParaAttribute;
                             if (att != null && type.GetInterfaces().Contains(typeof(IResultListerner)))
                             {
                                 string str = (file.Name + ";" + type.FullName).ToUpper();
                                 bool exists = ((IList)resultStrs).Contains(str);
                                 if(exists)
                                 {
                                     ResultDisplaylist.Add(new ResultDisplayType() { ResultType = type, Att = att, AssemblyPath = fileName });
                                 }
                             }
                            var attCal = type.GetCustomAttributes(typeof(ModelBaseLib.UICalibrationParaAttribute),false).FirstOrDefault() as ModelBaseLib.UICalibrationParaAttribute;
                            if(attCal !=null)
                            {
                                object obj = Activator.CreateInstance(type) as object;
                                if (obj is UserControl)
                                {
                                    UserControl uc = obj as UserControl;
                                    //if (attCal.DisplayName.ToUpper() == "LAST")
                                    //{
                                    //    CalibUClist.Add(uc);
                                    //}
                                    //else if (attCal.DisplayName.ToUpper() == "SELECT")
                                    //{
                                    //    int lastUIIndex = CalibUClist.FindIndex(x => x.GetType().Name == "CalibrationContentUC");
                                    //    if (lastUIIndex > -1)
                                    //    {
                                    //        CalibUClist.Insert(CalibUClist.Count - 1, uc);
                                    //    }
                                    //    else
                                    //    {
                                    //        CalibUClist.Add(uc);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    CalibUClist.Insert(0, uc);
                                    //}
                                    CalibUClist.Add(uc);
                                }
                               // CalibUClist.Add(new UITypeInfo() {Type=type,AssemblePath=fileName });
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        DataUtils.LOGINFO.WriteLog(fileName + "can't be loaded");
                    }
                }
            }
           // return StepTypelist.ToArray();
            
        }
        public static List<TestStep> GetAllSubTestStep(TestStep step)
        {
            if (step is ParentTestStep)
            {
                List<TestStep> stepList = new List<TestStep>();
                foreach (var subStep in (step as ParentTestStep).ChildTestStepList)
                {
                    var subSteps = GetAllSubTestStep(subStep);
                    stepList.AddRange(subSteps);
                }
                return stepList;
            }
            else
            {
                List<TestStep> stepList = new List<TestStep>();
                stepList.Add(step);
                return stepList;
            }
        }
    }

    public class Blockinfo
    {
        public TestStep Step { get; set; }
        public int SpecIndex { get; set; }
        public Size Size { get; set; }
    }

    public class ResultDisplayType
    {
        public Type ResultType { get; set; }
        public UIDisplayParaAttribute Att { get; set; }
        public string AssemblyPath { get; set; }
    }

    public class StepTypeInfo
    {
        public Type Type { get; set; }
        public UITestStepParaAttribute Att { get; set; }
        public string AssemblyPath { get; set; }
    }
    public class ItemTypeInfo
    {
        public Type Type { get; set; }
        public UITestItemParaAttribute Att { get; set; }
        public string AssemblyPath { get; set; }
    }
    public class UITypeInfo
    {
        public Type Type { get; set; }
        public UIUserControlParaAttribute Att { get; set; }
        public string AssemblePath { get; set; }
    }
 
}
