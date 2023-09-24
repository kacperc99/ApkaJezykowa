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
    public string _task;
    public string _tip;
    public string _answer;
    public string Task { get { return _task; } set { _task = value; OnPropertyChanged(nameof(Task)); } }
    public string Answer { get { return _answer; } set { _answer = value; OnPropertyChanged(nameof(Answer)); } }
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
      for (var i = 0; i < Exs.Count; i++ )
      {
        if (Exs[i].Answer != "" && (Exs[i].Answer == Exercises[i].Answer || Exs[i].Answer == Exercises[i].Answer2 || Exs[i].Answer == Exercises[i].Answer3))
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
      if(TestModel.instance.TestMode==true)
      {
        TestModel.instance.Test_Points += points;
        for (var i = 0; i < TestModel.instance.TestId.Count; i++)
        {
          if (TestModel.instance.Test_Done[i] == false)
          {
            TestModel.instance.Test_Done[i] = true;
            if (i == TestModel.instance.TestId.Count - 1)
            {
              if (TestModel.instance.Test_Points < (TestModel.instance.TestId.Count*10*80/100))
              {
                Result = "Ilość Punktów: " + TestModel.instance.Test_Points;
              }
              else
              {
                Result = "Ilość Punktów: " + TestModel.instance.Test_Points + ". Odblokowałeś następne ćwiczenia!";
              }
              TestModel.instance.TestMode = false;
              TestModel.instance.Test_Done.Clear();
              TestModel.instance.TestId.Clear();
            }
            else
            {
              break;
            }
          }
        }
      }
      else if(TestModel.instance.TestMode==false)
      {
        if (points < 8)
        {
          Result = "Ilość Punktów: " + points;
        }
        else
        {
          Result = "Ilość Punktów: " + points + ". Gratulujemy wyniku!";
        }
      }
    }

    private void LoadCurrentCourseLevel()
    {
      if(TestModel.instance.TestMode==true)
      {
        for(var i =0; i < TestModel.instance.TestId.Count; i++)
        {
          if(TestModel.instance.Test_Done[i]==false)
          {
            Test = TestModel.instance.TestTasks[i];
            Exercises.Clear();
            Exs.Clear();
            exerciseRepository.Display(Exercises, TestModel.instance.TestId[i]);
            for (var j = 0; j < Exercises.Count; j++)
            {
              Ex p = new Ex();
              p.Task = Exercises[j].Task;
              p.Answer = "";
              p.Tip = "";
              Exs.Add(p);
            }
            Console.WriteLine(Test);
            foreach (Ex p in Exs) { Console.WriteLine(p.Task); }
            break;
          }
        }
      }
      else
      {
        Test = ExerciseLevelModel.Instance.Task_text;
        exerciseRepository.Display(Exercises, ExerciseLevelModel.Instance.id);
        for (var i = 0; i < Exercises.Count; i++)
        {
          Ex p = new Ex();
          p.Task = Exercises[i].Task;
          p.Answer = "";
          p.Tip = "";
          Exs.Add(p);
        }
        Console.WriteLine(Test);
        foreach (Ex p in Exs) { Console.WriteLine(p.Task, p.Answer, p.Tip); }
      }
    }
  }
}
