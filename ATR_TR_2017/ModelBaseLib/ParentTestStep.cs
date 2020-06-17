using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    [Serializable]
    public class ParentTestStep:TestStep
    {   
        public ParentTestStep()
        {
            ChildTestStepList = new List<TestStep>();
        }
        public List<TestStep> ChildTestStepList { get; set; }
    }
}
