using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DevExpress.Mvvm;
using System.Windows.Input;
using DevExpress.Xpf.Core;
namespace ViewModelBaseLib
{
    public class CalibrationVM:NotifyBase
    {

        public CalibrationVM()
        {
            GuideMsglist = new ObservableCollection<CalInfo>();
            
        }
        private ObservableCollection<TreeNodeVM> CopyManualConnList = new ObservableCollection<TreeNodeVM>();
        public ObservableCollection<TreeNodeVM> ManualConnList
        {
            get
            {
                ObservableCollection<TreeNodeVM> treeNodelist = new ObservableCollection<TreeNodeVM>();
                foreach(var mcNode in (new ViewModelLocator()).CurrentTestPlanVm.ManualConnList)
                {
                    TreeNodeVM mcTreeNode = new TreeNodeVM() { NodeObj=mcNode.NodeObj,IsSelected=true};
                    mcTreeNode.SubTreeNodeList.Clear();
                    foreach(var stepNode in mcNode.SubTreeNodeList)
                    {
                        foreach(var realStepNode in Interface.GetAllSubTreeNodeVM(stepNode))
                        {
                            if(TestStepInfoMgr.Instance.TestStepInfoList.Where(x=>x.DisplayName==realStepNode.DisplayName && x.IsNeedCal==true).Count()>0)
                            {
                                TreeNodeVM realTreeStepNode = new TreeNodeVM() { NodeObj = realStepNode.NodeObj, IsSelected = true,ParentNode=mcTreeNode};
                                mcTreeNode.SubTreeNodeList.Add(realTreeStepNode);
                            }
                        }
                    }
                    if(mcTreeNode.SubTreeNodeList.Count() > 0)
                    {
                        treeNodelist.Add(mcTreeNode);
                    }
                }
                //foreach (var mcNode in treeNodelist)
                //{
                //    mcNode.PropertyChanged += item_PropertyChanged;
                //    foreach (var stepNode in mcNode.SubTreeNodeList)
                //    {
                //        stepNode.PropertyChanged += item_PropertyChanged;
                //    }
                //}
                CopyManualConnList = treeNodelist;
                
                return treeNodelist;
            }
            set
            { 
               
            }
        }
        private List<TreeNodeVM> treeNodeList = new List<TreeNodeVM>();
        private void item_PropertyChanged(object sender,PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
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
            }
            UpdateGuideMsglist();
        }
        public void UpdateGuideMsglist()
        {     
            GuideMsglist.Clear();
            treeNodeList.Clear();
            foreach (var node in CopyManualConnList)
            {
                foreach (var stepNode in node.SubTreeNodeList)
                {
                    //if (stepNode.IsSelected)
                    //{
                        treeNodeList.Add(stepNode);
                    //}
                }
            }
             foreach(TreeNodeVM node in treeNodeList)
             {
                 if(node.NodeObj is TestStep)
                 {
                     if ((node.NodeObj as TestStep).CalStepMsgList != null)
                     {
                         foreach (string msg in (node.NodeObj as TestStep).CalStepMsgList)
                         {
                             if (GuideMsglist.Where(x => x.GuidMsgStr == msg).Count() > 0)
                             {
                                 var item = GuideMsglist.Where(x => x.GuidMsgStr == msg).FirstOrDefault();
                                 string tempMsg = msg;
                                 item.StepActionlist.Add(() => CalStepAction(tempMsg, node.NodeObj as TestStep));
                             }
                             else
                             {
                                 CalInfo calInfo = new CalInfo();
                                 string temMsg1 = msg;
                                 calInfo.StepActionlist.Add(() => CalStepAction(msg, node.NodeObj as TestStep));
                                 calInfo.GuidMsgStr = msg;
                                 GuideMsglist.Add(calInfo);
                             }
                         }
                     }
                 }
             }
        }
        private void CalStepAction(string msg, TestStep step)
        {
            try
            {
                TestPlanManager.PathConfigSetter.SetPathConfig(step.GetCurrentPathConfigInfo().Path);
            }
            catch(Exception ex)
            {
                throw (new Exception("can't set path" + step.PathConfigName));
            }
            step.AcquireStep(msg);
        }
        public ObservableCollection<CalInfo> GuideMsglist { get; set; }
        private int _CurrentIndex;
        private const string CurrentIndexPropertyName = "CurrentIndex";
        public int CurrentIndex
        {
            get
            {
                return _CurrentIndex;
            }
            set
            {
                _CurrentIndex = value;
                NotifyPropertyChanged(CurrentIndexPropertyName);
                NotifyPropertyChanged(PrevVisibilityPropertyName);
                NotifyPropertyChanged(NextVisibilityPropertyName);
                NotifyPropertyChanged(FinishVisibilityPropertyName);

            }
        }
        private System.Windows.Visibility _PrevVisibility;
        private const string PrevVisibilityPropertyName = "PrevVisibility";
        public System.Windows.Visibility PrevVisibility
        {
            get
            {
                if (CurrentIndex > 0)
                    return System.Windows.Visibility.Visible;
                else
                    return System.Windows.Visibility.Hidden;
            }
            set
            {
                _PrevVisibility = value;
                NotifyPropertyChanged(PrevVisibilityPropertyName);
            }
        }
        private System.Windows.Visibility _NextVisibility;
        private const string NextVisibilityPropertyName = "NextVisibility";
        public System.Windows.Visibility NextVisibility
        {
            get
            {
                if (CurrentIndex == TestStepFactory.CalibUClist.Count - 1)
                    return System.Windows.Visibility.Collapsed;
                else
                    return System.Windows.Visibility.Visible;
            }
            set
            {
                _NextVisibility = value;
                NotifyPropertyChanged(NextVisibilityPropertyName);
            }
        }

        private System.Windows.Visibility _FinishVisibility;
        private const string FinishVisibilityPropertyName = "FinishVisibility";
        public System.Windows.Visibility FinishVisibility
        {
            get
            {
                if (CurrentIndex == TestStepFactory.CalibUClist.Count-1)
                    return System.Windows.Visibility.Visible;
                else
                    return System.Windows.Visibility.Collapsed;
            }
            set
            {
                _FinishVisibility = value;
                NotifyPropertyChanged(FinishVisibilityPropertyName);
            }
        }
        public void Next()
        {
            if (CurrentIndex < TestStepFactory.CalibUClist.Count())
            {
                IMoveNext moveNext = TestStepFactory.CalibUClist[CurrentIndex] as IMoveNext;
                if (moveNext != null)
                {
                    moveNext.MoveNext();
                }
                CurrentIndex = CurrentIndex + 1;
                
            }
            if (CurrentIndex == TestStepFactory.CalibUClist.Count() - 1)
            {
                //InitAllCal();
                //UpdateGuideMsglist();
            }
        }
        public void InitAllCal()
        {
            //Symtant.InstruDriver.InstruDriverFactory.ResetAllInstru();
            foreach (var conn in CopyManualConnList)
            {
                foreach (var stepNode in conn.SubTreeNodeList)
                {
                    
                    
                    //if (stepNode.IsSelected)
                    //{
                        (stepNode.NodeObj as TestStep).InitCal();
                    //}
                }
            }
        }
        public void Prev()
        {
            if (CurrentIndex > 0)
            {
                CurrentIndex = CurrentIndex - 1;
            }
        }
        public void Cancle()
        { 
        }
        public void Confirm()
        {
            foreach (var conn in CopyManualConnList)
            {
                foreach (var stepNode in conn.SubTreeNodeList)
                {
                    //if (stepNode.IsSelected)
                    //{
                        (stepNode.NodeObj as TestStep).FinishCal();
                    //}
                }
            }
            foreach (var conn in CopyManualConnList)
            {
                foreach (var stepNode in conn.SubTreeNodeList)
                {
                    //if (stepNode.IsSelected)
                    //{
                        (stepNode.NodeObj as TestStep).SaveCal();
                    //}
                }
            }
            
            Interface.SaveAllLocalSettings(TestPlanManager.CurrentTestPlan);
            DXMessageBox.Show("校准完成！");
        }

    }
    public class CalInfo:NotifyBase
    {
        public CalInfo()
        {
            StepActionlist = new List<Action>();
            //ButtonBackGround = System.Windows.Media.Brushes.LightGray;
        }
        public string GuidMsgStr { get; set; }
        public bool IsStepFinish { get; set; }
        //private System.Windows.Media.Brush _ButtonBackGround;
        private const string ButtonBackGroundPropertyName = "ButtonBackGround";
        public System.Windows.Media.Brush ButtonBackGround
        {
            get
            {
                return IsStepFinish?System.Windows.Media.Brushes.Green: System.Windows.Media.Brushes.LightGray;
            }
            
        }
        
        public List<Action> StepActionlist { get; set; }
        private DelegateCommand _CalibratCommand;
        public ICommand CalibratCommand
        {
            get
            {
                if (_CalibratCommand == null)
                {
                    _CalibratCommand = new DelegateCommand(StepCal);
                }
                return _CalibratCommand;
            }
        }
        private void StepCal()
        { 
            foreach(var action in StepActionlist)
            {
                if(action !=null)
                {
                    action.Invoke();
                }
            }
            IsStepFinish = true;
            NotifyPropertyChanged(ButtonBackGroundPropertyName);
        }
    }
}
