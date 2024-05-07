using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class MeasurementModel
  {
    public PerformanceCounter cpu {  get; set; }
    public PerformanceCounter ram { get; set; }
    //public Thread measurement { get; set; }
    //public Stopwatch stopwatch { get; set; }
    public void CPU_Measurement()
    {
      while (Properties.Settings.Default.ThreadManager)
      {
        Console.WriteLine("CPU: " + MeasurementModel.Instance.cpu.NextValue().ToString() + "%" + " RAM: " + MeasurementModel.Instance.ram.NextValue().ToString() + "MB");
        //Thread.Sleep(100);
      }
    }
    private MeasurementModel() { }
    public static readonly MeasurementModel Instance = new MeasurementModel();
  }
}
