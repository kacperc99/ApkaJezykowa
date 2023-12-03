using ApkaJezykowa.Commands;
using ApkaJezykowa.Main;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

  public class ExerciseViewModel : BaseViewModel
  {
    string _test;
    public ObservableCollection<ExerciseModel> exercises = new ObservableCollection<ExerciseModel>();
    public ObservableCollection<Ex> exs = new ObservableCollection<Ex>();
    public string _result;
    private BaseViewModel _selectedViewModel;
    public ObservableCollection<TestData> TestingData = new ObservableCollection<TestData>();
    public int id;
    public string taskText;
    public bool IsTesting = false;
    public int Points = 0;

    private IExerciseRepository exerciseRepository;

    public string Test { get { return _test; } set { _test = value; OnPropertyChanged(nameof(Test)); } }
    public ObservableCollection<ExerciseModel> Exercises { get { return exercises; } set { exercises = value; OnPropertyChanged(nameof(Exercises)); } }
    public ObservableCollection<Ex> Exs { get { return exs; } set { exs = value; OnPropertyChanged(nameof(Exs)); } }
    public string Result { get { return _result; } set { _result = value; OnPropertyChanged(nameof(Result)); } }
    public BaseViewModel SelectedViewModel 
    { 
      get { return _selectedViewModel; }
      set { _selectedViewModel = value; OnPropertyChanged(nameof(SelectedViewModel)); }
    }
    public ICommand CheckAnswers { get; }
    public ICommand FrenchExerciseUpdateViewCommand { get; set; }
    public ExerciseViewModel(string Lang, int id, string taskText)
    {
      this.id = id;
      this.taskText = taskText;
      exerciseRepository = new ExerciseRepository();
      CheckAnswers = new RelayCommand(ExecuteCheckAnswers, CanExecuteCheckAnswers);
      FrenchExerciseUpdateViewCommand = new FrenchExerciseUpdateViewCommand(this, IsTesting, Lang);
      LoadCurrentCourseLevel();
    }
    public ExerciseViewModel(ObservableCollection<TestData> TestingData, string Lang)
    {
      this.TestingData = TestingData;
      IsTesting = true;
      exerciseRepository = new ExerciseRepository();
      CheckAnswers = new RelayCommand(ExecuteCheckAnswers, CanExecuteCheckAnswers);
      FrenchExerciseUpdateViewCommand = new FrenchExerciseUpdateViewCommand(this, Points, IsTesting, TestingData, Lang);
      LoadCurrentCourseLevelTest();
    }
    /*public ExerciseViewModel(List<TestData> TestingData, string Lang, int Points)
    {
      this.Points = Points;
      this.TestingData = TestingData;
      IsTesting = true;
      exerciseRepository = new ExerciseRepository();
      CheckAnswers = new RelayCommand(ExecuteCheckAnswers, CanExecuteCheckAnswers);
      FrenchExerciseUpdateViewCommand = new FrenchExerciseUpdateViewCommand(this, Points, IsTesting, TestingData, Lang);
      LoadCurrentCourseLevelTest();
    }*/
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
      if(IsTesting==true)
      {
        Points += points;
        for (var i = 0; i < TestingData.Count; i++)
        {
          if (TestingData[i].TestDone == false)
          {
            TestingData[i].TestDone = true;
            if (i == TestingData.Count - 1)
            {
              if (Points < (TestingData.Count*10*80/100))
              {
                Result = "Ilość Punktów: " + Points;
              }
              else
              {
                Result = "Ilość Punktów: " + Points + ". Odblokowałeś następne ćwiczenia!";
              }
              //problem z synchronizacją, funkcja odpowiedzialna za zmianę ekranu wykonuje się szybciej
              IsTesting = false;
              //TestModel.instance.Test_Done.Clear();
              //TestModel.instance.TestId.Clear();
            }
            else
            {
              LoadCurrentCourseLevelTest();
              break;
            }
          }
        }
      }
      else if(IsTesting==false)
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
      /*if(TestModel.instance.TestMode==true)
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
      else*/
      {
        Test = taskText;
        exerciseRepository.Display(Exercises, id);
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
    private void LoadCurrentCourseLevelTest()
    {
      for (var i = 0; i < TestingData.Count; i++)
      {
        if (TestingData[i].TestDone == false)
        {
          Test = TestingData[i].TestTasks;
          Exercises.Clear();
          Exs.Clear();
          exerciseRepository.Display(Exercises, TestingData[i].TestId);
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
  }
}
