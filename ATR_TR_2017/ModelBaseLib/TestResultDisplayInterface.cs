using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    public interface IResultListerner
    {
        TestPlan TestPlan { get; set; }
        int SpecIndex { get; set; }
        bool OnTestPlanRunStart();
        bool OnTestPlanRunCompleted();
        void OnTestManualConnRunStart();
        void OnTestManualConnRunCompleted();
        void OnTestStepRunStart(int stepIndex);
        void OnTestStepRunCompleted(int stepIndex);
        void OnChildTestStepRunStart();
        void OnChildTestStepRunCompleted(string stepName);
        void OnPointFinish(int stepIndex, int traceIndex, int markerIndex);
    }
}
