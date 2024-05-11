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
    public List<double> CPU_Vals { get; set; }
    public List<float> RAM_Vals { get; set; }
    //function measured, CPU time measurements, RAM measurements, elapsed time, av. CPU time, av. RAM 
    public List<Tuple<string, List<double>, List<float>, TimeSpan, double, float>> Measurement_Results { get; set; }
    public void CPU_Measurement()
    {
      while (Properties.Settings.Default.ThreadManager)
      {
        MeasurementModel.Instance.CPU_Vals.Add(MeasurementModel.Instance.cpu.NextValue());
        MeasurementModel.Instance.RAM_Vals.Add(MeasurementModel.Instance.ram.NextValue());
      }
    }
    private MeasurementModel() { }
    public static readonly MeasurementModel Instance = new MeasurementModel();
  }
}
