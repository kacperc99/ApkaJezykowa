using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ApkaJezykowa.Converters;
using ApkaJezykowa.Keys;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.View;
using ApkaJezykowa.Repositories;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ApkaJezykowa
{
  /// <summary>
  /// Logika interakcji dla klasy App.xaml
  /// </summary>
  
  public partial class App : Application
  {
    /*public IServiceProvider ServiceProvider { get; private set; }
    public IConfiguration Configuration { get; private set; }*/
    [DllImport("Kernel32")]
    public static extern void AllocConsole();

    [DllImport("Kernel32", SetLastError = true)]
    public static extern void FreeConsole();
    protected override void OnStartup(StartupEventArgs e)
    {
      var client = new NamedPipeClientStream("DataPasser");
      client.Connect();
      using (var reader = new StreamReader(client))
      {
      SpeechServiceKey.Instance.Key = reader.ReadLine();
      SpeechServiceKey.Instance.Region = reader.ReadLine();
        SpeechServiceKey.Instance.UserBaseConnection = @"Server=192.168.50.116,49170; Initial Catalog=UserBase; User ID=app; Password=app; MultipleActiveResultSets=true";
        SpeechServiceKey.Instance.CourseBaseConnection = @"Server=192.168.50.116,49170; Initial Catalog=CourseBase; User ID=app; Password=app; MultipleActiveResultSets=true";
      }
      AllocConsole();
      MeasurementModel.Instance.cpu = new("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
      MeasurementModel.Instance.ram = new("Process", "Working Set", Process.GetCurrentProcess().ProcessName);
      MeasurementModel.Instance.Measurement_Results = new List<Tuple<string, List<double>, List<float>, TimeSpan, double, float>>();
      MeasurementModel.Instance.CPU_Vals = new List<double>();
      MeasurementModel.Instance.RAM_Vals = new List<float>();
      //MeasurementModel.Instance.measurement = new Thread(new ThreadStart(MeasurementModel.Instance.CPU_Measurement));
      //MeasurementModel.Instance.measurement.Start();
      //MeasurementModel.Instance.stopwatch = new Stopwatch();
      /*var applicationBuilder = Host.CreateApplicationBuilder();

       applicationBuilder.Logging.AddConsole();

      applicationBuilder.Configuration.AddJsonFile("AppSettings.json");*/
    }
  }
}
