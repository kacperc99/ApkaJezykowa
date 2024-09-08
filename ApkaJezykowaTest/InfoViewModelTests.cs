using NUnit.Framework;
using System;
using ApkaJezykowa.MVVM.ViewModel;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.View;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Principal;
using System.Net;
using System.Data.SqlClient;
using ApkaJezykowa.Keys;
using System.Diagnostics;

namespace ApkaJezykowaTest
{
  public class InfoViewModelTests
  {
    [SetUp]
    public void Setup()
    {
      MeasurementModel.Instance.cpu = new("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
      MeasurementModel.Instance.ram = new("Process", "Working Set", Process.GetCurrentProcess().ProcessName);
      MeasurementModel.Instance.Measurement_Results = new List<Tuple<string, List<double>, List<float>, TimeSpan, double, float>>();
      MeasurementModel.Instance.CPU_Vals = new List<double>();
      MeasurementModel.Instance.RAM_Vals = new List<float>();
      SpeechServiceKey.Instance.UserBaseConnection = @"Server=tcp:linguonator2.database.windows.net,1433;Initial Catalog=UserBase;Persist Security Info=False;User ID=r4fxdvt7fu;Password=jkhGKjuFKku653212#123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets=true";
      SpeechServiceKey.Instance.CourseBaseConnection = @"Server=tcp:linguonator2.database.windows.net,1433;Initial Catalog=CourseBase;Persist Security Info=False;User ID=r4fxdvt7fu;Password=jkhGKjuFKku653212#123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets=true";
    }

    [Test]
    public void UsernameVerification()
    {
      string Username = "admin";
      Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
      var InfoViewModel = new InfoViewModel();
      InfoViewModel.LoadCurrentUserData();
      Assert.AreEqual(InfoViewModel.CurrentUserAccount.Username, "Nazwa użytkownika: admin", "musi pyknąć");
    }
  }
}
