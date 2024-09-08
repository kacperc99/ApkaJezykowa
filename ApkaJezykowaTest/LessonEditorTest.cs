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
  internal class LessonEditorTest
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
    public void LessonEditorVerification()
    {
      var lessonEditorViewModel = new LessonEditorViewModel();
      lessonEditorViewModel.Country = "None";
      lessonEditorViewModel.Level = 0;
      lessonEditorViewModel.Language = "None";
      lessonEditorViewModel.LoadData();
      Assert.AreEqual(lessonEditorViewModel.LessonNames.Count() - 1, 38, "Powinno być 38 tytułów");
      lessonEditorViewModel.Country = "French";
      lessonEditorViewModel.LoadData();
      Assert.AreEqual(lessonEditorViewModel.LessonNames.Count() - 1, 14, "Powinno być 14 tytułów");
      lessonEditorViewModel.Language = "Polish";
      lessonEditorViewModel.LoadData();
      Assert.AreEqual(lessonEditorViewModel.LessonNames.Count() - 1, 12, "Powinno być 12 tytułów");
      lessonEditorViewModel.Level = 1;
      lessonEditorViewModel.LoadData();
      Assert.AreEqual(lessonEditorViewModel.LessonNames.Count() - 1, 1, "Powinno być 1 tytułów");
      //stan na 18.06.2024
      //funkcja add/edit jest niemożliwia do przeprowadzenia na niej testów jednostkowych
    }
  }
}
