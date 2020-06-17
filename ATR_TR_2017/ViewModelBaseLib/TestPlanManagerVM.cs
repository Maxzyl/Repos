using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using System.Windows.Input;
using System.Collections.ObjectModel;
namespace ViewModelBaseLib
{
    public class TestPlanManagerVM:NotifyBase
    {
        public TestPlanManagerVM()
        {
            _TestPlanList.CollectionChanged += _TestPlanList_CollectionChanged;
        }

        void _TestPlanList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
        }
        /// <summary>
        /// get test plan list from database 
        /// </summary>
        private TestPlanDB testPlanDB = new TestPlanDB();
        private ObservableCollection<TestPlanInfo> _TestPlanList=new ObservableCollection<TestPlanInfo>();
        private const string TestPlanListPropertyName = "TestPlanList";
        public ObservableCollection<TestPlanInfo> TestPlanList
        {
            get
            {
                return _TestPlanList;
            }
            set
            {
                _TestPlanList = value;
                NotifyPropertyChanged(TestPlanListPropertyName);
            }
        }
        public void OnLoad()
        {
            _TestPlanList.Clear();
            var testPlanInDB = testPlanDB.GetTestPlanList();
            
        }
        /// <summary>
        /// create new test plan in database
        /// and notify test plan list
        /// </summary>
        public void New()
        {

        }
        public void Edit()
        {

        }
        public void Copy()
        {
        }
        public void Remove()
        {
        }
        public void Paste()
        {
        }
        public void Load()
        {

        }
        public void Save()
        {
            testPlanDB.UpdateToDB(_TestPlanList.ToList());
        }
        private int  _SelectedIndex;
        private const string SelectedIndexPropertyName = "SelectedIndex";
        public int  SelectedIndex
        {
            get
            {
                return _SelectedIndex;
            }
            set
            {
                _SelectedIndex = value;
                NotifyPropertyChanged(SelectedIndexPropertyName);
            }
        }
        
    }
    //public class TestPlanInfoVM:NotifyBase
    //{
    //    public TestPlanInfo testPlan = new TestPlanInfo();
    //    private string  _DisplayName;
    //    private const string DisplayNamePropertyName = "DisplayName";
    //    public string  DisplayName
    //    {
    //        get
    //        {
    //            return testPlan.Name;
    //        }
    //        set
    //        {
    //            testPlan.Name = value;
    //            NotifyPropertyChanged(DisplayNamePropertyName);
    //        }
    //    }

    //    private string _Version;
    //    private const string VersionPropertyName = "Version";
    //    public string Version
    //    {
    //        get
    //        {
    //            return testPlan.Version;
    //        }
    //        set
    //        {
    //            testPlan.Version = value;
    //            NotifyPropertyChanged(VersionPropertyName);
    //        }
    //    }
    //    private string _Description;
    //    private const string DescriptionPropertyName = "Description";
    //    public string Description
    //    {
    //        get
    //        {
    //            return testPlan.Description;
    //        }
    //        set
    //        {
    //            testPlan.Description = value;
    //            NotifyPropertyChanged(DescriptionPropertyName);
    //        }
    //    }
    //    private string _TestProcess;
    //    private const string TestProcessPropertyName = "TestProcess";
    //    public string TestProcess
    //    {
    //        get
    //        {
    //            return testPlan.TestProcess;
    //        }
    //        set
    //        {
    //            testPlan.TestProcess = value;
    //            NotifyPropertyChanged(TestProcessPropertyName);
    //        }
    //    }

    //    private string _OwnerName;
    //    private const string OwnerNamePropertyName = "OwnerName";
    //    public string OwnerName
    //    {
    //        get
    //        {
    //            return testPlan.OwnerName;
    //        }
    //        set
    //        {
    //            testPlan.OwnerName = value;
    //            NotifyPropertyChanged(OwnerNamePropertyName);
    //        }
    //    }
    //    private DateTime _UpdateTime;
    //    private const string UpdateTimePropertyName = "UpdateTime";
    //    public DateTime UpdateTime
    //    {
    //        get
    //        {
    //            return testPlan.UpdateTime;
    //        }
    //        set
    //        {
    //            testPlan.UpdateTime = value;
    //            NotifyPropertyChanged(UpdateTimePropertyName);
    //        }
    //    }
        
    //}
}
