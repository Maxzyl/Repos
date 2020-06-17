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

namespace TestModelLib
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

    }
    public class TestStepFactory
    {
        public static void InitTestStepInfo()
        {
            List<TestStepInfo> stepInfoList = new List<TestStepInfo>();
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
                         IsFixedItem=type.Att.IsFixedItem
                    };
                    if(step.SupportedMeasClsInfoList !=null)
                    {
                        stepInfo.MeasClsInfoList = step.SupportedMeasClsInfoList.ToList();
                    }
                    stepInfoList.Add(stepInfo);
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

            TestStepInfoMgr.Instance.TestStepInfoList = stepInfoList.ToArray();       
            TestStepInfoMgr.Instance.LoadInstruInfoFromFile();
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
        public static void getTypeFromAssemble()
        {
            StepTypelist.Clear();
            ItemTypelist.Clear();
            string folderName = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo info = new DirectoryInfo(folderName);
            string fileName;
            foreach (FileInfo file in info.GetFiles())
            {
                fileName = file.FullName.ToLower();
                if (fileName.EndsWith(".dll"))
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
                        var attTrace = type.GetCustomAttributes(typeof(ModelBaseLib.UITestItemParaAttribute),false).FirstOrDefault() as ModelBaseLib.UITestItemParaAttribute;
                        if(attTrace !=null && type.IsSubclassOf(typeof(TestItem)))
                        {
                            ItemTypelist.Add(new ItemTypeInfo() {Type=type,Att=attTrace,AssemblyPath=fileName });
                        }
                        var attUI = type.GetCustomAttributes(typeof(ModelBaseLib.UIUserControlParaAttribute),false).FirstOrDefault() as ModelBaseLib.UIUserControlParaAttribute;
                        if(attUI !=null && type.IsSubclassOf(typeof(UserControl)))
                        {
                            UITypelist.Add(new UITypeInfo() {Type= type,Att=attUI,AssemblePath=fileName});
                        }
                    }
                }
            }
           // return StepTypelist.ToArray();
        }
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
