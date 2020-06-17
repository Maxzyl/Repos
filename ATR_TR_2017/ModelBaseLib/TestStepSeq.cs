using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelBaseLib
{
    
    public class TestStepSeq : List<TestStep>
    {
        public TestStepSeq()
        {
            ExceptionCollection = new List<Exception>();
        }
        public event StepFinishHandler ProgressReporter;
        public event SeqFinishHandler SeqFinish;
        public List<Exception> ExceptionCollection { get; set; }
        public bool IsRunWhenException { get; set; }
        private bool IsRuning;
        private bool IsRun;
        public bool IsAsync;
        public void Stop()
        {
            IsRun = false;

        }
        private int stopIndex = 0;
        public void ReStart()
        {
            stopIndex = 0;
        }
        private void OnSeqRun()
        {
            
            for (int i = stopIndex; i < Count; i++)
            {

                if (IsRun)
                {
                    try
                    {
                        this[i].Single();
                        System.Diagnostics.Debug.WriteLine("testing " + i);
                    }
                    catch (Exception exp)
                    {
                        this[i].CleanUp();
                        ExceptionCollection.Add(exp);

                        IsRun = IsRunWhenException;
                    }
                    if (ProgressReporter != null)
                    {
                        ProgressReporter.Invoke(i);
                    }
                }
                else
                {
                    stopIndex = i;
                    System.Diagnostics.Debug.WriteLine(stopIndex);
                    break;

                }
            }
            if (SeqFinish != null)
            {
                SeqFinish.Invoke();
                IsRuning = false;
            }
        }
        public void Run()
        {
            if (IsRuning)
            {
                return;
            }
            else
            {
                IsRun = true;
                IsRuning = true;
                if (IsAsync)
                {
                    //bw.RunWorkerAsync();
                    System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(OnSeqRun));
                    t.IsBackground = true;
                    t.Start();

                }
                else
                {
                    OnSeqRun();
                }
            }
        }
    }
}
