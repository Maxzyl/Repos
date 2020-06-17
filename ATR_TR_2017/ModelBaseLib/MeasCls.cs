using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    public interface IMeasCls
    {
        TestStep TestStep { get; set; }
        void InitOnce();
        void Single();
        
    }
}
