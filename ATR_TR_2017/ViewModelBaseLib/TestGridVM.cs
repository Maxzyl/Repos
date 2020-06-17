using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;

namespace ViewModelBaseLib
{
    public  class TestGridVM
    {
        public TestGridVM()
        {
            TestVMList = new ObservableCollection<object>();
        }
        private string _ConnName;
        private const string ConnNamePropertyName = "ConnName";
        public string ConnName
        {
            get
            {
                return _ConnName;
            }
            set
            {
                _ConnName = value;
            }
        }
        public ObservableCollection<object> TestVMList { get; set; }
    }
}
