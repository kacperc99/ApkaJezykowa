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
    private ExerciseLevelModel _level;
    string _test;

    public ExerciseLevelModel Level { get { return _level; } set { _level = value; OnPropertyChanged(nameof(Level)); } }
    public string Test { get { return _test; } set { _test = value; OnPropertyChanged(nameof(Test)); } }
    public FrenchExerciseViewModel(int Level)
    {
      LoadCurrentCourseLevel(Level);
    }

    private void LoadCurrentCourseLevel(int Level)
    {
      Console.WriteLine(Level.ToString() );
      Console.WriteLine("Trzeci Paramentr");
      Test =Level
        .ToString();
    }
  }
}
