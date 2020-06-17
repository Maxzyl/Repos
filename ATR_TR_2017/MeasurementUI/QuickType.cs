using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeasurementUI
{
    public class QuickType
    {
        public static double[] GenerateIndexedArray(double start,double stop,int points)
        {
            double[] ress = new double[points];
            for (int i = 0; i < points; i++)
            {
                ress[i] = CalcIndexedValue(start, stop, points, i);
            }
            return ress;
        }

        public static double CalcIndexedValue(double start, double stop, int points, int i)
        {
            if (points == 1)
            {
                return start;
            }
            else
            {
                return start + (stop - start) / (points - 1) * i;
            }
        }
    }
}
