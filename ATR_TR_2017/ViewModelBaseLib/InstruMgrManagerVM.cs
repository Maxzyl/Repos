using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Symtant.InstruDriver;

namespace ViewModelBaseLib
{
    public class InstruMgrManagerVM : NotifyBase
    {
        public InstruMgrManagerVM()
        {

            foreach (TestStepInfo stepInfo in TestStepInfoMgr.Instance.TestStepInfoList)
            {
                foreach (var clsInfo in stepInfo.MeasClsInfoList)
                {
                    if (clsInfo.IsSelected == true)
                    {
                        InstruMgrVM mgrVM = new InstruMgrVM();
                        mgrVM.DisplayName = clsInfo.DisplayName;
                        foreach (var instruInfo in clsInfo.InstruInfoList)
                        {
                            mgrVM.InstruInfoList.Add(new InstruInfoVM() { InstruInfo = instruInfo });
                            
                            InstruInfo.Add(instruInfo);
                        }
                        TempInstruList.Add(mgrVM);
                    }
                }
            }

            InstruList = TempInstruList;
            var v = InstruInfo.GroupBy(x => new { x.DisplayName, x.Name }).ToList();
            foreach (var item in v)
            {
                InstruMgrVM mgrVM = new InstruMgrVM();
                mgrVM.DisplayName = item.Key.DisplayName;
                foreach (var child in item)
                {
                    mgrVM.InstruInfoList.Add(new InstruInfoVM() { InstruInfo = child, InstruInfoList = item.ToList() });
                    break;
                }
                OverViewInstruList.Add(mgrVM);
            }
            UpdateInstruMgrList();
        }

        private List<InstruInfo> _instruInfo = new List<InstruInfo>();
        private const string instruInfoPropertyName = "instruInfo";
        public List<InstruInfo> InstruInfo
        {
            get
            {
                return _instruInfo;
            }
            set
            {
                _instruInfo = value;
                NotifyPropertyChanged(instruInfoPropertyName);
            }
        }

        private ObservableCollection<InstruMgrVM> _OverViewInstruList = new ObservableCollection<InstruMgrVM>();
        private const string OverViewInstruListPropertyName = "OverViewInstruList";
        public ObservableCollection<InstruMgrVM> OverViewInstruList
        {
            get
            {
                return _OverViewInstruList;
            }
            set
            {
                _OverViewInstruList = value;
                NotifyPropertyChanged(OverViewInstruListPropertyName);
            }
        }

        private InstruMgrVM _SelectedInstruMgr;
        private const string SelectedInstruMgrPropertyName = "SelectedInstruMgr";
        [System.Xml.Serialization.XmlIgnore]
        public InstruMgrVM SelectedInstruMgr
        {
            get
            {
                return _SelectedInstruMgr;
            }
            set
            {
                _SelectedInstruMgr = value;
                NotifyPropertyChanged(SelectedInstruMgrPropertyName);
            }
        }
        
        private ObservableCollection<InstruMgrVM>  _InstruList=new ObservableCollection<InstruMgrVM>();
        private const string InstruListPropertyName = "InstruList";
        public ObservableCollection<InstruMgrVM> InstruList
        {
            get
            {
                return _InstruList;
            }
            set
            {
                _InstruList = value;
                NotifyPropertyChanged(InstruListPropertyName);
            }
        }


        private ObservableCollection<InstruMgrVM> _TempInstruList = new ObservableCollection<InstruMgrVM>();
        private const string TempInstruListPropertyName = "TempInstruList";
        public ObservableCollection<InstruMgrVM> TempInstruList
        {
            get
            {
                return _TempInstruList;
            }
            set
            {
                _TempInstruList = value;
                NotifyPropertyChanged(TempInstruListPropertyName);
            }
        }


        private ObservableCollection<InstruInfoVM> _InstruMgrList = new ObservableCollection<InstruInfoVM>();
        private const string InstruMgrListPropertyName = "InstruMgrList";
        public ObservableCollection<InstruInfoVM> InstruMgrList
        {
            get
            {
                _InstruMgrList.Clear();

                foreach (InstruMgrVM instruMgrVM in InstruList)
                {
                    foreach(InstruInfoVM instruCmdVM in instruMgrVM.InstruInfoList)
                    {
                        _InstruMgrList.Add(instruCmdVM);
                    }
                }
                return _InstruMgrList;
            
            }
        }
        public void ConnectAll()
        { 
            foreach(InstruMgrVM instruMgrVM in InstruList)
            {
                 foreach(InstruInfoVM instruComVM in instruMgrVM.InstruInfoList)
                 {
                     instruComVM.Connect();
                    //zhang_yanling  make Comment  below
                   //  instruComVM.IsConnect = true;
                 }
            }
        
        }
        void UpdateConnect()
        {
            foreach (InstruMgrVM instruMgrVM in InstruList)
            {
                int count = instruMgrVM.InstruInfoList.Count();
                int i = instruMgrVM.InstruInfoList.Where(x => x.IsConnect == null).Count();
                int j = instruMgrVM.InstruInfoList.Where(x=>x.IsConnect==true).Count();
                int k = instruMgrVM.InstruInfoList.Where(x => x.IsConnect == false).Count();
                if(i > 0)
                {
                    instruMgrVM.IsConnect = null;
                    continue;
                }
                if(count==j)
                {
                    instruMgrVM.IsConnect = true;
                }
                if(i==0 && k > 0)
                {
                    instruMgrVM.IsConnect = false;
                }
            }
        }
        public void InitialData(ObservableCollection<InstruInfoVM> InstruInfoVmList)
        {
        //    foreach (InstruInfoVM instruInfoVMCopy in InstruInfoVmList)
        //    {
        //        InstruInfo instruInfoCopy = instruInfoVMCopy.InstruInfo;
        //        foreach (InstruInfoVM instruInforVM in this.InstruMgrList)
        //        {
        //            InstruInfo instruInfo = instruInforVM.InstruInfo;
        //            if (instruInfoCopy.Name == instruInfo.Name && instruInfoCopy.DisplayName == instruInfo.DisplayName)
        //            {
        //                instruInfo.InstruDriverType = instruInfoCopy.InstruDriverType;
        //                instruInfo.Model = instruInfoCopy.Model;
        //                instruInfo.Vendor = instruInfoCopy.Vendor;
        //                instruInfo.SerialNum = instruInfoCopy.SerialNum;
        //                instruInfo.FirmwareVersion = instruInfoCopy.FirmwareVersion;
        //                instruInfo.RemoteInterface = instruInfoCopy.RemoteInterface;
        //                instruInfo.Address = instruInfoCopy.Address;
        //            }
        //        }
        //    }
        }
       
        public void UpdateInstruMgrList()
        {
            foreach (InstruMgrVM instruMgrVM in InstruList)
            {
                foreach (InstruInfoVM instruCmdVM in instruMgrVM.InstruInfoList)
                {
                    instruCmdVM.UpdateConnnect = UpdateConnect;
                }
            }
        }
    }
}
