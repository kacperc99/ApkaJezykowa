using ApkaJezykowa.MVVM.Model;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApkaJezykowa.Repositories
{
  public class PerformanceMeasurementRepository : IPerformanceMeasurementRepository
  {
    public void CPU_Measurement()
    {
      while (Properties.Settings.Default.ThreadManager)
      {
        Console.WriteLine("CPU: " + MeasurementModel.Instance.cpu.NextValue().ToString() + "%" + " RAM: " + MeasurementModel.Instance.ram.NextValue().ToString() + "MB");
        //Thread.Sleep(100);
      }
    }
  }
}
