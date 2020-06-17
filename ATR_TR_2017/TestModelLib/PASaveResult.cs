using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ModelBaseLib;
using PIMModelLib;

namespace TestModelLib
{
    public class PASaveResult
    {
        //public static PAResult setPAResult(TestPlan testPlan, string SN, string strRemark, string Material, string ProcessId, string TerminalId)
        //{
        //    PAResult result = new PAResult();
        //    try
        //    {
        //        result.SN = SN;
        //        result.ATEKind = "ATEPA";
        //        result.Material = (Material == null ? "" : Material);
        //        result.TerminalId = TerminalId;
        //        result.ProcessId = (ProcessId == null ? "" : ProcessId);
        //        result.HostName = System.Net.Dns.GetHostName();
        //        result.TestUserID = (DataUtils.StaticInfo.LoginUser == null ? "" : DataUtils.StaticInfo.LoginUser);
        //        result.Remark = strRemark;
        //        result.SaveDateTime = DataUtils.StaticInfo.DBTime;

        //        int i = 0;

        //        string strDeviceModel = "";
        //        foreach (ManualConnection manualConnection in testPlan.ManualConnectionList)
        //        {
        //            foreach (TestStep testStep in manualConnection.TestStepList)
        //            {
        //                if (testStep as PIMTestStep != null)
        //                {
        //                    PIMTestStep PIMTS = testStep as PIMTestStep;
        //                    if (PIMTS.MeasInfo != null)
        //                    {
        //                        string strInstruInfo = PIMTS.MeasInfo.InstruInfoList[0].Vendor + "," + PIMTS.MeasInfo.InstruInfoList[0].Model + "," + PIMTS.MeasInfo.InstruInfoList[0].SerialNum;
        //                        if (!strDeviceModel.Contains(strInstruInfo))
        //                        {
        //                            strDeviceModel = strDeviceModel + ";" + strInstruInfo;
        //                        }
        //                    }

        //                    PIMStepData PIMSD = new PIMStepData();
        //                    i++;
        //                    PIMSD.STEPID = i.ToString();
        //                    PIMSD.STEPKind = "PIM";
        //                    PIMSD.ProName = manualConnection.Name;
        //                    PIMSD.PortName = PIMTS.PortName.ToString();
        //                    PIMSD.Band = (PIMTS.FreqRange == null ? "" : PIMTS.FreqRange);
        //                    PIMSD.RBandMin = (PIMTS.RXFreq1 / 1000000).ToString("f3");
        //                    PIMSD.RBandMax = (PIMTS.RXFreq2 / 1000000).ToString("f3");
        //                    PIMSD.CalType = PIMTS.CalType.ToString();
        //                    PIMSD.MeasMode = PIMTS.MeasMode.ToString();
        //                    PIMSD.Order = PIMTS.PIMOrder.ToString();
        //                    PIMSD.Freq1 = (PIMTS.CWFreq1 / 1000000).ToString("f3");
        //                    PIMSD.Freq2 = (PIMTS.CWFreq2 / 1000000).ToString("f3");
        //                    PIMSD.Pow1 = PIMTS.CWPOW1.ToString("f2");
        //                    PIMSD.Pow2 = PIMTS.CWPOW2.ToString("f2");
        //                    PIMSD.IMPow1 = PIMTS.PIMPower1.ToString("f2");
        //                    PIMSD.IMPow2 = PIMTS.PIMPower2.ToString("f2");

        //                    foreach (PIMTestTrace tr in PIMTS.TraceList)
        //                    {
        //                        //BUG HHX
        //                        if (tr.TypeName == PIMTestTraceType.PIMTrace1)
        //                        {
        //                            PIMSD.ResUnit = (tr.Unit == null ? "" : tr.Unit);

        //                            XYTestMarker xyTestMarker = tr.TestSpecList[0].TestMarkerList[0] as XYTestMarker;
        //                            PIMSD.Limit = xyTestMarker.Limit.YMax.ToString();

        //                            XYTraceResult traceRes = tr.TestResult as XYTraceResult;
        //                            if (traceRes.ResultData == null || traceRes.ResultData.Count == 0)
        //                            {
        //                                PIMSD.ResultFim1 = "0";
        //                                PIMSD.ResultValue1 = "0";
        //                            }
        //                            else
        //                            {
        //                                if (PIMTS.CalType == PIMCalType.Sweep)
        //                                {
        //                                    PIMSD.ResultFim1 = (xyTestMarker.MarkerResult[0].X / 1000000).ToString("f3");
        //                                }
        //                                else
        //                                {
        //                                    int num = PIMTS.PIMOrder / 2;
        //                                    double dblIMF = (((num + 1) * PIMTS.CWFreq1) - (num * PIMTS.CWFreq2));
        //                                    PIMSD.ResultFim1 = (dblIMF / 1000000).ToString("f3");
        //                                }

        //                                PIMSD.ResultValue1 = xyTestMarker.MarkerResult[0].Y.ToString("f2");
        //                            }
        //                            PIMSD.ResultFim2 = "0";
        //                            PIMSD.ResultValue2 = "0";
        //                        }
        //                        if (tr.TypeName == PIMTestTraceType.PIMTrace2)
        //                        {
        //                            if (PIMTS.CalType == PIMCalType.Sweep)
        //                            {
        //                                XYTraceResult traceRes = tr.TestResult as XYTraceResult;
        //                                if (traceRes.ResultData == null || traceRes.ResultData.Count == 0)
        //                                {
        //                                    PIMSD.ResultFim2 = "0";
        //                                    PIMSD.ResultValue2 = "0";
        //                                }
        //                                else
        //                                {
        //                                    XYTestMarker xyTestMarker = tr.TestSpecList[0].TestMarkerList[0] as XYTestMarker;
        //                                    PIMSD.ResultFim2 = (xyTestMarker.MarkerResult[0].X / 1000000).ToString("f3");
        //                                    PIMSD.ResultValue2 = xyTestMarker.MarkerResult[0].Y.ToString("f2");
        //                                }
        //                            }
        //                        }
        //                    }

        //                    PIMSD.Result = (PIMTS.PassFail == true ? "PASS" : "FAIL");
        //                    PIMSD.TestDateTime = (PIMTS.SingleDBTime == null ? "" : PIMTS.SingleDBTime);
        //                    PIMSD.isImage = PIMTS.isImageSave;
        //                    PIMSD.Image = (PIMTS.bImage == null ? new byte[] { } : PIMTS.bImage);

        //                    result.lstPIMStepData.Add(PIMSD);
        //                }

        //                if (testStep as IMDTestStep != null)
        //                {
        //                    IMDTestStep IMDTS = testStep as IMDTestStep;
        //                    if (IMDTS.MeasInfo != null)
        //                    {
        //                        string strInstruInfo = IMDTS.MeasInfo.InstruInfoList[0].Vendor + "," + IMDTS.MeasInfo.InstruInfoList[0].Model + "," + IMDTS.MeasInfo.InstruInfoList[0].SerialNum;
        //                        if (!strDeviceModel.Contains(strInstruInfo))
        //                        {
        //                            strDeviceModel = strDeviceModel + ";" + strInstruInfo;
        //                        }
        //                    }

        //                    foreach (IMDTestTrace tr in IMDTS.TraceList)
        //                    {
        //                        IMDStepData IMDSD = new IMDStepData();
        //                        i++;
        //                        IMDSD.STEPID = i.ToString();
        //                        IMDSD.STEPKind = "IP3";

        //                        IMDSD.PortName = manualConnection.Name;
        //                        IMDSD.CalType = IMDTS.SweepMode.ToString();
        //                        IMDSD.Order = IMDTS.Order.ToString();
        //                        IMDSD.Freq1 = IMDTS.F1.ToString();
        //                        IMDSD.Freq2 = IMDTS.F2.ToString();
        //                        IMDSD.Pow1 = IMDTS.F1Power.ToString();
        //                        IMDSD.Pow2 = IMDTS.F2Power.ToString();
        //                        IMDSD.SAATT = IMDTS.RecvAtten.ToString();
        //                        IMDSD.SARBW = IMDTS.MeasBandwidth.ToString();

        //                        IMDSD.ResUnit = (tr.Unit == null ? "" : tr.Unit);

        //                        XYTestMarker xyTestMarker = tr.TestSpecList[0].TestMarkerList[0] as XYTestMarker;
        //                        IMDSD.Limit = xyTestMarker.Limit.YMax.ToString();

        //                        XYTraceResult traceRes = tr.TestResult as XYTraceResult;
        //                        if (traceRes.ResultData == null || traceRes.ResultData.Count == 0)
        //                        {
        //                            IMDSD.ResultFim = "0";
        //                            IMDSD.ResultValue = "0";
        //                        }
        //                        else
        //                        {
        //                            IMDSD.ResultFim = (xyTestMarker.MarkerResult[0].X / 1000000).ToString("f3");
        //                            IMDSD.ResultValue = xyTestMarker.MarkerResult[0].Y.ToString("f2");
        //                        }

        //                        IMDSD.Result = (IMDTS.PassFail == true ? "PASS" : "FAIL");
        //                        //IMDSD.TestDateTime = (IMDTS.SingleDBTime == null ? "" : IMDTS.SingleDBTime);
        //                        //IMDSD.isImage = IMDTS.isImageSave;
        //                        IMDSD.Image = (IMDTS.bImage == null ? new byte[] { } : IMDTS.bImage);

        //                        result.lstIMDStepData.Add(IMDSD);
        //                    }

                            
        //                }

        //                if (testStep as NFTestStep != null)
        //                {
        //                    NFTestStep NFTS = testStep as NFTestStep;
        //                    if (NFTS.MeasInfo != null)
        //                    {
        //                        string strInstruInfo = NFTS.MeasInfo.InstruInfoList[0].Vendor + "," + NFTS.MeasInfo.InstruInfoList[0].Model + "," + NFTS.MeasInfo.InstruInfoList[0].SerialNum;
        //                        if (!strDeviceModel.Contains(strInstruInfo))
        //                        {
        //                            strDeviceModel = strDeviceModel + ";" + strInstruInfo;
        //                        }
        //                    }

        //                    foreach (NFTestTrace tr in NFTS.TraceList)
        //                    {
        //                        NFStepData NFSD = new NFStepData();
        //                        i++;

        //                        NFSD.STEPID = i.ToString();
        //                        NFSD.STEPKind = "NF";
        //                        NFSD.PortName = manualConnection.Name;
        //                        NFSD.StartFreq = NFTS.StartFreq.ToString();
        //                        NFSD.StopFreq = NFTS.StopFreq.ToString();
        //                        NFSD.SweepPoints = NFTS.SweepPoints.ToString();
        //                        NFSD.MeasBandwidth = NFTS.MeasBandwidth.ToString();
        //                        NFSD.AverageCount = NFTS.AverageCount.ToString();
        //                        NFSD.TypeName = tr.TypeName.ToString();
        //                        NFSD.Compensation = tr.Compensation.ToString();

        //                        XYTestMarker xyTestMarker = tr.TestSpecList[0].TestMarkerList[0] as XYTestMarker;
        //                        NFSD.Limit = xyTestMarker.Limit.YMax.ToString();

        //                        XYTraceResult traceRes = tr.TestResult as XYTraceResult;
        //                        if (traceRes.ResultData == null || traceRes.ResultData.Count == 0)
        //                        {
        //                            NFSD.ResultFim = "0";
        //                            NFSD.ResultValue = "0";
        //                        }
        //                        else
        //                        {
        //                            NFSD.ResultFim = (xyTestMarker.MarkerResult[0].X / 1000000).ToString("f3");
        //                            NFSD.ResultValue = xyTestMarker.MarkerResult[0].Y.ToString("f2");
        //                        }

        //                        NFSD.Result = (NFTS.PassFail == true ? "PASS" : "FAIL");
        //                        //NFSD.TestDateTime = (NFTS.SingleDBTime == null ? "" : NFTS.SingleDBTime);
        //                        //NFSD.isImage = NFTS.isImageSave;
        //                        NFSD.Image = (NFTS.bImage == null ? new byte[] { } : NFTS.bImage);

        //                        result.lstNFStepData.Add(NFSD);
        //                    }
        //                }
        //            }
        //        }

        //        string strResultForHead = "PASS";
        //        foreach (ManualConnection manualConnection in testPlan.ManualConnectionList)
        //        {
        //            foreach (TestStep testStep in manualConnection.TestStepList)
        //            {
        //                if (testStep.PassFail == false || testStep.PassFail == null)
        //                {
        //                    strResultForHead = "FAIL";
        //                    break;
        //                }
        //            }
        //        }

        //        result.ResultForHead = strResultForHead;
        //        result.DeviceModel = strDeviceModel;
        //    }
        //    catch (Exception ex)
        //    {
                
        //    }
        //    return result;
        //}

        public static BaseObject setPAResult(TestPlan testPlan, string SN, string strRemark, string Material, string ProcessId, string TerminalId)
        {
            BaseObject result = new BaseObject();
            try
            {
                Datalist HeadDataSN = new Datalist() { paramName = "SN", paramValue = SN };
                result.HeadData.Add(HeadDataSN);

                Datalist HeadDataATEKind = new Datalist() { paramName = "ATEKind", paramValue = "ATEPA" };
                result.HeadData.Add(HeadDataATEKind);

                Datalist HeadDataMaterial = new Datalist() { paramName = "Material", paramValue = (Material == null ? "" : Material) };
                result.HeadData.Add(HeadDataMaterial);

                Datalist HeadDataTerminalId = new Datalist() { paramName = "TerminalId", paramValue = TerminalId };
                result.HeadData.Add(HeadDataTerminalId);

                Datalist HeadDataProcessId = new Datalist() { paramName = "ProcessId", paramValue = (ProcessId == null ? "" : ProcessId) };
                result.HeadData.Add(HeadDataProcessId);

                Datalist HeadDataHostName = new Datalist() { paramName = "HostName", paramValue = System.Net.Dns.GetHostName() };
                result.HeadData.Add(HeadDataHostName);

                Datalist HeadDataTestUserID = new Datalist() { paramName = "TestUserID", paramValue = (DataUtils.StaticInfo.LoginUser == null ? "" : DataUtils.StaticInfo.LoginUser) };
                result.HeadData.Add(HeadDataTestUserID);

                Datalist HeadDataRemark = new Datalist() { paramName = "Remark", paramValue = strRemark };
                result.HeadData.Add(HeadDataRemark);

                Datalist HeadDataSaveDateTime = new Datalist() { paramName = "SaveDateTime", paramValue = DataUtils.StaticInfo.DBTime };
                result.HeadData.Add(HeadDataSaveDateTime);

                int i = 0;
                string strDeviceModel = "";
                foreach (ManualConnection manualConnection in testPlan.ManualConnectionList)
                {
                    foreach (TestStep testStep in manualConnection.TestStepList)
                    {
                        if (testStep as PIMTestStep != null)
                        {
                            Stepdata stepData = new Stepdata();
                            i++;

                            PIMTestStep PIMTS = testStep as PIMTestStep;
                            if (PIMTS.MeasInfo != null)
                            {
                                string strInstruInfo = PIMTS.MeasInfo.InstruInfoList[0].Vendor + "," + PIMTS.MeasInfo.InstruInfoList[0].Model + "," + PIMTS.MeasInfo.InstruInfoList[0].SerialNum;
                                if (!strDeviceModel.Contains(strInstruInfo))
                                {
                                    strDeviceModel = strDeviceModel + ";" + strInstruInfo;
                                }
                            }

                            Datalist stepDataSTEPID = new Datalist() { paramName = "STEPID", paramValue = i.ToString() };
                            stepData.Datalist.Add(stepDataSTEPID);
                            Datalist stepDataSTEPKind = new Datalist() { paramName = "STEPKind", paramValue = "PIM" };
                            stepData.Datalist.Add(stepDataSTEPKind);
                            Datalist stepDataProName = new Datalist() { paramName = "ProName", paramValue = manualConnection.Name };
                            stepData.Datalist.Add(stepDataProName);
                            Datalist stepDataPortName = new Datalist() { paramName = "PortName", paramValue = PIMTS.PortName.ToString() };
                            stepData.Datalist.Add(stepDataPortName);
                            Datalist stepDataBand = new Datalist() { paramName = "Band", paramValue = (PIMTS.FreqRange == null ? "" : PIMTS.FreqRange) };
                            stepData.Datalist.Add(stepDataBand);
                            Datalist stepDataRBandMin = new Datalist() { paramName = "RBandMin", paramValue = (PIMTS.RXFreq1 / 1000000).ToString("f3") };
                            stepData.Datalist.Add(stepDataRBandMin);
                            Datalist stepDataRBandMax = new Datalist() { paramName = "RBandMax", paramValue = (PIMTS.RXFreq2 / 1000000).ToString("f3") };
                            stepData.Datalist.Add(stepDataRBandMax);
                            Datalist stepDataCalType = new Datalist() { paramName = "CalType", paramValue = PIMTS.CalType.ToString() };
                            stepData.Datalist.Add(stepDataCalType);
                            Datalist stepDataMeasMode = new Datalist() { paramName = "MeasMode", paramValue = PIMTS.MeasMode.ToString() };
                            stepData.Datalist.Add(stepDataMeasMode);
                            Datalist stepDataOrder = new Datalist() { paramName = "Order", paramValue = PIMTS.PIMOrder.ToString() };
                            stepData.Datalist.Add(stepDataOrder);
                            Datalist stepDataFreq1 = new Datalist() { paramName = "Freq1", paramValue = (PIMTS.CWFreq1 / 1000000).ToString("f3") };
                            stepData.Datalist.Add(stepDataFreq1);
                            Datalist stepDataFreq2 = new Datalist() { paramName = "Freq2", paramValue = (PIMTS.CWFreq2 / 1000000).ToString("f3") };
                            stepData.Datalist.Add(stepDataFreq2);
                            Datalist stepDataPow1 = new Datalist() { paramName = "Pow1", paramValue = PIMTS.CWPOW1.ToString("f2") };
                            stepData.Datalist.Add(stepDataPow1);
                            Datalist stepDataPow2 = new Datalist() { paramName = "Pow2", paramValue = PIMTS.CWPOW2.ToString("f2") };
                            stepData.Datalist.Add(stepDataPow2);
                            Datalist stepDataIMPow1 = new Datalist() { paramName = "IMPow1", paramValue = PIMTS.PIMPower1.ToString("f2") };
                            stepData.Datalist.Add(stepDataIMPow1);
                            Datalist stepDataIMPow2 = new Datalist() { paramName = "IMPow2", paramValue = PIMTS.PIMPower2.ToString("f2") };
                            stepData.Datalist.Add(stepDataIMPow2);

                            foreach (PIMTestTrace tr in PIMTS.TraceList)
                            {
                                //BUG HHX
                                if (tr.TypeName == PIMTestTraceType.PIMTrace1)
                                {
                                    Datalist stepDataResUnit = new Datalist() { paramName = "ResUnit", paramValue = (tr.Unit == null ? "" : tr.Unit) };
                                    stepData.Datalist.Add(stepDataResUnit);

                                    XYTestMarker xyTestMarker = tr.TestSpecList[0].TestMarkerList[0] as XYTestMarker;
                                    Datalist stepDataLimit = new Datalist() { paramName = "Limit", paramValue = xyTestMarker.Limit.YMax.ToString() };
                                    stepData.Datalist.Add(stepDataLimit);

                                    XYTraceResult traceRes = tr.TestResult as XYTraceResult;

                                    if (traceRes.ResultData == null || traceRes.ResultData.Count == 0)
                                    {
                                        Datalist stepDataResultFim1 = new Datalist() { paramName = "ResultFim1", paramValue = "0" };
                                        stepData.Datalist.Add(stepDataResultFim1);
                                        Datalist stepDataResultValue1 = new Datalist() { paramName = "ResultValue1", paramValue = "0" };
                                        stepData.Datalist.Add(stepDataResultValue1);
                                    }
                                    else
                                    {
                                        if (PIMTS.CalType == PIMCalType.Sweep)
                                        {
                                            Datalist stepDataResultFim1 = new Datalist() { paramName = "ResultFim1", paramValue = (xyTestMarker.MarkerResult[0].X / 1000000).ToString("f3") };
                                            stepData.Datalist.Add(stepDataResultFim1);
                                        }
                                        else
                                        {
                                            int num = PIMTS.PIMOrder / 2;
                                            double dblIMF = (((num + 1) * PIMTS.CWFreq1) - (num * PIMTS.CWFreq2));
                                            Datalist stepDataResultFim1 = new Datalist() { paramName = "ResultFim1", paramValue = (dblIMF / 1000000).ToString("f3") };
                                            stepData.Datalist.Add(stepDataResultFim1);
                                        }

                                        Datalist stepDataResultValue1 = new Datalist() { paramName = "ResultValue1", paramValue = xyTestMarker.MarkerResult[0].Y.ToString("f2") };
                                        stepData.Datalist.Add(stepDataResultValue1);
                                    }
                                }

                                if (tr.TypeName == PIMTestTraceType.PIMTrace2)
                                {
                                    if (PIMTS.CalType == PIMCalType.Sweep)
                                    {
                                        XYTraceResult traceRes = tr.TestResult as XYTraceResult;
                                        if (traceRes.ResultData == null || traceRes.ResultData.Count == 0)
                                        {
                                            Datalist stepDataResultFim2 = new Datalist() { paramName = "ResultFim2", paramValue = "0" };
                                            stepData.Datalist.Add(stepDataResultFim2);
                                            Datalist stepDataResultValue2 = new Datalist() { paramName = "ResultValue2", paramValue = "0" };
                                            stepData.Datalist.Add(stepDataResultValue2);
                                        }
                                        else
                                        {
                                            XYTestMarker xyTestMarker = tr.TestSpecList[0].TestMarkerList[0] as XYTestMarker;
                                            Datalist stepDataResultFim2 = new Datalist() { paramName = "ResultFim2", paramValue = (xyTestMarker.MarkerResult[0].X / 1000000).ToString("f3") };
                                            stepData.Datalist.Add(stepDataResultFim2);
                                            Datalist stepDataResultValue2 = new Datalist() { paramName = "ResultValue2", paramValue = xyTestMarker.MarkerResult[0].Y.ToString("f2") };
                                            stepData.Datalist.Add(stepDataResultValue2);
                                        }
                                    }
                                }
                            }

                            Datalist stepDataResult = new Datalist() { paramName = "Result", paramValue = (PIMTS.PassFail == true ? "PASS" : "FAIL") };
                            stepData.Datalist.Add(stepDataResult);
                            Datalist stepDataTestDateTime = new Datalist() { paramName = "TestDateTime", paramValue = (PIMTS.SingleDBTime == null ? "" : PIMTS.SingleDBTime) };
                            stepData.Datalist.Add(stepDataTestDateTime);

                            if (PIMTS.isImageSave)
                            {
                                Imagelist stepDataImage = new Imagelist() { Index = "1", ImageName = i.ToString() + "-"+manualConnection.Name, ImageKind = "png", ImageString = Convert.ToBase64String(PIMTS.bImage) };
                                stepData.Imagelist.Add(stepDataImage);
                            }

                            result.StepData.Add(stepData);
                        }

                        if (testStep as IMDTestStep != null)
                        {
                            Stepdata stepData = new Stepdata();
                            i++;

                            IMDTestStep IMDTS = testStep as IMDTestStep;
                            if (IMDTS.MeasInfo != null)
                            {
                                string strInstruInfo = IMDTS.MeasInfo.InstruInfoList[0].Vendor + "," + IMDTS.MeasInfo.InstruInfoList[0].Model + "," + IMDTS.MeasInfo.InstruInfoList[0].SerialNum;
                                if (!strDeviceModel.Contains(strInstruInfo))
                                {
                                    strDeviceModel = strDeviceModel + ";" + strInstruInfo;
                                }
                            }

                            foreach (IMDTestTrace tr in IMDTS.TraceList)
                            {
                                Datalist stepDataSTEPID = new Datalist() { paramName = "STEPID", paramValue = i.ToString() };
                                stepData.Datalist.Add(stepDataSTEPID);
                                Datalist stepDataSTEPKind = new Datalist() { paramName = "STEPKind", paramValue = "IP3" };
                                stepData.Datalist.Add(stepDataSTEPKind);
                                Datalist stepDataProName = new Datalist() { paramName = "ProName", paramValue = manualConnection.Name };
                                stepData.Datalist.Add(stepDataProName);
                                Datalist stepDataCalType = new Datalist() { paramName = "CalType", paramValue = IMDTS.SweepMode.ToString() };
                                stepData.Datalist.Add(stepDataCalType);
                                Datalist stepDataOrdere = new Datalist() { paramName = "Order", paramValue = IMDTS.Order.ToString() };
                                stepData.Datalist.Add(stepDataOrdere);
                                Datalist stepDataFreq1 = new Datalist() { paramName = "Freq1", paramValue = IMDTS.F1.ToString() };
                                stepData.Datalist.Add(stepDataFreq1);
                                Datalist stepDataFreq2 = new Datalist() { paramName = "Freq2", paramValue = IMDTS.F2.ToString() };
                                stepData.Datalist.Add(stepDataFreq2);
                                Datalist stepDataPow1 = new Datalist() { paramName = "Pow1", paramValue = IMDTS.F1Power.ToString() };
                                stepData.Datalist.Add(stepDataPow1);
                                Datalist stepDataPow2 = new Datalist() { paramName = "Pow2", paramValue = IMDTS.F2Power.ToString() };
                                stepData.Datalist.Add(stepDataPow2);
                                Datalist stepDataSAATT = new Datalist() { paramName = "SAATT", paramValue = IMDTS.RecvAtten.ToString() };
                                stepData.Datalist.Add(stepDataSAATT);
                                Datalist stepDataSARBW = new Datalist() { paramName = "SARBW", paramValue = IMDTS.MeasBandwidth.ToString() };
                                stepData.Datalist.Add(stepDataSARBW);
                                Datalist stepDataResUnit = new Datalist() { paramName = "ResUnit", paramValue = (tr.Unit == null ? "" : tr.Unit) };
                                stepData.Datalist.Add(stepDataResUnit);

                                XYTestMarker xyTestMarker = tr.TestSpecList[0].TestMarkerList[0] as XYTestMarker;
                                Datalist stepDataLimit = new Datalist() { paramName = "Limit", paramValue = xyTestMarker.Limit.YMax.ToString() };
                                stepData.Datalist.Add(stepDataLimit);

                                XYTraceResult traceRes = tr.TestResult as XYTraceResult;
                                if (traceRes.ResultData == null || traceRes.ResultData.Count == 0)
                                {
                                    Datalist stepDataResultFim = new Datalist() { paramName = "ResultFim", paramValue = "0" };
                                    stepData.Datalist.Add(stepDataResultFim);
                                    Datalist stepDataResultValue = new Datalist() { paramName = "ResultValue", paramValue = "0" };
                                    stepData.Datalist.Add(stepDataResultValue);
                                }
                                else
                                {
                                    Datalist stepDataResultFim = new Datalist() { paramName = "ResultFim", paramValue = (xyTestMarker.MarkerResult[0].X / 1000000).ToString("f3") };
                                    stepData.Datalist.Add(stepDataResultFim);
                                    Datalist stepDataResultValue = new Datalist() { paramName = "ResultValue", paramValue = xyTestMarker.MarkerResult[0].Y.ToString("f2") };
                                    stepData.Datalist.Add(stepDataResultValue);
                                }

                                Datalist stepDataResult = new Datalist() { paramName = "Result", paramValue = (IMDTS.PassFail == true ? "PASS" : "FAIL") };
                                stepData.Datalist.Add(stepDataResult);

                                //if (IMDTS.isImageSave)
                                //{
                                //    Imagelist stepDataImage = new Imagelist() { Index = "1", ImageName = i.ToString() + "-" + manualConnection.Name, ImageKind = "png", ImageString = Convert.ToBase64String(IMDTS.bImage) };
                                //    stepData.lstImage.Add(stepDataImage);
                                //}

                                result.StepData.Add(stepData);
                            }


                        }

                        if (testStep as NFTestStep != null)
                        {
                            Stepdata stepData = new Stepdata();
                            i++;

                            NFTestStep NFTS = testStep as NFTestStep;
                            if (NFTS.MeasInfo != null)
                            {
                                string strInstruInfo = NFTS.MeasInfo.InstruInfoList[0].Vendor + "," + NFTS.MeasInfo.InstruInfoList[0].Model + "," + NFTS.MeasInfo.InstruInfoList[0].SerialNum;
                                if (!strDeviceModel.Contains(strInstruInfo))
                                {
                                    strDeviceModel = strDeviceModel + ";" + strInstruInfo;
                                }
                            }

                            foreach (NFTestTrace tr in NFTS.TraceList)
                            {
                                Datalist stepDataSTEPID = new Datalist() { paramName = "STEPID", paramValue = i.ToString() };
                                stepData.Datalist.Add(stepDataSTEPID);
                                Datalist stepDataSTEPKind = new Datalist() { paramName = "STEPKind", paramValue = "NF" };
                                stepData.Datalist.Add(stepDataSTEPKind);
                                Datalist stepDataProName = new Datalist() { paramName = "ProName", paramValue = manualConnection.Name };
                                stepData.Datalist.Add(stepDataProName);



                                Datalist stepDataStartFreq = new Datalist() { paramName = "StartFreq", paramValue = NFTS.StartFreq.ToString() };
                                stepData.Datalist.Add(stepDataStartFreq);
                                Datalist stepDataStopFreq = new Datalist() { paramName = "StopFreq", paramValue = NFTS.StopFreq.ToString() };
                                stepData.Datalist.Add(stepDataStopFreq);
                                Datalist stepDataSweepPoints = new Datalist() { paramName = "SweepPoints", paramValue = NFTS.SweepPoints.ToString() };
                                stepData.Datalist.Add(stepDataSweepPoints);
                                Datalist stepDataMeasBandwidth = new Datalist() { paramName = "MeasBandwidth", paramValue = NFTS.MeasBandwidth.ToString() };
                                stepData.Datalist.Add(stepDataMeasBandwidth);
                                Datalist stepDataAverageCount = new Datalist() { paramName = "AverageCount", paramValue = NFTS.AverageCount.ToString() };
                                stepData.Datalist.Add(stepDataAverageCount);
                                Datalist stepDataTypeName = new Datalist() { paramName = "TypeName", paramValue = tr.TypeName.ToString() };
                                stepData.Datalist.Add(stepDataTypeName);
                                Datalist stepDataCompensation = new Datalist() { paramName = "Compensation", paramValue = tr.Compensation.ToString() };
                                stepData.Datalist.Add(stepDataCompensation);

                                XYTestMarker xyTestMarker = tr.TestSpecList[0].TestMarkerList[0] as XYTestMarker;
                                Datalist stepDataLimit = new Datalist() { paramName = "Limit", paramValue = xyTestMarker.Limit.YMax.ToString() };
                                stepData.Datalist.Add(stepDataLimit);

                                XYTraceResult traceRes = tr.TestResult as XYTraceResult;
                                if (traceRes.ResultData == null || traceRes.ResultData.Count == 0)
                                {
                                    Datalist stepDataResultFim = new Datalist() { paramName = "ResultFim", paramValue = "0" };
                                    stepData.Datalist.Add(stepDataResultFim);
                                    Datalist stepDataResultValue = new Datalist() { paramName = "ResultValue", paramValue = "0" };
                                    stepData.Datalist.Add(stepDataResultValue);
                                }
                                else
                                {
                                    Datalist stepDataResultFim = new Datalist() { paramName = "ResultFim", paramValue = (xyTestMarker.MarkerResult[0].X / 1000000).ToString("f3") };
                                    stepData.Datalist.Add(stepDataResultFim);
                                    Datalist stepDataResultValue = new Datalist() { paramName = "ResultValue", paramValue = xyTestMarker.MarkerResult[0].Y.ToString("f2") };
                                    stepData.Datalist.Add(stepDataResultValue);
                                }

                                Datalist stepDataResult = new Datalist() { paramName = "Result", paramValue = (NFTS.PassFail == true ? "PASS" : "FAIL") };
                                stepData.Datalist.Add(stepDataResult);

                                //if (NFTS.isImageSave)
                                //{
                                //    Imagelist stepDataImage = new Imagelist() { Index = "1", ImageName = i.ToString() + "-" + manualConnection.Name, ImageKind = "png", ImageString = Convert.ToBase64String(NFTS.bImage) };
                                //    stepData.lstImage.Add(stepDataImage);
                                //}

                                result.StepData.Add(stepData);
                            }
                        }

                        
                    }
                }

                string strResultForHead = "PASS";
                foreach (ManualConnection manualConnection in testPlan.ManualConnectionList)
                {
                    foreach (TestStep testStep in manualConnection.TestStepList)
                    {
                        if (testStep.PassFail == false || testStep.PassFail == null)
                        {
                            strResultForHead = "FAIL";
                            break;
                        }
                    }
                }

                Datalist HeadDataResultForHead = new Datalist() { paramName = "ResultForHead", paramValue = strResultForHead };
                result.HeadData.Add(HeadDataResultForHead);

                Datalist HeadDataDeviceModel = new Datalist() { paramName = "DeviceModel", paramValue = strDeviceModel };
                result.HeadData.Add(HeadDataDeviceModel);
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public static bool savePAResult(string Token, string SN, BaseObject PAResult)
        {
            string strPAResult = LitJson.JsonMapper.ToJson(PAResult);
            string res = ServiceInterface.DataService.CheckOUTBYSN(Token, SN, strPAResult);

            if (!res.StartsWith("ER"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }


    //public class PAResult
    //{
    //    public string SN { get; set; }
    //    public string ATEKind { get; set; }
    //    public string Material { get; set; }
    //    public string TerminalId { get; set; }
    //    public string ProcessId { get; set; }
    //    public string HostName { get; set; }
    //    public string ResultForHead { get; set; }
    //    public string DeviceModel { get; set; }
    //    public string TestUserID { get; set; }
    //    public string Remark { get; set; }
    //    public string SaveDateTime { get; set; }

    //    public List<PIMStepData> lstPIMStepData { get; set; }
    //    public List<IMDStepData> lstIMDStepData { get; set; }
    //    public List<NFStepData> lstNFStepData { get; set; }

    //    public PAResult()
    //    {
    //        lstPIMStepData = new List<PIMStepData>();
    //        lstIMDStepData = new List<IMDStepData>();
    //        lstNFStepData = new List<NFStepData>();
    //    }
    //}

    //public class PIMStepData
    //{
    //    public string STEPID { get; set; }
    //    public string STEPKind { get; set; }
    //    public string ProName { get; set; }
    //    public string PortName { get; set; }
    //    public string Band { get; set; }
    //    public string RBandMin { get; set; }
    //    public string RBandMax { get; set; }
    //    public string CalType { get; set; }
    //    public string MeasMode { get; set; }
    //    public string Order { get; set; }
    //    public string Freq1 { get; set; }
    //    public string Freq2 { get; set; }
    //    public string Pow1 { get; set; }
    //    public string Pow2 { get; set; }
    //    public string Limit { get; set; }
    //    public string IMPow1 { get; set; }
    //    public string IMPow2 { get; set; }
    //    public string ResultFim1 { get; set; }
    //    public string ResultFim2 { get; set; }
    //    public string ResultValue1 { get; set; }
    //    public string ResultValue2 { get; set; }
    //    public string ResUnit { get; set; }
    //    public string Result { get; set; }
    //    public string TestDateTime { get; set; }
    //    public bool isImage { get; set; }
    //    public byte[] Image { get; set; }
    //}

    //public class IMDStepData
    //{
    //    public string STEPID { get; set; }
    //    public string STEPKind { get; set; }
    //    public string ProName { get; set; }
    //    public string CalType { get; set; }
    //    public string Order { get; set; }
    //    public string Freq1 { get; set; }
    //    public string Freq2 { get; set; }
    //    public string Pow1 { get; set; }
    //    public string Pow2 { get; set; }
    //    public string SAATT { get; set; }
    //    public string SARBW { get; set; }
    //    public string Limit { get; set; }
    //    public string ResUnit { get; set; }

    //    public string ResultFim { get; set; }
    //    public string ResultValue { get; set; }

    //    public string Result { get; set; }
    //    public string TestDateTime { get; set; }
    //    public bool isImage { get; set; }
    //    public byte[] Image { get; set; }
    //}

    //public class NFStepData
    //{
    //    public string STEPID { get; set; }
    //    public string STEPKind { get; set; }
    //    public string ProName { get; set; }
    //    public string StartFreq { get; set; }
    //    public string StopFreq { get; set; }
    //    public string SweepPoints { get; set; }
    //    public string MeasBandwidth { get; set; }
    //    public string AverageCount { get; set; }
    //    public string TypeName { get; set; }
    //    public string Compensation { get; set; }
    //    public string Limit { get; set; }
    //    public string ResultFim { get; set; }
    //    public string ResultValue { get; set; }
    //    public string Result { get; set; }
    //    public string TestDateTime { get; set; }
    //    public bool isImage { get; set; }
    //    public byte[] Image { get; set; }
    //}



    public class BaseObject
    {
        public List<Datalist> HeadData { get; set; }
        public List<Stepdata> StepData { get; set; }

        public BaseObject()
        {
            HeadData = new List<Datalist>();
            StepData = new List<Stepdata>();
        }
    }

    public class Stepdata
    {
        public string INDEXID { get; set; }
        public List<Datalist> Datalist { get; set; }
        public List<Imagelist> Imagelist { get; set; }

        public Stepdata()
        {
            Datalist = new List<Datalist>();
            Imagelist = new List<Imagelist>();
        }
    }

    public class Datalist
    {
        public string paramName { get; set; }
        public string paramValue { get; set; }
    }

    public class Imagelist
    {
        public string Index { get; set; }
        public string ImageName { get; set; }
        public string ImageKind { get; set; }
        public string ImageString { get; set; }
    }


    
}
