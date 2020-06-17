using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ModelBaseLib;

using ATE2MES;

namespace PIMModelLib
{
    public class PIMSaveResult
    {
        public static List<PIMResult> setPIMResult(TestPlan testPlan, string SN, string strRemark, string Material, string ProcessId)
        {
            List<PIMResult> lstResult = new List<PIMResult>();
            try
            {
                PIMResult result = new PIMResult();
                result.SN = SN;
                result.ATEKind = "ATEPIM";
                result.Material = (Material == null ? "" : Material);
                result.ProcessId = (ProcessId == null ? "" : ProcessId);

                result.TestUserID = (DataUtils.StaticInfo.LoginUser == null ? "" : DataUtils.StaticInfo.LoginUser);
                result.Remark = strRemark;
                result.SaveDateTime = DataUtils.StaticInfo.DBTime;

                int i = 0;

                string strDeviceModel = "";
                foreach (ManualConnection manualConnection in testPlan.ManualConnectionList)
                {
                    foreach (TestStep testStep in manualConnection.TestStepList)
                    {
                        PIMTestStep PTestStep = (PIMTestStep)testStep;
                        if (PTestStep.MeasInfo != null)
                        {
                            strDeviceModel = strDeviceModel + ";" + PTestStep.MeasInfo.InstruInfoList[0].Vendor + "," + PTestStep.MeasInfo.InstruInfoList[0].Model + "," + PTestStep.MeasInfo.InstruInfoList[0].SerialNum;
                        }
                        
                        StepData stepData = new StepData();
                        i++;
                        stepData.STEPID = i.ToString();
                        stepData.ProName = manualConnection.Name;
                        stepData.PortName = PTestStep.PortName.ToString();
                        stepData.Band = (PTestStep.FreqRange == null ? "" : PTestStep.FreqRange);
                        //stepData.Band = "";
                        stepData.RBandMin = (PTestStep.RXFreq1 / 1000000).ToString("f3");
                        stepData.RBandMax = (PTestStep.RXFreq2 / 1000000).ToString("f3");
                        stepData.CalType = PTestStep.CalType.ToString();
                        stepData.MeasMode = PTestStep.MeasMode.ToString();
                        stepData.Order = PTestStep.PIMOrder.ToString();
                        stepData.Freq1 = (PTestStep.CWFreq1 / 1000000).ToString("f3");
                        stepData.Freq2 = (PTestStep.CWFreq2 / 1000000).ToString("f3");
                        stepData.Pow1 = PTestStep.CWPOW1.ToString("f2");
                        stepData.Pow2 = PTestStep.CWPOW2.ToString("f2");
                        stepData.IMPow1 = PTestStep.PIMPower1.ToString("f2");
                        stepData.IMPow2 = PTestStep.PIMPower2.ToString("f2");

                        foreach (PIMTestTrace tr in PTestStep.TraceList)
                        {
                            //BUG HHX
                            if (tr.TypeName == PIMTestTraceType.PIMTrace1)
                            {
                                stepData.ResUnit = (tr.Unit == null ? "" : tr.Unit);

                                XYTestMarker xyTestMarker = tr.TestSpecList[0].TestMarkerList[0] as XYTestMarker;
                                stepData.Limit = xyTestMarker.Limit.YMax.ToString();

                                XYTraceResult traceRes = tr.TestResult as XYTraceResult;
                                if (traceRes.ResultData == null || traceRes.ResultData.Count == 0)
                                {
                                    stepData.ResultFim1 = "0";
                                    stepData.ResultValue1 = "0";
                                }
                                else
                                {
                                    if (PTestStep.CalType == PIMCalType.Sweep)
                                    {
                                        stepData.ResultFim1 = (xyTestMarker.MarkerResult[0].X / 1000000).ToString("f3");
                                    }
                                    else
                                    {
                                        int num = PTestStep.PIMOrder / 2;
                                        double dblIMF = (((num + 1) * PTestStep.CWFreq1) - (num * PTestStep.CWFreq2));
                                        stepData.ResultFim1 = (dblIMF / 1000000).ToString("f3");
                                    }

                                    stepData.ResultValue1 = xyTestMarker.MarkerResult[0].Y.ToString("f2");
                                }
                                stepData.ResultFim2 = "0";
                                stepData.ResultValue2 = "0";
                            }
                            if (tr.TypeName == PIMTestTraceType.PIMTrace2)
                            {
                                if (PTestStep.CalType == PIMCalType.Sweep)
                                {
                                    XYTraceResult traceRes = tr.TestResult as XYTraceResult;
                                    if (traceRes.ResultData == null || traceRes.ResultData.Count == 0)
                                    {
                                        stepData.ResultFim2 = "0";
                                        stepData.ResultValue2 = "0";
                                    }
                                    else
                                    {
                                        XYTestMarker xyTestMarker = tr.TestSpecList[0].TestMarkerList[0] as XYTestMarker;
                                        stepData.ResultFim2 = (xyTestMarker.MarkerResult[0].X / 1000000).ToString("f3");
                                        stepData.ResultValue2 = xyTestMarker.MarkerResult[0].Y.ToString("f2");
                                    }
                                }
                            }
                        }

                        stepData.Result = (PTestStep.PassFail == true ? "PASS" : "FAIL");
                        stepData.TestDateTime = (PTestStep.SingleDBTime == null ? "" : PTestStep.SingleDBTime);
                        stepData.isImage = PTestStep.isImageSave;
                        stepData.Image = (PTestStep.bImage == null ? new byte[] { } : PTestStep.bImage);

                        result.lstStepData.Add(stepData);
                    }




                }

                string strResultForHead = "PASS";
                foreach (ManualConnection manualConnection in testPlan.ManualConnectionList)
                {
                    foreach (TestStep testStep in manualConnection.TestStepList)
                    {
                        PIMTestStep PTestStep = (PIMTestStep)testStep;

                        if (PTestStep.PassFail == false || PTestStep.PassFail == null)
                        {
                            strResultForHead = "FAIL";
                            break;
                        }
                    }

                }

                result.ResultForHead = strResultForHead;
                result.DeviceModel = strDeviceModel;

                lstResult.Add(result);
            }
            catch (Exception ex)
            {
                //System.Windows.m
            }
            return lstResult;
        }

        public static void saveResult(PIMResult Res)
        {
            //保存到数据库
            ATE2MES.ATETESTDATA testData = new ATE2MES.ATETESTDATA();

            ATE2MES.ATEParamPack SN = new ATE2MES.ATEParamPack("SN", Res.SN);
            testData.HeadData.Add(SN);
            ATE2MES.ATEParamPack ATEKind = new ATE2MES.ATEParamPack("ATEKind", Res.ATEKind);
            testData.HeadData.Add(ATEKind);
            ATE2MES.ATEParamPack MATERIAL = new ATE2MES.ATEParamPack("MATERIAL", Res.Material);
            testData.HeadData.Add(MATERIAL);
            ATE2MES.ATEParamPack ProcessId = new ATE2MES.ATEParamPack("ProcessId", Res.ProcessId);
            testData.HeadData.Add(ProcessId);
            ATE2MES.ATEParamPack ResultHead = new ATE2MES.ATEParamPack("Result", Res.ResultForHead);
            testData.HeadData.Add(ResultHead);
            ATE2MES.ATEParamPack DeviceModel = new ATE2MES.ATEParamPack("DeviceModel", Res.DeviceModel);
            testData.HeadData.Add(DeviceModel);
            ATE2MES.ATEParamPack TestLoginUserID = new ATE2MES.ATEParamPack("TestLoginUserID", Res.TestUserID);
            testData.HeadData.Add(TestLoginUserID);
            ATE2MES.ATEParamPack ReTest = new ATE2MES.ATEParamPack("ReTest", Res.Remark);
            testData.HeadData.Add(ReTest);
            ATE2MES.ATEParamPack SaveDateTime = new ATE2MES.ATEParamPack("SaveDateTime", Res.SaveDateTime);
            testData.HeadData.Add(SaveDateTime);

            for (int i = 0; i < Res.lstStepData.Count; i++)
            {
                StepData SD = Res.lstStepData[i];

                ATE2MES.ATETESTSTEPDATA stepData = new ATE2MES.ATETESTSTEPDATA();
                stepData.STEPID = SD.STEPID;

                ATE2MES.ATEParamPack ProName = new ATE2MES.ATEParamPack("ProName", SD.ProName);
                stepData.ATEDATAS.Add(ProName);
                ATE2MES.ATEParamPack Port = new ATE2MES.ATEParamPack("Port", SD.PortName);
                stepData.ATEDATAS.Add(Port);
                ATE2MES.ATEParamPack Band = new ATE2MES.ATEParamPack("Band", SD.Band);
                stepData.ATEDATAS.Add(Band);
                ATE2MES.ATEParamPack RBandMin = new ATE2MES.ATEParamPack("RBandMin", SD.RBandMin);
                stepData.ATEDATAS.Add(RBandMin);
                ATE2MES.ATEParamPack RBandMax = new ATE2MES.ATEParamPack("RBandMax", SD.RBandMax);
                stepData.ATEDATAS.Add(RBandMax);
                ATE2MES.ATEParamPack Orientation = new ATE2MES.ATEParamPack("Orientation", (SD.MeasMode == "REFL" ? "反射" : "传输"));
                stepData.ATEDATAS.Add(Orientation);
                ATE2MES.ATEParamPack Model = new ATE2MES.ATEParamPack("Model", (SD.CalType == "Sweep" ? "扫频":"点频" ));
                stepData.ATEDATAS.Add(Model);
                ATE2MES.ATEParamPack Order = new ATE2MES.ATEParamPack("Order", SD.Order);
                stepData.ATEDATAS.Add(Order);
                ATE2MES.ATEParamPack Freq1 = new ATE2MES.ATEParamPack("Freq1", SD.Freq1);
                stepData.ATEDATAS.Add(Freq1);
                ATE2MES.ATEParamPack Freq2 = new ATE2MES.ATEParamPack("Freq2", SD.Freq2);
                stepData.ATEDATAS.Add(Freq2);
                ATE2MES.ATEParamPack Pow1 = new ATE2MES.ATEParamPack("Pow1", SD.Pow1);
                stepData.ATEDATAS.Add(Pow1);
                ATE2MES.ATEParamPack Pow2 = new ATE2MES.ATEParamPack("Pow2", SD.Pow2);
                stepData.ATEDATAS.Add(Pow2);
                ATE2MES.ATEParamPack Pow1Offset = new ATE2MES.ATEParamPack("Pow1Offset", "0");
                stepData.ATEDATAS.Add(Pow1Offset);
                ATE2MES.ATEParamPack Pow2Offset = new ATE2MES.ATEParamPack("Pow2Offset", "0");
                stepData.ATEDATAS.Add(Pow2Offset);
                ATE2MES.ATEParamPack Limit = new ATE2MES.ATEParamPack("Limit", SD.Limit);
                stepData.ATEDATAS.Add(Limit);
                ATE2MES.ATEParamPack IMPow1 = new ATE2MES.ATEParamPack("IMPow1", SD.IMPow1);
                stepData.ATEDATAS.Add(IMPow1);
                ATE2MES.ATEParamPack IMPow2 = new ATE2MES.ATEParamPack("IMPow2", SD.IMPow2);
                stepData.ATEDATAS.Add(IMPow2);
                ATE2MES.ATEParamPack resultFim1 = new ATE2MES.ATEParamPack("resultFim1", SD.ResultFim1);
                stepData.ATEDATAS.Add(resultFim1);
                ATE2MES.ATEParamPack resultFim2 = new ATE2MES.ATEParamPack("resultFim2", SD.ResultFim2);
                stepData.ATEDATAS.Add(resultFim2);
                ATE2MES.ATEParamPack resultValue1 = new ATE2MES.ATEParamPack("resultValue1", SD.ResultValue1);
                stepData.ATEDATAS.Add(resultValue1);
                ATE2MES.ATEParamPack resultValue2 = new ATE2MES.ATEParamPack("resultValue2", SD.ResultValue2);
                stepData.ATEDATAS.Add(resultValue2);
                ATE2MES.ATEParamPack ResUnit = new ATE2MES.ATEParamPack("ResUnit", SD.ResUnit);
                stepData.ATEDATAS.Add(ResUnit);
                ATE2MES.ATEParamPack result = new ATE2MES.ATEParamPack("result", SD.Result);
                stepData.ATEDATAS.Add(result);
                ATE2MES.ATEParamPack TestDateTime = new ATE2MES.ATEParamPack("TestDateTime", SD.TestDateTime);
                stepData.ATEDATAS.Add(TestDateTime);

                if (SD.isImage)
                {
                    ATE2MES.ATEImagePack Image = new ATE2MES.ATEImagePack("Image", SD.Image);
                    stepData.ATEIMGS.Add(Image);
                }

                testData.ATESTEPDATA.Add(stepData);
            }
            ATE2MES.ATE2MES.UploadATEData(testData);

        }
    }

    public class PIMResult
    {
        public string SN { get; set; }
        public string ATEKind { get; set; }
        public string Material { get; set; }
        public string ProcessId { get; set; }
        public string ResultForHead { get; set; }
        public string DeviceModel { get; set; }
        public string TestUserID { get; set; }
        public string Remark { get; set; }
        public string SaveDateTime { get; set; }

        public List<StepData> lstStepData { get; set; }

        public PIMResult()
        {
            lstStepData = new List<StepData>();
        }
    }

    public class StepData
    {
        public string STEPID { get; set; }
        public string ProName { get; set; }
        public string PortName { get; set; }
        public string Band { get; set; }
        public string RBandMin { get; set; }
        public string RBandMax { get; set; }
        public string CalType { get; set; }
        public string MeasMode { get; set; }
        public string Order { get; set; }
        public string Freq1 { get; set; }
        public string Freq2 { get; set; }
        public string Pow1 { get; set; }
        public string Pow2 { get; set; }
        public string Limit { get; set; }
        public string IMPow1 { get; set; }
        public string IMPow2 { get; set; }
        public string ResultFim1 { get; set; }
        public string ResultFim2 { get; set; }
        public string ResultValue1 { get; set; }
        public string ResultValue2 { get; set; }
        public string ResUnit { get; set; }
        public string Result { get; set; }
        public string TestDateTime { get; set; }
        public bool isImage { get; set; }
        public byte[] Image { get; set; }
    }
}
