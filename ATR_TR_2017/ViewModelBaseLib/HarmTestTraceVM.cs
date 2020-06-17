using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestModelLib;
using ModelBaseLib;
namespace ViewModelBaseLib
{
    public class HarmTestTraceVM:TestTraceVM
    {
        private int _nHarm;
        private const string nHarmPropertyName = "nHarm";
        [UIDisplay("谐波次数",null,2,3,4,5,6,7,8,9,10)]
        public int nHarm
        {
            get
            {
                return (TestTrace as HarmTestTrace).nHarm ;
            }
            set
            {
                (TestTrace as HarmTestTrace).nHarm = value;
                NotifyPropertyChanged(nHarmPropertyName);
            }
        }
        
    }
}
