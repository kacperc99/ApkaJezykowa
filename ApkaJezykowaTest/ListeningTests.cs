using ApkaJezykowa.Keys;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowaTest
{
  internal class ListeningTests
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
    public void TTSVerification()
    {
      var tTSPhraseViewModel = new TTSPhraseViewModel(1, "French");
      Assert.IsNotEmpty(tTSPhraseViewModel.phrases, "lista powinna zawierać do ośmiu elementów");
    }
    [Test]
    public void TTSTestVerification()
    {
      var tTSPhraseViewModel = new TTSPhraseViewModel(1, "French", true);
      Assert.IsNotEmpty(tTSPhraseViewModel.phrases, "lista powinna zawierać do ośmiu elementów");
    }
    [Test]
    public void ChooseRightPhraseVerification()
    {
      var chooseRightPhraseViewModel = new ChooseRightPhraseViewModel(1, "French", 8);
      Assert.IsNotEmpty(chooseRightPhraseViewModel.data, "lista powinna zawierać do dwóch elementów");
    }
    [Test]
    public void TestChooseRightPhraseVerification()
    {
      var chooseRightPhraseViewModel = new ChooseRightPhraseViewModel(1, "French", 8, true);
      Assert.IsNotEmpty(chooseRightPhraseViewModel.data, "lista powinna zawierać do dwóch elementów");
    }
  }
}
