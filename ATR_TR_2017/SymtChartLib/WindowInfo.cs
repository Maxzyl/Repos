using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SymtChartLib
{
    public class WindowInfo
    {
        public WindowInfo()
        {
            XAxisList = new List<XAxis>();
        }

        public string Name { get; set; }
        public List<XAxis> XAxisList { get; set; }
    }
}
