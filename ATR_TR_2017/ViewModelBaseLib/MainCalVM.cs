using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TestModelLib;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using System.IO;
using DevExpress.Xpf.Core;
namespace ViewModelBaseLib
{   

    public class MainCalVM:NotifyBase
    {
        private NFTestStepCalVM _NFTestStepCal=new NFTestStepCalVM();
        private const string NFTestStepCalPropertyName = "NFTestStepCal";
        public NFTestStepCalVM NFTestStepCal
        {
            get
            {
                return _NFTestStepCal;
            }
            set
            {
                _NFTestStepCal = value;
                NotifyPropertyChanged(NFTestStepCalPropertyName);
            }
        }
        private IMDTestStepCalVM _IMDTestStepCal=new IMDTestStepCalVM();
        private const string IMDTestStepCalPropertyName = "IMDTestStepCal";
        public IMDTestStepCalVM IMDTestStepCal
        {
            get
            {
                return _IMDTestStepCal;
            }
            set
            {
                _IMDTestStepCal = value;
                NotifyPropertyChanged(IMDTestStepCalPropertyName);
            }
        }
        public void InitialData(TestPlanVM testPlanVM)
        {
            //IMDTestStepCal.ManualConnList.Clear();
            //NFTestStepCal.ManualConnList.Clear();
            //foreach (ManualConnectionVM mcNode in testPlanVM.ManualConnList)
            //{
            //    if (mcNode.TestStepList.Select(x => x.TestStep.GetType() == typeof(NFTestStep)).Count() > 0)
            //    {
            //        ManualConnectionVM manualNode = new ManualConnectionVM() { DisplayName = mcNode.DisplayName };
            //        foreach (TestStepVM stepNode in mcNode.TestStepList)
            //        {
            //            if (stepNode.TestStep.GetType() == typeof(NFTestStep))
            //            {
            //                manualNode.TestStepList.Add(stepNode);
            //            }
            //        }
            //        if (manualNode.TestStepList.Count() > 0)
            //        {
            //            NFTestStepCal.ManualConnList.Add(manualNode);
            //        }
            //    }
            //    if (mcNode.TestStepList.Select(x => x.TestStep.GetType() == typeof(IMDTestStep)).Count() > 0)
            //    {
            //        ManualConnectionVM manualNode = new ManualConnectionVM() { DisplayName = mcNode.DisplayName };
            //        foreach (TestStepVM stepNode in mcNode.TestStepList)
            //        {
            //            if (stepNode.TestStep.GetType() == typeof(IMDTestStep))
            //            {
            //                manualNode.TestStepList.Add(stepNode);
            //            }
            //        }
            //        if (manualNode.TestStepList.Count() > 0)
            //        {
            //            IMDTestStepCal.ManualConnList.Add(manualNode);
            //        }
            //    }
            //}
            //testPlanVM.TestPlan.InitAllCal();
        }
        
        
    }
    public class TestStepCalVM:NotifyBase
    {
        private ObservableCollection<ManualConnectionVM> _ManualConnList = new ObservableCollection<ManualConnectionVM>();
        private const string ManualConnListPropertyName = "ManualConnList";
        [System.Xml.Serialization.XmlIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public ObservableCollection<ManualConnectionVM> ManualConnList
        {
            get
            {
                return _ManualConnList;

            }
            set
            {
                _ManualConnList = value;
                NotifyPropertyChanged(ManualConnListPropertyName);
            }
        }
    }
    public class NFTestStepCalVM : TestStepCalVM
    {   
        private DelegateCommand _CalNFCmd;
        public ICommand CalNFCmd
        {
            get
            {
                if (_CalNFCmd == null)
                { 
                    _CalNFCmd = new DelegateCommand(() => CalNF());
                }
                return _CalNFCmd;
            }
            
        }

        private void CalNF()
        {
            DXSplashScreen.Show<Cal_DXSplashScreen>();
            DXSplashScreen.Progress(0);
            TestPlanVM vm = (new ViewModelLocator()).CurrentTestPlanVm;
            if (!GeneTestSetup.Instance.IsSimulated)
            {
                foreach (var conn in ManualConnList)
                {
                    //foreach (var step in conn.TestStepList)
                    //{
                    //    if (step.IsTest)
                    //    {
                    //        (step.TestStep as NFTestStep).CalNFA();
                    //    }
                    //}
                }
                Interface.SaveAllLocalSettings(vm);
            }
            DXSplashScreen.Progress(1);
            if (DXSplashScreen.IsActive) DXSplashScreen.Close();
        }
    }
    public class IMDTestStepCalVM : TestStepCalVM
    {
        private DelegateCommand _CalIMDCmd;
        public ICommand CalIMDCmd
        {
            get
            {
                if (_CalIMDCmd == null)
                {
                    _CalIMDCmd = new DelegateCommand(() => CalIMD());
                }
                return _CalIMDCmd;
            }

        }

        private void CalIMD()
        {
            DXSplashScreen.Show<Cal_DXSplashScreen>();
            DXSplashScreen.Progress(0);
            TestPlanVM vm = (new ViewModelLocator()).CurrentTestPlanVm;
            if (!GeneTestSetup.Instance.IsSimulated)
            {
                foreach (var conn in ManualConnList)
                {
                    //foreach (var step in conn.TestStepList)
                    //{
                    //    if (step.IsTest)
                    //    {
                    //        (step.TestStep as IMDTestStep).CalIMDPower();
                    //    }
                    //}
                }
                Interface.SaveAllLocalSettings(vm);
            }
            DXSplashScreen.Progress(1);
            if (DXSplashScreen.IsActive) DXSplashScreen.Close();
        }
    }

}
