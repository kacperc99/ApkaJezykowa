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
        MeasurementModel.Instance.CPU_Vals.Add(MeasurementModel.Instance.cpu.NextValue());
        MeasurementModel.Instance.RAM_Vals.Add(MeasurementModel.Instance.ram.NextValue());
      }
    }
  }
}
