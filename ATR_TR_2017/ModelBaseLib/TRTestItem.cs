using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CalcEngine;
using Symtant.GeneFunLib;
namespace ModelBaseLib
{
    public class TRTestItem:TestItem
    {
        public TRTestItem()
        {
            TestSpecList = new List<TRTestItemSpec>();
        }  
        [XmlIgnore]
        public double[] FreqList { get; set; }
        [XmlIgnore]
        public AttPhasePair[] StateList { get; set; }

        
        
        /// <summary>
        /// Data[StateIndex,FreqIndex]
        /// </summary>
        [XmlIgnore]
        public double[,] Data { get; set; }
        public void ResetTestResult()
        {
            Data = new double[StateList.Count(), FreqList.Count()];
        }
        public double GetTestValue(int att, int phase, double freq)
        {

            int stateIndex = FindStateIndex(att, phase);

            int freqIndex = Array.IndexOf<double>(FreqList, freq);
            return Data[stateIndex, freqIndex];
        }
        public void SetTestValue(int att, int phase, double freq, double v)
        {

            int stateIndex = FindStateIndex(att, phase);

            int freqIndex = Array.IndexOf<double>(FreqList, freq);
            Data[stateIndex, freqIndex] = v;
        }
        public int FindStateIndex(int att, int phase)
        {
            for (int i = 0; i < StateList.Count(); i++)
            {
                if (StateList[i].Att == att && StateList[i].Phase == phase)
                    return i;
            }
            return -1;
        }
        public void FillSimulatedData()
        {
            Random ran = new Random();
            for (int i = 0; i < StateList.Count(); i++)
            {
                for (int j = 0; j < FreqList.Count(); j++)
                {
                    Data[i, j] = Math.Cos(j * Math.PI * 2 / 1000 + i) + ran.NextDouble();
                    //System.Threading.Thread.Sleep(2);
                }
            }
        }
        public bool IsFixedMarkers { get; set; }
        public void AddDefaultMarker(int specIndex)
        { 
            while(TestSpecList.Count <= specIndex)
            {
                TestSpecList.Add(new TRTestItemSpec());
            }
            TestSpecList[specIndex].TestMarkerList.Add(new XYTestMarker());
        }

        public void CalcInfo()
        {
            if (Data == null || Data.Length == 0)
            {
                foreach (var spec in TestSpecList)
                {
                    spec.PassFail = null;
                }
                return;
            }
            foreach(var spec in TestSpecList)
            {
                #region
                foreach(var marker in spec.TestMarkerList)
                {
                    if(marker is XYTestMarker)
                    {
                        var xyMarker = marker as XYTestMarker;
                        xyMarker.MarkerResult.Clear();
                        switch(xyMarker.Type)
                        {
                            case XYTestMarkerTypeEnum.ABSMax:
                                double yAbsMax = GeneFun.GetTwoArraryMax(Data,true);
                                double xAbsMax = FreqList[0];
                                xyMarker.MarkerResult.Add(new XYData(xAbsMax, yAbsMax));
                                break;
                            case XYTestMarkerTypeEnum.ABSMin:
                                double yAbsMin = GeneFun.GetTwoArrayMin(Data,true);
                                double xAbsMin = FreqList[0];
                                xyMarker.MarkerResult.Add(new XYData(xAbsMin, yAbsMin));
                                break;
                            case XYTestMarkerTypeEnum.Max:
                                double yMax = GeneFun.GetTwoArraryMax(Data,false);
                                double xMax = FreqList[0];
                                xyMarker.MarkerResult.Add(new XYData(xMax, yMax));
                                break;
                            case XYTestMarkerTypeEnum.Min:
                                double yMin = GeneFun.GetTwoArrayMin(Data,false);
                                double xMin = FreqList[0];
                                xyMarker.MarkerResult.Add(new XYData(xMin, yMin));
                                break;
                            case XYTestMarkerTypeEnum.Peak:
                                double y1Max = GeneFun.GetTwoArraryMax(Data,false);
                                double y1Min = GeneFun.GetTwoArrayMin(Data,false);
                                if (Math.Abs(y1Max) >= Math.Abs(y1Min))
                                {
                                    xyMarker.MarkerResult.Add(new XYData(FreqList[0], y1Max));
                                }
                                else
                                {
                                    xyMarker.MarkerResult.Add(new XYData(FreqList[0],y1Min));
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    marker.JudgePassFail();
                }
                #endregion
                #region
                List<bool> boolList = new List<bool>();
                CalcEngine.CalcEngine ce = new CalcEngine.CalcEngine();
                ce.Variables.Add("att",null);
                ce.Variables.Add("phase",null);
                ce.Variables.Add("freq",null);
                for (int i = 0; i < Data.GetLength(0);i++ )
                {
                    for (int j = 0; j < Data.GetLength(1); j++ )
                    {
                        ce.Variables["att"] = StateList[i].Att;
                        ce.Variables["phase"] = StateList[i].Phase;
                        ce.Variables["freq"] = FreqList[j];
                        object highLimitStr;
                        object lowLimitStr;
                        if (string.IsNullOrWhiteSpace(spec.HighLimit))
                        {
                            highLimitStr = null;
                        }
                        else
                        {
                            highLimitStr = ce.Evaluate(spec.HighLimit);
                        }
                        if (string.IsNullOrWhiteSpace(spec.LowLimit))
                        {
                            lowLimitStr = null;
                        }
                        else
                        {
                            lowLimitStr = ce.Evaluate(spec.LowLimit);
                        }
                        double? highLimitValue = highLimitStr.ToNullDouble();
                        double? lowLimitValue = lowLimitStr.ToNullDouble();
                        bool passFail = GeneFun.IsPassFail(Data[i, j], highLimitValue, lowLimitValue);
                        boolList.Add(passFail);
                    }
                }
                spec.PassFail = GeneFun.NullBoolAndList(boolList);
                spec.PassFail = GeneFun.NullBoolAnd(GeneFun.NullBoolAndList(spec.TestMarkerList.Select(x => x.PassFail).ToList()),spec.PassFail);
                #endregion
            }
            PassFail = GeneFun.NullBoolAndList(TestSpecList.Select(x => x.PassFail).ToList());
        }

        public List<TRTestItemSpec> TestSpecList { get; set; }
    }
    public class AttPhasePair
    {
        public int Att { get; set; }
        public int Phase { get; set; }
    }
    public class TRTestItemSpec
    {
        public TRTestItemSpec()
        {
            TestMarkerList = new List<XYTestMarker>();
        }
        public bool? PassFail { get; set; }
        
        public string HighLimit { get; set; }
        public string LowLimit { get; set; }
        public string LimitDescription { get; set; }
        public List<XYTestMarker> TestMarkerList { get; set; }
    }
}
