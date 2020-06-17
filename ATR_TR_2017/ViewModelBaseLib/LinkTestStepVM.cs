using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ViewModelBaseLib
{
    public  class LinkTestStepVM:TestStepVM
    {
        private LinkTestStep LinkTestStep
        {
            get
            {
                return TestStep as LinkTestStep;
            }
        }
        public TestPlanVM TestPlanVM { get; set; }
        //public TestPlan TestPlan
        //{
        //    get
        //    {
        //        return LinkTestStep.TestPlan;
        //    }
        //    set
        //    {
        //        LinkTestStep.TestPlan = value;
        //        if(value !=null)
        //        {
        //            LinkTestStep.CoupleStepNameList.Clear();
        //            List<TestStep> stepList = TestStep.GetAllTestStep(value as TestPlan);
        //            foreach(TestStep step in stepList)
        //            {   
        //                if(!step.Equals(LinkTestStep))
        //                {
        //                    LinkTestStep.CoupleStepNameList.Add(step.Name);
        //                }
        //            }
        //        }
        //    }
        //}
        //public TestStep CouppledTestStep
        //{
        //    get
        //    {
        //        return LinkTestStep.CoupledTestStep;
        //    }
        //    set
        //    {
        //        LinkTestStep.CoupledTestStep = value;
        //    }
        //}
        public List<string> CoupledStepNameList
        {
            get
            {
                return LinkTestStep.CoupleStepNameList;
            }
        }
        private string _CoupleStepName;
        private const string CoupleStepNamePropertyName = "CoupleStepName";
        [UIDisplay("绑定步骤", null, "CoupledStepNameList")]
        public string CoupleStepName
        {
            get
            {
                return LinkTestStep.CoupleStepName;
            }
            set
            {

                //CouppledTestStep = GetTestStepFromStepName(value);
                if (TestPlanVM != null && value != CoupleStepName)
                {
                    TestStep step = LinkTestStep.GetTestStepFromStepName(value, LinkTestStep.TestPlan);
                    if (step != null)
                    {
                        var stepInfo = TestStepInfoMgr.GetStepInfo(step.GetType().Name);
                        TreeNodeVM currentStepNode = null;
                        foreach (var connNode in TestPlanVM.ManualConnList)
                        {
                            foreach (var stepNode in connNode.SubTreeNodeList)
                            {
                                if (stepNode.NodeObj == LinkTestStep)
                                {
                                    currentStepNode = stepNode;
                                    break;
                                }
                            }
                        }
                        LinkTestStep.CoupleStepName = value;
                        LinkTestStep.ItemList.Clear();
                        if (currentStepNode != null)
                        {
                            currentStepNode.SubTreeNodeList.Clear();
                        }
                        if (stepInfo != null)
                        {
                            if (stepInfo.IsFixedItem)
                            {
                                if (stepInfo.TestTraceTypeList != null)
                                {
                                    foreach (var tr in stepInfo.TestTraceTypeList)
                                    {
                                        TestPlanVM.AddTestItem(currentStepNode, tr);
                                    }
                                }
                            }
                        }
                    }
                }
                LinkTestStep.CoupleStepName = value;
                NotifyPropertyChanged(CoupleStepNamePropertyName);
            }
        }
        
    }
}
