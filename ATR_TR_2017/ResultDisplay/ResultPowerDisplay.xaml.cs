using ModelBaseLib;
using Symtant.GeneFunLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestResultMarkerDip
{
    /// <summary>
    /// Interaction logic for UCTestPowerDisplay.xaml
    /// </summary>
    [UIDisplayPara("载波功率显示")]
    public partial class ResultPowerDisplay : UserControl, IResultListerner
    {
        TestPowerResult tr = new TestPowerResult();
        public ResultPowerDisplay()
        {
            InitializeComponent();
            this.DataContext = tr;
        }
        
        public TestPlan TestPlan { get; set; }
        public int SpecIndex { get; set; }
        public  bool OnTestPlanRunStart()
        {
            return true;
        }
        public  bool OnTestPlanRunCompleted()
        {
            return true;
        }
        public  void OnTestManualConnRunStart()
        {

        }
        public  void OnTestManualConnRunCompleted()
        {

        }
        public  void OnTestStepRunStart(int stepIndex)
        {

        }
        public  void OnTestStepRunCompleted(int stepIndex)
        {

        }
        public  void OnChildTestStepRunStart()
        {

        }
        public  void OnChildTestStepRunCompleted(string stepName)
        {

        }
        public  void OnPointFinish(int stepIndex, int traceIndex, int markerIndex)
        {
           TestStep step = TestPlan.ManualConnectionList[TestPlan.currentConnIndex].TestStepList[stepIndex];
           if (step is LoopTestStep) return;
           for (int k = 0; k < step.ItemList.Count; k++)
           {
               if (step.ItemList[k] as PointTestItem != null)
               {
                   if ((step.ItemList[k] as PointTestItem).TypeName=="功率F1")
                   {
                       tr.Power1 = (step.ItemList[k] as PointTestItem).Y;
                   }
                   else if ((step.ItemList[k] as PointTestItem).TypeName=="功率F2")
                   {
                       tr.Power2 = (step.ItemList[k] as PointTestItem).Y;
                   }
               }
           }
        }
    }
}
