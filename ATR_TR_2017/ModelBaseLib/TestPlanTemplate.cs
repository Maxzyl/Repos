using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    /// <summary>
    /// 测试方案模板，用来根据用户要求定制化易用的测试方案模板
    /// </summary>
    public interface ITestPlanTemplate
    {
        TestPlan GetTestPlan();
        
    }

    
}
