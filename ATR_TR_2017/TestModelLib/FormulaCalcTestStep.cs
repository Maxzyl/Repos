using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using System.Windows.Input;
using DevExpress.Mvvm;
using SymtCalc;
namespace TestModelLib
{   
    public class FormulaCalcTestStep:TestStep
    {
        public override void Single()
        {
            double[] x = null;
            if(UpdateCalTestItem !=null)
            {
                x=UpdateCalTestItem.Invoke();
            }
            if(x.Length==0)return;
            foreach(TestItem item in this.ItemList)
            {
                if(item as TestTrace !=null)
                {
                    CalcPointTestTrace trace = item as CalcPointTestTrace;
                    object result = Cal.Calucate(trace.Formula);
                    if (result is double[] && x!=null)
                    {
                        trace.ResultData.Clear();
                        double[] data = result as double[];
                        for (int i = 0; i < data.Length;i++ )
                        {
                            trace.ResultData.Add(new XYData() {X=x[i],Y=data[i] });
                        }
                    }

                }
                else if(item as PointTestItem !=null)
                {
                    CalcPointTestItem poinItem = item as CalcPointTestItem;
                    object result = Cal.Calucate(poinItem.Formula);
                    if(result is double && x!=null)
                    {
                        poinItem.X=x[0];
                        poinItem.Y = Convert.ToDouble(result);
                    }
                }
            }

        }
        [System.Xml.Serialization.XmlIgnore]
        public Func<double[]> UpdateCalTestItem;
        [System.Xml.Serialization.XmlIgnore]
        public Calculation Cal { get; set; }
    }
    
}
