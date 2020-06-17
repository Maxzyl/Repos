using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    

    /// <summary>
    /// wrapper of Dictionary<freq, double>
    /// </summary>
    public class FreqIndexedData : Dictionary<string, double>
    {

    }
    public struct XYData
    {
        public double X;
        public double Y;
        
        public XYData(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
    public class XYDataArr : List<XYData>
    {

    }
    public class XYDadaClone
    {   
        public double X { get; set; }
        public double Y { get; set; }
    }
    
}
