using ModelBaseLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModelBaseLib
{
    public class ViewModelLocator
    {
        private static  TestPlanVM _CurrentTestPlanVm = new TestPlanVM();
        public TestPlanVM CurrentTestPlanVm
        {
            get
            {
                return _CurrentTestPlanVm;
            }
        }
        public GeneTestSetup GeneralTestSetupModel
        {
            get
            {
                return GeneTestSetup.Instance;
            }
        }
        private static MainWindowVM _MainWindow = new MainWindowVM();
        public MainWindowVM MainWindow
        {
            get
            {
                return _MainWindow;
            }
        }
        private static ResultDisplayListenerVM _ResultDisplayListenerVM = new ResultDisplayListenerVM();
        public ResultDisplayListenerVM ResultDisplayListenerVM
        {
            get
            {
                return _ResultDisplayListenerVM;
            }
        }
        private static TestPlanLocalSettingVM _TestPlanLocalSettingVM;
        public static TestPlanLocalSettingVM TestPlanLocalSettingVM
        {
            get
            {
                if (_TestPlanLocalSettingVM == null)
                {
                    _TestPlanLocalSettingVM = new TestPlanLocalSettingVM();
                }
                return _TestPlanLocalSettingVM;
            }
        }
    }
}
