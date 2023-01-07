using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;
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
    public List<string> _exercise = new List<string>();
    public List<string> _answer = new List<string>();
    public List<string> _tip = new List<string>();
    private IExerciseRepository exerciseRepository;

    public string Test { get { return _test; } set { _test = value; OnPropertyChanged(nameof(Test)); } }
    public List<string> Exercise { get { return _exercise; } }
    public List<string> Answer { get { return _answer; } }
    public List<string> Tip { get { return _tip; } }
    public FrenchExerciseViewModel()
    {
      exerciseRepository = new ExerciseRepository();
      LoadCurrentCourseLevel();
    }

    private void LoadCurrentCourseLevel()
    {
      Console.WriteLine(ExerciseLevelModel.Instance.Level.ToString());
      Console.WriteLine("Trzeci Paramentr");
      Test = ExerciseModel.Instance.TaskText;
      exerciseRepository.Display(ExerciseLevelModel.Instance.Level, ExerciseLevelModel.Instance.Language);
      Exercise.AddRange(ExerciseModel.Instance.Exercise);
    }
  }
}
