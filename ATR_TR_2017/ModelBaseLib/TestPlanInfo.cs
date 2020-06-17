using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    public class TestPlanInfo
    {
        
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string TestProcess { get; set; }
        public string OwnerName { get; set; }
        public DateTime UpdateTime { get; set; }

    }
    public class TestPlanDB
    {
        public List<TestPlanInfo> GetTestPlanList()
        {
            return new List<TestPlanInfo>();
        }
        public void UpdateToDB(List<TestPlanInfo> newInfoList)
        {

        }
    }
}
