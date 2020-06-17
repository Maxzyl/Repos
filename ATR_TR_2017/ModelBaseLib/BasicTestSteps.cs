using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    [UITestStepPara("弹出对话框",null,null)]
    public class MsgBoxTestStep:TestStep
    {
        [UIDisplay("信息")]
        [UIDisplayPara("信息")]
        public string Msg { get; set; }
        public override void Single()
        {
            System.Windows.MessageBox.Show(Msg);
        }
    }
    [UITestStepPara("延时", null, null)]
    public class DelayTestStep : TestStep
    {
        [UIDisplay("时间(s)", typeof(DataUtils.SIPrefixConverter))]
        [UIDisplayPara("时间(s)", typeof(DataUtils.SIPrefixConverter))]
        public double Time { get; set; }
        public override void Single()
        {
            System.Threading.Thread.Sleep((int)(Time * 1000));
        }
    }
}
