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
    public List<string> _tiptmp = new List<string>();
    public List<string> _tip = new List<string>(new string[10]);
    public string _tmp1;
    public string _tmp2;
    public string _tmp3;
    public string _tmp4;
    public string _tmp5;
    public string _tmp6;
    public string _tmp7;
    public string _tmp8;
    public string _tmp9;
    public string _tmp10;
    public string _result;

    private IExerciseRepository exerciseRepository;

    public string Test { get { return _test; } set { _test = value; OnPropertyChanged(nameof(Test)); } }
    public List<string> Exercise { get { return _exercise; } }
    public List<string> Answer { get { return _answer; } }
    public List<string> Tiptmp { get { return _tiptmp; } }
    public List<string> Tip { get { return _tip; } set { _tip = value; OnPropertyChanged(nameof(Tip)); }}
    public string Tmp1 { get { return _tmp1; } set { _tmp1 = value; OnPropertyChanged(nameof(Tmp1)); } }
    public string Tmp2 { get { return _tmp2; } set { _tmp2 = value; OnPropertyChanged(nameof(Tmp2)); }}
    public string Tmp3 { get { return _tmp3; } set { _tmp3 = value; OnPropertyChanged(nameof(Tmp3)); } }
    public string Tmp4 { get { return _tmp4; } set { _tmp4 = value; OnPropertyChanged(nameof(Tmp4)); } }
    public string Tmp5 { get { return _tmp5; } set { _tmp5 = value; OnPropertyChanged(nameof(Tmp5)); } }
    public string Tmp6 { get { return _tmp6; } set { _tmp6 = value; OnPropertyChanged(nameof(Tmp6)); } }
    public string Tmp7 { get { return _tmp7; } set { _tmp7 = value; OnPropertyChanged(nameof(Tmp7)); } }
    public string Tmp8 { get { return _tmp8; } set { _tmp8 = value; OnPropertyChanged(nameof(Tmp8)); } }

    public string Tmp9 { get { return _tmp9; } set { _tmp9 = value; OnPropertyChanged(nameof(Tmp9)); } }
    public string Tmp10 { get { return _tmp10; } set { _tmp10 = value; OnPropertyChanged(nameof(Tmp10)); } }
    public string Result { get { return _result; } set { _result = value; OnPropertyChanged(nameof(Result)); } }

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
      int points = 0;
      for(int i = 0; i < 10; i++)
      {
        if(Answer[i]==ExerciseModel.Instance.Answer[i])
        {
          Tip[i]="Prawidłowa odpowiedź!";
          Console.WriteLine(Answer[i]);
          Console.WriteLine(Tip[i]);
          points++;
        }
        else
        {
          Tip[i]=(ExerciseModel.Instance.Tip[i]);
          Console.WriteLine(Answer[i]);
          Console.WriteLine(Tip[i]);
        }
      }
      Tmp1 = Tip[0];
      Tmp2 = Tip[1];
      Tmp3 = Tip[2];
      Tmp4 = Tip[3];
      Tmp5 = Tip[4];
      Tmp6 = Tip[5];
      Tmp7 = Tip[6];
      Tmp8 = Tip[7];
      Tmp9 = Tip[9];
      Tmp10 = Tip[9];

      if(points < 8)
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
      exerciseRepository.Display(ExerciseLevelModel.Instance.Level, ExerciseLevelModel.Instance.Language);
      Test = ExerciseModel.Instance.TaskText;
      Exercise.AddRange(ExerciseModel.Instance.Exercise);
    }
  }
}
