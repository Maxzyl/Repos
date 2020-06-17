using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;

namespace TestModelLib
{
    public class TempSetStep:TestStep
    {
        public override void Single()
        {
            System.Windows.MessageBox.Show("当前温度"+Temperature);
        }
        [UIDisplayPara("温度")]
        public double Temperature { get; set; }
    }
}
