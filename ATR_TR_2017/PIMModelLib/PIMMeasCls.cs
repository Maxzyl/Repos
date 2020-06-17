using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelBaseLib;
using Symtant.InstruDriver.PIMTester;
using Symtant.InstruDriver;
namespace PIMModelLib
{
    //public enum PIMSweepToneEnum { Tone1, Tone2 }
    //public enum PIMSidebandTypeEnum { Low, High, Dual }
    //public enum PIMMeasMode { Point, Sweep }
    //public enum PIMCalType { Point,Sweep,TimeDomain}

    
    //public enum ResultUnit { dBm,dBc}

    public class PIMMeasCls
    {
        //public IPIMTester PIMTester;
        //public double CWFreq1 { get; set; }
        //public double CWFreq2 { get; set; }
        //public double CWPOW1 { get; set; }
        //public double CWPOW2 { get; set; }

        //public double TestStep { get; set; }//步进
        //public double TestTime { get; set; }//测试时间-秒

        //public double StartFreq1 { get; set; }
        //public double StopFreq1 { get; set; }

        //public double StartFreq2 { get; set; }
        //public double StopFreq2 { get; set; }

        //private string SweepFlag { get; set; }

        
        //public ResultUnit ResultUnit { get; set; }

        //public PIMMeasMode MeasMode { get; set; }

        
        
        //    public double Port1PathLoss { get; set; } //放在本地更合理
        //   public double Port2PathLoss { get; set; } //放在本地更合理

        

        //public PIMSidebandTypeEnum SideBand { get; set; }
        //public int PIMOrder { get; set; }
        //public PIMSweepToneEnum SweepTone { get; set; }

        //     public double Pow1Comp { get; set; }     //放在本地更合理
        //    public double Pow2Comp { get; set; }      //放在本地更合理
        //     public double[] Pow1Comps { get; set; } //放在本地更合理
        //    public double[] Pow2Comps { get; set; }  //放在本地更合理
        //public PIMCalType CalType { get; set; }
        

        /// <summary>
        /// 测量之前调用
        /// </summary>
        public void BeginMeas()
        {

            //setting to instru

        }
        /// <summary>
        /// 测量之后调用
        /// </summary>
        public void EndMeas()
        {
            
            
        }
        /// <summary>
        /// 完成一次测试
        /// </summary>
        public void Single()
        {
            ////setting to instru
            //PIMTester.setFreq(CWFreq1, CWFreq2);

            //PIMTester.setPow(CWPOW1, CWPOW2);

            //PIMTester.setMeasMode(MeasMode.ToString());

            //PIMTester.setUnit(ResultUnit.ToString());

            //PIMTester.setIMOrder(PIMOrder);

            //if (CalType == PIMCalType.Point)
            //{
            //    int num = PIMOrder / 2;
            //    double dblIMF = ((num + 1) * CWFreq1) - (num * CWFreq2);

            //    PIMTester.setModeStandard(dblIMF);

            //    PIMTester.setPowOn(true, true);

            //    List<XYData> lstRes = new List<XYData>();

            //    double[] strRes = PIMTester.getPointIMPow(CWPOW1);
            //    XYData xyData = new XYData();
            //    //赋值
            //    xyData.X = dblIMF;
            //    xyData.Y = strRes[2];

            //    lstRes.Add(xyData);

            //    PIMTestResult1 = (XYDataArr)lstRes;

            //}
            //else if (CalType == PIMCalType.Sweep)
            //{
            //    SweepFlag = PIMTester.getSweepFlag();

            //    int number = (PIMOrder / 2);
            //    double dblTestStep1 = TestStep * (number + 1f);
            //    double dblTestStep2 = TestStep * number;

            //    int intTotalCount1 = (SweepFlag == "F1UP" ? Convert.ToInt32((float)((StopFreq1 - StartFreq1) / dblTestStep1)) : Convert.ToInt32((float)((StopFreq2 - StartFreq2) / dblTestStep2)));
            //    int intTotalCount2 = (SweepFlag == "F1UP" ? Convert.ToInt32((float)((StopFreq2 - StartFreq2) / dblTestStep2)) : Convert.ToInt32((float)((StopFreq1 - StartFreq1) / dblTestStep1)));

            //    PIMTester.setModeSweepTx(TestStep, CWFreq1, CWFreq2);
            //    PIMTester.setPowOn(true, true);

            //    List<XYData> lstRes1 = new List<XYData>();
            //    List<XYData> lstRes2 = new List<XYData>();

            //    for (int i = 0; i < (intTotalCount1 + intTotalCount2 + 2); i++)
            //    {
            //        double dblNowFreq1 = 0;
            //        double dblNowFreq2 = 0;

            //        int j = 0;
            //        if (i <= intTotalCount1)
            //        {
            //            j = i;
            //            dblNowFreq1 = (SweepFlag == "F1UP" ? (StartFreq1 + i * TestStep) : StartFreq1);
            //            dblNowFreq2 = (SweepFlag == "F1UP" ? StopFreq2 : (StopFreq2 - j * TestStep));
            //        }
            //        else
            //        {
            //            j = (i - intTotalCount1 - 1);

            //            dblNowFreq1 = (SweepFlag == "F1UP" ? StartFreq1 : (StartFreq1 + i * TestStep));
            //            dblNowFreq2 = (SweepFlag == "F1UP" ? (StopFreq2 - j * TestStep) : StopFreq2);
            //        }

            //        double[] strRes = new double[4];
            //        if (ResultUnit == ResultUnit.dBm)
            //        {
            //            strRes = PIMTester.getSweepIMPow(PIMOrder, dblNowFreq1, dblNowFreq2, 0);
            //        }
            //        else
            //        {
            //            strRes = PIMTester.getSweepIMPow(PIMOrder, dblNowFreq1, dblNowFreq2, CWPOW1);
            //        }

            //        XYData xyData = new XYData();
            //        //赋值
            //        xyData.X = strRes[2];
            //        xyData.Y = strRes[3];

            //        if (i <= intTotalCount1)
            //        {
            //            if (SweepFlag == "F1UP")
            //            {
            //                lstRes1.Add(xyData);
            //            }
            //            else
            //            {
            //                lstRes2.Add(xyData);
            //            }
                        
            //        }
            //        else
            //        {
            //            if (SweepFlag == "F1UP")
            //            {
            //                lstRes2.Add(xyData);
            //            }
            //            else
            //            {
            //                lstRes1.Add(xyData);
            //            }
            //        }
            //    }

            //    PIMTestResult1 = (XYDataArr)lstRes1;
            //    PIMTestResult2 = (XYDataArr)lstRes2;
            //}
            //else
            //{
            //    int num = PIMOrder / 2;
            //    double dblIMF = ((num + 1) * CWFreq1) - (num * CWFreq2);

            //    PIMTester.setModeStandard(dblIMF);

            //    PIMTester.setPowOn(true, true);

            //    List<XYData> lstRes = new List<XYData>();

            //    DateTime TimeOld = System.DateTime.Now;

            //    while (System.DateTime.Now.Subtract(TimeOld).TotalSeconds < TestTime)
            //    {
            //        double[] strRes = PIMTester.getPointIMPow(CWPOW1);
            //        XYData xyData = new XYData();
            //        //赋值
            //        xyData.X = dblIMF;
            //        xyData.Y = strRes[2];

            //        lstRes.Add(xyData);
            //    }

            //    PIMTestResult1 = (XYDataArr)lstRes;
            //}








            ///
            //setting to instru
            //PIMTester.SetFreq(CWFreq1, CWFreq2);

            //PIMTester.SetPow(CWPOW1, CWPOW2);

            //PIMTester.SetIMOrder(PIMOrder.ToString());

            //PIMTester.SetMeasMode(MeasMode.ToString());

            //PIMTester.SetUnit(ResultUnit.ToString());

            //PIMTester.SetGainOffset(GainOffset);

            //if (CalType == PIMCalType.Point)
            //{
            //    int num = PIMOrder / 2;
            //    double dblIMF = ((num + 1) * CWFreq1) - (num * CWFreq2);

            //    PIMTester.SetModeStandard(dblIMF.ToString("0.00"));

            //    PIMTester.SetOutputSwitch(true, true);

            //    List<XYData> lstRes = new List<XYData>();

            //    string strRes = PIMTester.GetPointIMPow(CWPOW1);
            //    XYData xyData = new XYData();
            //    //赋值
            //    lstRes.Add(xyData);

            //    PIMTestResult = (XYDataArr)lstRes;

            //}
            //else if (CalType == PIMCalType.Sweep)
            //{
            //    PIMTester.SetModeSweepTx(TestStep, CWFreq1, CWFreq2, false);

            //    PIMTester.SetOutputSwitch(true, true);

            //    List<XYData> lstRes = new List<XYData>();
            //    for (int i = 0; i < 100; i++)
            //    {
            //        string strRes = PIMTester.GetSweepIMPow(PIMOrder, CWFreq1, CWFreq2, CWPOW1);
            //        XYData xyData = new XYData();
            //        //赋值
            //        lstRes.Add(xyData);
            //    }

            //    PIMTestResult = (XYDataArr)lstRes;
            //}
            //else
            //{
            //    int num = PIMOrder / 2;
            //    double dblIMF = ((num + 1) * CWFreq1) - (num * CWFreq2);

            //    PIMTester.SetModeStandard(dblIMF.ToString("0.00"));

            //    PIMTester.SetOutputSwitch(true, true);

            //    List<XYData> lstRes = new List<XYData>();

            //    DateTime TimeOld = System.DateTime.Now;

            //    while (System.DateTime.Now.Subtract(TimeOld).TotalSeconds < TestTime)
            //    {
            //        string strRes = PIMTester.GetPointIMPow(CWPOW1);
            //        XYData xyData = new XYData();
            //        //赋值
            //        lstRes.Add(xyData);
            //    }

            //    PIMTestResult = (XYDataArr)lstRes;
            //}

        }
        /// <summary>
        /// 一个方案加载的时候需要调用的
        /// </summary>
        public void InitOnce()
        {
            
        }
    }
}
