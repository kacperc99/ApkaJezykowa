using ApkaJezykowa.Commands;
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
  public class Ex : BaseViewModel
  {
    public string _tip;
    public string Task { get; set; }
    public string Answer { get; set; }
    public string Tip { get { return _tip; } set { _tip = value; OnPropertyChanged(nameof(Tip)); } }
  }

  public class FrenchExerciseViewModel : BaseViewModel
  {
    string _test;
    public List<ExerciseModel> exercises = new List<ExerciseModel>();
    public List<Ex> exs = new List<Ex>();
    public string _result;
    private BaseViewModel _selectedViewModel;

    private IExerciseRepository exerciseRepository;

    public string Test { get { return _test; } set { _test = value; OnPropertyChanged(nameof(Test)); } }
    public List<ExerciseModel> Exercises { get { return exercises; } set { exercises = value; OnPropertyChanged(nameof(Exercises)); } }
    public List<Ex> Exs { get { return exs; } set { exs = value; OnPropertyChanged(nameof(Exs)); } }
    public string Result { get { return _result; } set { _result = value; OnPropertyChanged(nameof(Result)); } }
    public BaseViewModel SelectedViewModel 
    { 
      get { return _selectedViewModel; }
      set { _selectedViewModel = value; OnPropertyChanged(nameof(SelectedViewModel)); }
    }
    public ICommand CheckAnswers { get; }
    public ICommand FrenchExerciseUpdateViewCommand { get; set; }
    public FrenchExerciseViewModel()
    {
      exerciseRepository = new ExerciseRepository();
      CheckAnswers = new RelayCommand(ExecuteCheckAnswers, CanExecuteCheckAnswers);
      FrenchExerciseUpdateViewCommand = new FrenchExerciseUpdateViewCommand(this);
      LoadCurrentCourseLevel();
    }

    private bool CanExecuteCheckAnswers(object arg)
    {
      return true;
    }

    private void ExecuteCheckAnswers(object obj)
    {
      int points = 0;
      /*var ExTuple = Exercises.Zip(Exercises, (e, x) => new { Exs = e, Exercises = x });
      foreach (var ex in ExTuple)
      {
        if(ex.Exs.Answer==ex.Exercises.Answer || ex.Exs.Answer == ex.Exercises.Answer2 || ex.Exs.Answer == ex.Exercises.Answer3)
        {
          ex.Exs.Tip="Prawidłowa odpowiedź!";
          Console.WriteLine(ex.Exs.Answer);
          Console.WriteLine(ex.Exs.Tip);
          points++;
          Console.WriteLine(points);
        }
        else
        {
          ex.Exs.Tip = ex.Exercises.Tip;
          Console.WriteLine(ex.Exs.Answer);
          Console.WriteLine(ex.Exs.Tip);
        }
      }*/
      for (var i = 0; i < Exs.Count; i++)
      {
        if(Exs[i].Answer != "" && (Exs[i].Answer == Exercises[i].Answer || Exs[i].Answer == Exercises[i].Answer2 || Exs[i].Answer == Exercises[i].Answer3))
        {
            Exs[i].Tip = "Prawidłowa odpowiedź!";
            Console.WriteLine(Exs[i].Answer);
            Console.WriteLine(Exs[i].Tip);
            points++;
            Console.WriteLine(points);
        }
        else
        {
          Exs[i].Tip = Exercises[i].Tip;
          Console.WriteLine(Exs[i].Answer);
          Console.WriteLine(Exs[i].Tip);
        }
      }
        if (points < 8)
      {
        Result = "Ilość Punktów: " + points;
      }
      else
      {
        Result = "Ilość Punktów: " + points + ". Gratulujemy wyniku!";
      }
    }

    private void LoadCurrentCourseLevel()
    {
      Test = ExerciseLevelModel.Instance.Task_text;
      exerciseRepository.Display(Exercises, ExerciseLevelModel.Instance.id);
      /*var ExTuple = Exercises.Zip(Exercises, (e, x) => new { Exs = e, Exercises = x });
      foreach(var ex in ExTuple)
      {
        ex.Exs.Task = ex.Exercises.Task;
        ex.Exs.Tip = null;
        ex.Exs.Answer = null;
        Console.WriteLine(ex.Exs.Task);
      }*/
      for( var i = 0; i < Exercises.Count; i++ )
      {
        Ex p = new Ex();
        p.Task = Exercises[i].Task;
        p.Answer = "";
        p.Tip = "";
        Exs.Add(p);
      }
      Console.WriteLine(Test);
      foreach (Ex p in Exs) { Console.WriteLine(p.Task); }
    }
  }
}
