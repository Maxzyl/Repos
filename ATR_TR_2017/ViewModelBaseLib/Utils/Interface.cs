using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ViewModelBaseLib;
using ModelBaseLib;
using System.Security.Cryptography;
using DevExpress.Xpf.Core;
using System.Windows.Markup;
using System.Windows.Data;
using System.Reflection;
using System.Data;
using System.Collections.ObjectModel;
using Symtant.GeneFunLib;
using System.Xml.Linq;
namespace ViewModelBaseLib
{
    public class Interface
    {
        
        //public static Type[] Steptypes = new Type[] { typeof(TestPlan),typeof(ManualConnection),typeof(SParamTestStep),typeof(LoopTestStep),typeof(ParentTestStep), typeof(DCPSTestStep),  typeof(DUTTestStep),  typeof(AvgPowerTestStep),  typeof(PointTestItem),typeof(SignalGeneSetupStep),typeof(HarmTestStep),typeof(HarmTestItem),typeof(SpurTestStep),typeof(CurrentTestStep),typeof(CurrentTestItem),
        //     typeof(NFTestStep),typeof(IMDTestStep),typeof(SParamTestTrace),typeof(XYTestMarker),typeof(DCPSInfo),typeof(LoopParamInfo),typeof(TotalSpecVM),typeof(FormulaCalcTestStep),typeof(CalcPointTestItem),typeof(CalcPointTestTrace),typeof(TempSetStep),typeof(ManualLoopTestStep)};

        // public static Type[] StepDatatypes=new Type[] { typeof(NFCorrData), typeof(double[]), typeof(XYDataArr),typeof(XYData)};

        public static Type[] Steptypes = new Type[] { typeof(TestPlan),typeof(ManualConnection),typeof(LoopTestStep),typeof(ParentTestStep), typeof(PointTestItem),typeof(BoolTestItem),
            typeof(XYTestMarker),typeof(LoopParamInfo),typeof(TotalSpecVM),typeof(FormulaCalcTestStep),typeof(CalcPointTestItem),typeof(CalcPointTestTrace),typeof(ManualLoopTestStep),typeof(LinkTestStep)};


        public static Type[] GetSerialTypeFromAssemble()
        {
            List<Type> types = new List<Type>();
            foreach(var type in Interface.Steptypes)
            {   
                if(!types.Contains(type))
                {
                    types.Add(type);
                }
            }
            foreach(var type in TestStepFactory.StepTypelist)
            {   
                if(!types.Contains(type.Type))
                {
                    types.Add(type.Type);
                }               
            }
            foreach (var type in TestStepFactory.localSettingTypelist)
            {   
                if(!types.Contains(type))
                {
                    types.Add(type);
                }           
            }
            return types.ToArray();
        }
        public static Type[] GetResultTypeFromAssemble()
        {
            List<Type> types = new List<Type>();
            foreach(var type in TestStepFactory.ResultDisplaylist)
            {
                if(!types.Contains(type.ResultType))
                {
                    types.Add(type.ResultType);
                }
            }
            return types.ToArray();
        }

        public static byte[] SerializerStateModel(object vm)
        {
            byte[] result = null;
            MemoryStream ms = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(vm.GetType(), GetSerialTypeFromAssemble());
            serializer.Serialize(ms, vm); 
           
            ms.Flush();
            ms.Position = 0;
            result = new byte[ms.Length];
            ms.Read(result, 0, Convert.ToInt32(ms.Length));
            ms.Close();
            return result;
        }
        public static object DeSerializerStateModel(byte[] data,object vm)
        {
            object result = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(vm.GetType(),GetSerialTypeFromAssemble());
                MemoryStream ms = new MemoryStream(data);
                ms.Position = 0;
                ms.Seek(0, SeekOrigin.Begin);
                result = serializer.Deserialize(ms);
            }
            catch(Exception ex)
            {
                DXMessageBox.Show("请选择合适的状态文件！","提示");
            }
            return result;
        }
        public static void SaveResultVM(object vm)
        {
            byte[] result = null;
            string fileName = @"./chooseResult.xml";
            MemoryStream ms = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(vm.GetType());
            serializer.Serialize(ms, vm);

            ms.Flush();
            ms.Position = 0;
            result = new byte[ms.Length];
            ms.Read(result, 0, Convert.ToInt32(ms.Length));
            ms.Close();
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            System.IO.File.WriteAllBytes(fileName, result);
        }

        public static object DeSerializerResultVM(object vm)
        {
            object result = null;
            try
            {
                string fileName = @"./chooseResult.xml";
                if(!File.Exists(fileName))return result;
                byte[] bytes = System.IO.File.ReadAllBytes(fileName);
                XmlSerializer serializer = new XmlSerializer(vm.GetType());
                MemoryStream ms = new MemoryStream(bytes);
                ms.Position = 0;
                ms.Seek(0, SeekOrigin.Begin);
                result = serializer.Deserialize(ms);
            }
            catch (Exception ex)
            {
                
            }
            return result;
        }

        public static object CopyTestPlanVM(object vm)
        {
            object result = null;
            MemoryStream ms = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(vm.GetType(), GetSerialTypeFromAssemble());
            serializer.Serialize(ms,vm);
            ms.Flush();
            ms.Position = 0;
            result = serializer.Deserialize(ms);
            return result;
        }

        public static byte[] SerializerStepData(List<object> objs)
        {
            byte[] result = null;
            MemoryStream ms = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(objs.GetType(),TestStepFactory.localSettingTypelist.ToArray());
            serializer.Serialize(ms, objs);
            ms.Flush();
            ms.Position = 0;
            result = new byte[ms.Length];
            ms.Read(result, 0, Convert.ToInt32(ms.Length));
            ms.Close();
            return result;
        }

        public static object DeSerializerStepData(byte[] data,List<object> obj)
        {
            object result = null;
            XmlSerializer serializer = new XmlSerializer(obj.GetType(),TestStepFactory.localSettingTypelist.ToArray());
            MemoryStream ms = new MemoryStream(data);
            ms.Position = 0;
            ms.Seek(0, SeekOrigin.Begin);
            result = serializer.Deserialize(ms);
            return result;
        }

        public static void SaveAllLocalSettings(TestPlanVM vm)
        {
            string fileName = Environment.CurrentDirectory + "/configfiles/" + vm.DisplayName + ".xml";
            List<object> objects = new List<object>();
            foreach (var mcNode in vm.ManualConnList)
            {
                foreach (var stepNode in mcNode.SubTreeNodeList)
                {
                    object obj = (stepNode.NodeObj as TestStep).GetLocalSetting();
                    objects.Add(obj);
                }
            }
            byte[] bytes = Interface.SerializerStepData(objects);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            System.IO.File.WriteAllBytes(fileName, bytes);
        }
        
        public static void SaveAllLocalSettings(TestPlan testPlan)
        {

            string fileName = Environment.CurrentDirectory + "/local files/" + testPlan.DisplayName + ".xml";
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }
            List<object> localSettings = new List<object>();

            
            localSettings.Add(testPlan.GuidStr);
            localSettings.Add(DateTime.Now.ToString());
            foreach (var step in TestStep.GetAllTestStep(testPlan))
            {
                localSettings.Add(step.GetLocalSetting());
            }
            byte[] localSettingsbytes = SerializerStepData(localSettings);
            
            System.IO.File.WriteAllBytes(fileName, localSettingsbytes.ToArray());
            
        }
        public static void LoadLocalSettings(TestPlan testPlan)
        {

            string fileName = Environment.CurrentDirectory + "/local files/" + testPlan.DisplayName + ".xml";

            if (File.Exists(fileName))
            {
                try
                {
                    byte[] allBytes = File.ReadAllBytes(fileName);

                    List<object> localSettinngobjects = new List<object>();
                    object obj = Interface.DeSerializerStepData(allBytes, localSettinngobjects);
                    localSettinngobjects = obj as List<object>;
                    string guidStr = localSettinngobjects[0] as string;

                    if (guidStr == testPlan.GuidStr)
                    {
                        List<TestStep> steps = TestStep.GetAllTestStep(testPlan);
                        if (steps.Count == localSettinngobjects.Count - 2)
                        {
                            for (int i = 0; i < localSettinngobjects.Count - 2; i++)
                            {
                                steps[i].SetLocalSetting(localSettinngobjects[i + 2]);
                                steps[i].IsCalValid = true;
                            }
                            return;
                        }
                    }
                    else
                    {
                        var res= DXMessageBox.Show("状态文件已经被修改，本地设置和校准文件是否合法？", "本地设置和校准合法性确认", System.Windows.MessageBoxButton.YesNo);
                        if (res == System.Windows.MessageBoxResult.Yes)
                        {
                            List<TestStep> steps = TestStep.GetAllTestStep(testPlan);
                            if (steps.Count == localSettinngobjects.Count - 2)
                            {
                                for (int i = 0; i < localSettinngobjects.Count - 2; i++)
                                {
                                    steps[i].SetLocalSetting(localSettinngobjects[i + 2]);
                                    steps[i].IsCalValid = true;
                                }
                                SaveAllLocalSettings(testPlan);
                                return;
                            }
                            
                        }
                        else
                        {
                            

                        }
                    }
                }
                catch
                {
                    foreach (var step in TestStep.GetAllTestStep(testPlan))
                    {
                        step.IsCalValid = false;
                    }

                }
            }

            foreach (var step in TestStep.GetAllTestStep(testPlan))
            {
                step.IsCalValid = false;
            }

        }

        public static string GetMD5HashFromByte(byte[] data)
        {
            string str;
            try
            {
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(data, 0, data.Length);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++)
                {
                    builder.Append(buffer[i].ToString("X2"));
                }
                str = builder.ToString();
            }
            catch (Exception exception)
            {
                throw new Exception("GetMD5HashFromByte() fail,error:" + exception.Message);
            }
            return str;
        }
        public static object GetViewModelFromModel(Type modelType)
        {
            string vmTypeName = modelType.Name + "VM";
            var vmType = Type.GetType("ViewModelBaseLib." + vmTypeName + ",ViewModelBaseLib");
            if (vmType != null)
            {
                var res = Activator.CreateInstance(vmType);
                return res;
            }
            else
            {
                return null;
            }
        }
        public static object GetStepViewModelFromModel(Type modelType)
        {
            var stepInfo = TestStepInfoMgr.Instance.TestStepInfoList.Where(x => x.TypeName == modelType.Name).FirstOrDefault();
            TestStepVM stepVM = null;
            if(stepInfo !=null)
            {
                if (stepInfo.TestStepViewModelStr == null || string.IsNullOrWhiteSpace(stepInfo.TestStepViewModelStr)) return stepVM;
                string viewModelStr = stepInfo.TestStepViewModelStr;
                string[] strs = viewModelStr.Split(';');
                string fileName = AppDomain.CurrentDomain.BaseDirectory + strs[0];
                viewModelStr = viewModelStr.Remove(0, strs[0].Length + 1);
                Assembly ass = Assembly.LoadFile(fileName);
                Type typeVieModel = ass.GetType(viewModelStr);
                stepVM = Activator.CreateInstance(typeVieModel) as TestStepVM;
            }
            return stepVM;
        }
        public static object GetItemViewModelFromModel(Type modelType)
        {
            var itemInfo = TestStepFactory.ItemTypelist.Where(x=>x.Type.FullName==modelType.FullName).FirstOrDefault();
            TestItemVM itemVM = null;
            if(itemInfo !=null)
            {
                if(itemInfo.Att.ItemViewModelTypeStr==null || string.IsNullOrWhiteSpace(itemInfo.Att.ItemViewModelTypeStr))return itemVM;
                string viewModelStr = itemInfo.Att.ItemViewModelTypeStr;
                string[] strs = viewModelStr.Split(';');
                string fileName=AppDomain.CurrentDomain.BaseDirectory + strs[0];
                viewModelStr = viewModelStr.Remove(0,strs[0].Length + 1);
                Assembly ass = Assembly.LoadFile(fileName);
                Type typeViewModel = ass.GetType(viewModelStr);
                itemVM = Activator.CreateInstance(typeViewModel) as TestItemVM;
            }
            return itemVM;
        }

        public static object GetStepUserControlFromModel(TestStep step)
        {
            var stepInfo = TestStepInfoMgr.Instance.TestStepInfoList.Where(x => x.TypeName == step.GetType().Name).FirstOrDefault();
            object obj = null;
            if(stepInfo !=null)
            {
                if (stepInfo.AdvUserControlStr == null || string.IsNullOrWhiteSpace(stepInfo.AdvUserControlStr)) return obj;
                string userControlStr = stepInfo.AdvUserControlStr;
                string[] strs = userControlStr.Split(';');
                string fileName = AppDomain.CurrentDomain.BaseDirectory + strs[0];
                userControlStr = userControlStr.Remove(0, strs[0].Length + 1);
                Assembly ass = Assembly.LoadFile(fileName);
                Type userControlType = ass.GetType(userControlStr);
                obj = Activator.CreateInstance(userControlType);
            }
            return obj;
        }
        public static object GetItemUserControlFromModel(Type item)
        {
            var itemInfo = TestStepFactory.ItemTypelist.Where(x => x.Type.FullName == item.FullName).FirstOrDefault();
            object obj = null;
            if(itemInfo !=null)
            {
                if(itemInfo.Att.UserControlType==null || string.IsNullOrWhiteSpace(itemInfo.Att.UserControlType))return obj;
                string userControlStr = itemInfo.Att.UserControlType;
                string[] strs = userControlStr.Split(';');
                string fileName=AppDomain.CurrentDomain.BaseDirectory + strs[0];
                userControlStr = userControlStr.Remove(0,strs[0].Length + 1);
                Assembly ass = Assembly.LoadFile(fileName);
                Type userControlType = ass.GetType(userControlStr);
                obj = Activator.CreateInstance(userControlType);
            }
            return obj;
        }
        public static Dictionary<object, object> ViewModelDict = new Dictionary<object, object>();
        public static ManualConnectionVM GetManualConntionVM(ManualConnection mc)
        {
            if (ViewModelDict.Keys.Contains(mc))
            {
                return ViewModelDict[mc] as ManualConnectionVM;
            }
            else
            {
                ManualConnectionVM vm = GetViewModelFromModel(mc.GetType()) as ManualConnectionVM;
                if (vm != null)
                {
                    vm.ManualConn = mc;
                }
                ViewModelDict.Add(mc,vm);
                return vm;
            }
        }
        public static TestStepVM GetTestStepVMFromViewModel(TestStep step)
        {
            if (ViewModelDict.Keys.Contains(step))
            {
                return ViewModelDict[step] as TestStepVM;
            }
            else
            {
                 TestStepVM vm = GetViewModelFromModel(step.GetType()) as TestStepVM;
                if (vm != null)
                {
                    vm.TestStep = step;
                    ViewModelDict.Add(step, vm);
                }
                else
                {
                    vm = new TestStepVM();
                    vm.TestStep = step;
                    ViewModelDict.Add(step, vm);
                }

                return vm;
            }
        }
        /// <summary>
        /// 获得step的ViewModel，如果在测试步骤信息中指定的ViewModel，使用该ViewModel，否则使用基类ViewModel
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public static TestStepVM GetTestStepVM(TestStep step)
        {
            if (ViewModelDict.Keys.Contains(step))
            {
                return ViewModelDict[step] as TestStepVM;
            }
            else
            {
                TestStepVM vm = GetViewModelFromModel(step.GetType()) as TestStepVM;
                TestStepVM vm2 = GetStepViewModelFromModel(step.GetType()) as TestStepVM;
                TestStepVM vmReal = new TestStepVM();
                if(vm !=null)
                {
                    vm.TestStep = step;
                    vmReal = vm;
                }
                else if (vm2 != null)
                {
                    vm2.TestStep = step;
                    vmReal = vm2;
                }
                else
                {
                    vmReal = new TestStepVM();
                    vmReal.TestStep = step;
                }
                ViewModelDict.Add(step,vmReal);
                return vmReal;
                //if (vm != null)
                //{
                //    vm.TestStep = step;
                //    //vm.DisplayName = GetStepTypeName(step.GetType());
                //    ViewModelDict.Add(step, vm);
                //}
                //else if(vm2 !=null)
                //{
                //    vm2.TestStep = step;
                //    ViewModelDict.Add(step,vm2);
                //}
                //else
                //{
                //    vm = new TestStepVM();
                //    vm.TestStep = step;
                //    ViewModelDict.Add(step, null);
                //    //vm.DisplayName = GetStepTypeName(step.GetType());
                //}
                
                //return vm;
            }
        }
        public static TestItemVM GetTestItemVM(TestItem tr)
        {
            if (ViewModelDict.Keys.Contains(tr))
            {
                return ViewModelDict[tr] as TestItemVM;
            }
            else
            {
               // var res = Interface.GetViewModelFromModel(tr.GetType());
                var res = Interface.GetItemViewModelFromModel(tr.GetType());
                if (res == null)
                {
                    if (tr is PointTestItem)
                    {
                        PointTestItemVM ptVM = new PointTestItemVM();
                        ptVM.TestItem = tr;
                        ViewModelDict.Add(tr, ptVM);
                        return ptVM;
                    }
                    if (tr is TestTrace)
                    {
                        TestTraceVM trVM = new TestTraceVM();
                        trVM.TestItem = tr;
                        ViewModelDict.Add(tr, trVM);
                        return trVM;
                    }
                    if (tr is TRTestItem)
                    {
                        TRTestItemVM trTestVM = new TRTestItemVM();
                        trTestVM.TestItem = tr;
                        ViewModelDict.Add(tr, trTestVM);
                        return trTestVM;
                    }
                    return null;
                }
                else
                {
                    TestItemVM trVM = res as TestItemVM;
                    trVM.TestItem = tr;
                    ViewModelDict.Add(tr, trVM);
                    return trVM;
                }
                
                //if (trVM != null)
                //{
                //    trVM.TestItem = tr;
                //}
                //else
                //{
                //    trVM = new TestItemVM();
                //}
                //ViewModelDict.Add(tr, trVM);
                //return trVM;
            }
        }
        public static XYTestMarkerVM GetTestMarkerVM(XYTestMarker mk)
        {
            if (ViewModelDict.Keys.Contains(mk))
            {
                return ViewModelDict[mk] as XYTestMarkerVM;
            }
            else
            {
                XYTestMarkerVM vm = new XYTestMarkerVM();
                vm.Marker = mk;
                ViewModelDict.Add(mk, vm);
                return vm;
            }
        }
        public static string GetStepTypeName(Type stepType)
        {
            
            string typeName = stepType.Name;
            var stepInfo = TestStepInfoMgr.Instance.TestStepInfoList.Where(x => x.TypeName == typeName).FirstOrDefault();
            if (stepInfo != null)
            {
                return stepInfo.DisplayName;
            }
            else
                return null;
        }
        public static void ExportDataTableToCSV(string fileName, DataTable dt)
        {   
           // string csvStr = ConvertDataTable(dt);
           // if(string.IsNullOrWhiteSpace(csvStr))return;
            using(FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                //byte[] bytes = new byte[] { 0xEF,0xBB, 0xBF};
                //bytes = System.Text.Encoding.Default.GetBytes(csvStr.ToCharArray(), 0 ,csvStr.Length);
                //fs.Write(bytes, 0 ,csvStr.Length);
                //fs.Flush();
                StreamWriter sw = new StreamWriter(new BufferedStream(fs), System.Text.Encoding.Default);
                string csvStr = "";
                foreach (DataColumn column in dt.Columns)
                {
                    csvStr += column.ColumnName + ",";
                }
                csvStr = csvStr.Substring(0, csvStr.Length - 1) + "\n";
                sw.Write(csvStr);
                foreach (DataRow row in dt.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;
                    string line = "";
                    foreach (DataColumn column in dt.Columns)
                    {
                        line += row[column].ToString() + ",";
                    }
                    line = line.Substring(0, line.Length - 1) + "\n";
                    sw.Write(line);
                }
                sw.Close();
                fs.Close();
            }
        }

        private static string ConvertDataTable(DataTable dt)
        { 
            if(dt.Rows.Count == 0) return "";
            string csvStr = "";
            foreach(DataColumn column in dt.Columns)
            {
                csvStr += column.ColumnName + ",";
            }
            csvStr = csvStr.Remove(csvStr.LastIndexOf(","),1);
            csvStr += "\n";
            foreach(DataRow row in dt.Rows)
            {
                foreach(DataColumn column in dt.Columns)
                {
                    csvStr += row[column].ToString() + ",";
                }
                csvStr = csvStr.Remove(csvStr.LastIndexOf(","), 1);
                csvStr += "\n";
            }
            return csvStr;

        }

        public static DataTable GetDataTableFromCSV(string fileName, ObservableCollection<LoopParamInfo> paramInfos)
        {
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(fileName,System.IO.FileMode.Open,System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs,Encoding.Default);
            string strline = "";
            string[] aryline = null;
            string[] tableHead = null;
            int columnCount = 0;
            bool IsFirst = true;
            while((strline=sr.ReadLine()) !=null)
            {
                if (IsFirst == true)
                {
                    tableHead = strline.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    { 
                        DataColumn dc = new DataColumn(tableHead[i]);
                        int count = paramInfos.Where(x => x.DisplayName == dc.ColumnName && x.IsChecked == true).Count();
                        if (count == 0) return null;
                        var item = paramInfos.Where(x => x.DisplayName == dc.ColumnName && x.IsChecked == true).FirstOrDefault();
                        dc.DataType = item.PropertyType;
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryline = strline.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount;j++ )
                    {
                        if (aryline[j].ToNullDouble() != null)
                        {
                            dr[j] = aryline[j];
                        }
                        else
                        {
                            return dt;
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            sr.Close();
            fs.Close();
            return dt;
        }
        public static bool IsTestPlanEqual(TestPlan plan1, TestPlan plan2)
        {
            //bool bEquals = true;
            byte[] bytes1 = Interface.SerializerStateModel(plan1);
            byte[] bytes2 = Interface.SerializerStateModel(plan2);
            
            return GeneFun.ArraysEqual<byte>(bytes1, bytes2);
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

        public static List<LoopTestStep> GetAllLoopTestStep(LoopTestStep loopTestStep)
        {
            List<LoopTestStep> stepList = new List<LoopTestStep>();
            foreach(var step in loopTestStep.ChildTestStepList)
            {
                if (step is LoopTestStep)
                {
                    stepList.Add(step as LoopTestStep);
                    foreach(var subStep in (step as LoopTestStep).ChildTestStepList)
                    {
                        if(subStep is LoopTestStep)
                        {
                            var subSteps = GetAllLoopTestStep(subStep as LoopTestStep);
                            stepList.AddRange(subSteps);
                        }
                    }
                }
            }
            return stepList;
        }

        public static List<TreeNodeVM> GetAllSubTreeNodeVM(TreeNodeVM node)
        { 
            if(node.NodeObj is ParentTestStep)
            {
                List<TreeNodeVM> nodeList = new List<TreeNodeVM>();
                foreach(var subNode in node.SubTreeNodeList)
                {
                    var subNodes = GetAllSubTreeNodeVM(subNode);
                    nodeList.AddRange(subNodes);
                }
                return nodeList;
            }
            else
            {
                List<TreeNodeVM> nodeList = new List<TreeNodeVM>();
                nodeList.Add(node);
                return nodeList;
            }
        }
    }
    public class ShowExportInfo : MarkupExtension, IValueConverter
    {

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = "";
            if (System.Convert.ToInt32(value) == 0)
            {
                result = "正在校准...";
            }
            else
            {
                result = "校准完成！";
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
