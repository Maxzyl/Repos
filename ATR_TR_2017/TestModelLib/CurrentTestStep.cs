using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
namespace TestModelLib
{
    public class CurrentTestStep:TestStep
    {
        public override void CreateTrace(string traceTypeName)
        {
            ItemList.Add(new CurrentTestItem() { TypeName = traceTypeName, ChannelNum = 1 });
        }
        public override void Single()
        {
            if (GeneTestSetup.Instance.IsSimulated)
            {
                GeneSimulatedData();
            }
            else
            {

            }
        }
        public override void GeneSimulatedData()
        {
            foreach (PointTestItem item in ItemList)
            {
                item.X = 0;
                item.Y = Symtant.GeneFunLib.GeneFun.GetRand(10, 5);
            }
        }
    }
    public class CurrentTestItem : PointTestItem
    {
        [UIDisplay("通道")]
        public int ChannelNum { get; set; }
    }
}
