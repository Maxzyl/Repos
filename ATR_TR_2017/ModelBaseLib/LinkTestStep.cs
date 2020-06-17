using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    public class LinkTestStep:TestStep
    {
        public LinkTestStep()
        {
            
        }
        [System.Xml.Serialization.XmlIgnore]
        public TestPlan TestPlan { get; set; }
        
        [System.Xml.Serialization.XmlIgnore]
        public List<string> CoupleStepNameList
        {
            get
            {
                List<string> stepNameList = new List<string>();
                if (TestPlan != null)
                {
                    List<TestStep> stepList = TestStep.GetAllTestStep(TestPlan);
                    foreach(TestStep step in stepList)
                    {
                        if (!(step is LinkTestStep))
                        {
                            stepNameList.Add(step.Name);
                        }
                    }
                }
                return stepNameList;
            }
        }
        public string CoupleStepName { get; set; }
        public override void InitOnce()
        {
            
        }
        public override void PreSingle()
        {
            
        }
        public override void PostSingle()
        {
            
        }
        public override void Single()
        {
            TestStep CoupledTestStep = GetTestStepFromStepName(CoupleStepName,TestPlanManager.CurrentTestPlan);
             if(CoupledTestStep!=null)
             {
                 var cacheItemList = CoupledTestStep.ItemList;
                 CoupledTestStep.ItemList = ItemList;
                 CoupledTestStep.Single();
                 CoupledTestStep.ItemList = cacheItemList;
                // Console.WriteLine("aaa");
             }
        }
        public TestStep GetTestStepFromStepName(string stepName,TestPlan testPlan)
        {
            TestStep step = null;
            List<TestStep> stepList = TestStep.GetAllTestStep(testPlan);
            var linkStep = stepList.Where(x => x.Name == stepName).FirstOrDefault();
            if (linkStep != null)
            {
                step = linkStep;
            }
            return step;
        }
        public override string[] ItemTypeNameList
        {
            get
            {
                TestStep CoupledTestStep = GetTestStepFromStepName(CoupleStepName, TestPlan);
                if (CoupledTestStep != null)
                {
                    return CoupledTestStep.ItemTypeNameList;
                }
                return null;
            }
        }
        public override void CreateTrace(string traceTypeName)
        {
            TestStep CoupledTestStep = GetTestStepFromStepName(CoupleStepName, TestPlan);
            if (CoupledTestStep != null)
            {
                var stepInfo = TestStepInfoMgr.GetStepInfo(CoupledTestStep.GetType().Name);
                if (stepInfo!=null&&stepInfo.IsFixedItem && ItemList.Where(x => x.TypeName == traceTypeName).FirstOrDefault() != null)
                {
                    return;
                }
                else
                {
                    var cacheItemList = CoupledTestStep.ItemList;
                    CoupledTestStep.ItemList = ItemList;
                    CoupledTestStep.CreateTrace(traceTypeName);
                    CoupledTestStep.ItemList = cacheItemList;
                }
            }
        }
    }
}
