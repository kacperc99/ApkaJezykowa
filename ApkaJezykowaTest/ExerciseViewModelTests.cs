using ApkaJezykowa.MVVM.Model;
using NUnit.Framework;
using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApkaJezykowa.Keys;
using System.Diagnostics;

namespace ApkaJezykowaTest
{
  internal class ExerciseViewModelTests
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
    public void LoadDataTest()
    {
      var exerciseViewModel = new ExerciseViewModel("French", 1, "Podaj właściwy rodzajnik określony");
      Assert.AreEqual(exerciseViewModel.Exercises.Count(), 10, "Powinno być 10 przykładów z ćwiczenia  'Rodzajniki Określone' z francuskiego w języku polskim");
      //ExerciseLevelModel.Instance.Level = 1;
      //ExerciseLevelModel.Instance.Language = "Francuski";
      //var ExerciseViewModel = new ExerciseViewModel();
      //Assert.IsNotNull(ExerciseViewModel.Test, "nie powinno być nullem");
      //ExerciseViewModel.CheckAnswers.Execute(null);
    }
  }
}
