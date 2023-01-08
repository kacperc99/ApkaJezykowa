using ApkaJezykowa.Main;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.MVVM.ViewModel
{
  internal class FrenchExerciseViewModel : BaseViewModel
  {
    string _test;
    public List<string> _exercise = new List<string>();
    public List<string> _answer = new List<string>(new string[10]);
    public List<string> _tip = new List<string>(new string[10]);
    private IExerciseRepository exerciseRepository;

    public string Test { get { return _test; } set { _test = value; OnPropertyChanged(nameof(Test)); } }
    public List<string> Exercise { get { return _exercise; } }
    public List<string> Answer { get { return _answer; } }
    public List<string> Tip { get { return _tip; } }
    public ICommand CheckAnswers { get; }
    public FrenchExerciseViewModel()
    {
      exerciseRepository = new ExerciseRepository();
      CheckAnswers = new RelayCommand(ExecuteCheckAnswers, CanExecuteCheckAnswers);
      LoadCurrentCourseLevel();
    }

    private bool CanExecuteCheckAnswers(object arg)
    {
      return true;
    }

    private void ExecuteCheckAnswers(object obj)
    {
      for(int i = 0; i < 10; i++)
      {
        if(Answer[i]==ExerciseModel.Instance.Answer[i])
        {
          Tip[i]="Prawidłowa odpowiedź!";
          Console.WriteLine(Answer[i]);
          Console.WriteLine(Tip[i]);
        }
        else
        {
          Tip[i]=ExerciseModel.Instance.Tip[i];
          Console.WriteLine(Answer[i]);
          Console.WriteLine(Tip[i]);
        }
      }
    }

    private void LoadCurrentCourseLevel()
    {
      exerciseRepository.Display(ExerciseLevelModel.Instance.Level, ExerciseLevelModel.Instance.Language);
      Test = ExerciseModel.Instance.TaskText;
      Exercise.AddRange(ExerciseModel.Instance.Exercise);
    }
  }
}
