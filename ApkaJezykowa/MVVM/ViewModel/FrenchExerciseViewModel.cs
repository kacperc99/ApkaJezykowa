using ApkaJezykowa.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.ViewModel
{
  internal class FrenchExerciseViewModel : BaseViewModel
  {
    string _test;

    public string Test { get { return _test; } set { _test = value; OnPropertyChanged(nameof(Test)); } }
    public FrenchExerciseViewModel()
    {
      LoadCurrentCourseLevel();
    }

    private void LoadCurrentCourseLevel()
    {
      Console.WriteLine(ExerciseLevelModel.Instance.Level.ToString());
      Console.WriteLine("Trzeci Paramentr");
      Test = ExerciseLevelModel.Instance.Level
        .ToString();
    }
  }
}
