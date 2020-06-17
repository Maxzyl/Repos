using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Reflection;
using System.IO;
using System.Windows.Input;
namespace ViewModelBaseLib
{
    public class TestPlanLocalSettingVM:NotifyBase
    {
        public TestPlanLocalSettingVM()
        {
            manualConnList.CollectionChanged += manualConnList_CollectionChanged;
            AllNodeList = new ObservableCollection<TestStep>();
            FilterUCList = new ObservableCollection<UCLocalSetting>();
        }

        void manualConnList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
        }
        public ObservableCollection<UCLocalSetting> FilterUCList { get; set; }
        private ObservableCollection<UCLocalSetting> uCLocalSettingList = new ObservableCollection<UCLocalSetting>();
        public ObservableCollection<UCLocalSetting> UCLocalSettingList
        {
            get
            {
                return uCLocalSettingList;
            }
            set
            {
                uCLocalSettingList = value;
            }
        }
        private ObservableCollection<TreeNodeVM> manualConnList = new ObservableCollection<TreeNodeVM>();
        public ObservableCollection<TreeNodeVM> ManualConnList
        {
            get
            {
                ObservableCollection<TreeNodeVM> treeNodelist = new ObservableCollection<TreeNodeVM>();
                foreach (var mc in TestPlanManager.CurrentTestPlan.ManualConnectionList)
                {
                    TreeNodeVM mcTreeNode = new TreeNodeVM() { NodeObj =mc , IsSelected = true };
                    mcTreeNode.SubTreeNodeList.Clear();
                    foreach (var step in mc.TestStepList)
                    {
                        foreach (var realStep in step.GetAllSubTestStep())
                        {

                            TreeNodeVM realTreeStepNode = new TreeNodeVM() { NodeObj = realStep, IsSelected = true, ParentNode = mcTreeNode };
                            mcTreeNode.SubTreeNodeList.Add(realTreeStepNode);
                            AddUCLocalSetting(realStep,realTreeStepNode.Name);
                        }
                    }
                    if (mcTreeNode.SubTreeNodeList.Count() > 0)
                    {
                        treeNodelist.Add(mcTreeNode);
                    }
                }
                manualConnList = treeNodelist;
                AllNodeList = new ObservableCollection<TestStep>();
                foreach (var mcNode in treeNodelist)
                {
                    mcNode.PropertyChanged += item_PropertyChanged;
                    foreach (var stepNode in mcNode.SubTreeNodeList)
                    {
                        stepNode.PropertyChanged += item_PropertyChanged;
                        AllNodeList.Add(stepNode.NodeObj as TestStep);
                    }
                }

                getSelectedUC();
                return treeNodelist;
            }
        }
        private void AddUCLocalSetting(TestStep step,string displayName)
        {
            var item = TestStepInfoMgr.Instance.TestStepInfoList.Where(x => x.StepType == step.GetType()).Select(x => x.LocalSettingViewStr).FirstOrDefault();
            var temp = TestStepInfoMgr.Instance.TestStepInfoList.Where(x => x.StepType == step.GetType());
            if(item != null)
            {
                string folderName = AppDomain.CurrentDomain.BaseDirectory;
                string[] strs = item.Split(';');
                string fullName = folderName + strs[0];
                Assembly assembly = Assembly.LoadFile(fullName);
                Type modelType = assembly.GetType(strs[1]);
                object obj = Activator.CreateInstance(modelType) as object;
                UserControl uc = obj as UserControl;
                uc.DataContext = step;
                if(uc !=null)
                {
                    UCLocalSetting ucLocalSetting = new UCLocalSetting() { StepDisplayName = displayName, UserControl = uc, DisplayName = displayName+"         "+step.PathConfigName };
                    uCLocalSettingList.Add(ucLocalSetting);
                }
            }
        }

        void item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {   
            switch (e.PropertyName)
            {
                case "IsSelected":
                    if ((sender as TreeNodeVM).NodeObj is ManualConnection)
                    {
                        foreach (var stepNode in (sender as TreeNodeVM).SubTreeNodeList)
                        {
                            stepNode.PropertyChanged -= item_PropertyChanged;
                            stepNode.IsSelected = (sender as TreeNodeVM).IsSelected;
                            stepNode.PropertyChanged += item_PropertyChanged;
                        }
                    }
                    else
                    {
                        TreeNodeVM subNode = sender as TreeNodeVM;
                        if (subNode.IsSelected == true)
                        {
                            if (subNode.ParentNode.SubTreeNodeList.Where(x => x.IsSelected == false).Count() == 0)
                            {
                                subNode.ParentNode.PropertyChanged -= item_PropertyChanged;
                                subNode.ParentNode.IsSelected = true;
                                subNode.ParentNode.PropertyChanged += item_PropertyChanged;
                            }

                        }
                        else
                        {
                            if (subNode.ParentNode.SubTreeNodeList.Where(x => x.IsSelected == true).Count() < subNode.ParentNode.SubTreeNodeList.Count())
                            {
                                subNode.ParentNode.PropertyChanged -= item_PropertyChanged;
                                subNode.ParentNode.IsSelected = false;
                                subNode.ParentNode.PropertyChanged += item_PropertyChanged;
                            }
                        }
                    }
                    break;
            }
            getSelectedUC();
        }
        void getSelectedUC()
        {
            FilterUCList.Clear();
            foreach(TreeNodeVM node in manualConnList)
            {
                foreach(TreeNodeVM subNode in node.SubTreeNodeList)
                {
                    if(subNode.IsSelected)
                    {
                        var item = UCLocalSettingList.Where(x => x.StepDisplayName == subNode.Name).FirstOrDefault();
                        if(item != null)
                        {
                            UCLocalSetting uc = item as UCLocalSetting;
                            FilterUCList.Add(uc);
                        }
                    }
                }
            }
        }
        
        public ObservableCollection<TestStep> AllNodeList { get; set; }

        //private object GetDataContext(TestStep step)
        //{
        //    if (step == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        var stepInfo=TestStepInfoMgr.GetStepInfo(step.GetType().Name);
        //        if (stepInfo == null)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //}
        

        public void OnViewLoad()
        {
            TestPlanManager.CurrentTestPlan.InitAllCal();
        }

        public void SaveAllSettings()
        {
            
            TestPlanManager.CurrentTestPlan.SaveAllCal();
            Interface.SaveAllLocalSettings(TestPlanManager.CurrentTestPlan);
        }
    }
    public class UCLocalSetting : NotifyBase
    {
        private string _StepDisplayName;
        private const string StepDisplayNamePropertyName = "StepDisplayName";
        public string StepDisplayName
        {
            get
            {
                return _StepDisplayName;
            }
            set
            {
                _StepDisplayName = value;
                NotifyPropertyChanged(StepDisplayNamePropertyName);
            }
        }
        private string _DisplayName;
        private const string DisplayNamePropertyName = "DisplayName";
        public string DisplayName
        {
            get
            {
                return _DisplayName;
            }
            set
            {
                _DisplayName = value;
                NotifyPropertyChanged(DisplayNamePropertyName);
            }
        }
        
        
        
        private UserControl _UserControl;
        private const string UserControlPropertyName = "UserControl";
        public UserControl UserControl
        {
            get
            {
                return _UserControl;
            }
            set
            {
                _UserControl = value;
                NotifyPropertyChanged(UserControlPropertyName);
            }
        }
    }
    
}
