using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using ApkaJezykowa.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  internal class Pars
  {
    public string par { get; set; }
    public int id { get; set; }
    public string text { get; set; }
  }

  internal class ExerciseMenuUpdateViewCommand : BaseViewModel, ICommand 
  {
    public string Lang;
    private ExerciseMenuViewModel viewModel;
    public List<Pars> pars = new List<Pars>();
    private IExerciseRepository exerciseRepository;
    public ExerciseMenuUpdateViewCommand(ExerciseMenuViewModel viewModel, string Lang)
    {
      this.Lang=Lang;
      this.viewModel = viewModel;
      exerciseRepository = new ExerciseRepository();
    }
   

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }
    public void Execute(object parameter)
    {
      exerciseRepository.Obtain_Pars(pars, Lang);
      Console.WriteLine("Clicked!");
      foreach(var s in pars)
      {
        if (parameter.ToString() == s.par)
        {
          if (s.par.Contains("Test"))
          {
            int id = s.id;
            viewModel.SelectedViewModel = new TestInfoViewModel(Lang, id);
          }
          else
          {
            int id = s.id;
            string Task_text = s.text;
            viewModel.SelectedViewModel = new ExerciseViewModel(Lang, id, Task_text);
          }
        }
      }
      /*
      if (parameter.ToString() == "ArticlesExercise")
      {
        ExerciseLevelModel.Instance.Level = 1;
        viewModel.SelectedViewModel = new ExerciseViewModel();
      }
      if (parameter.ToString() == "BasicSentencesExercise")
      {
        ExerciseLevelModel.Instance.Level = 2;
        viewModel.SelectedViewModel = new ExerciseViewModel();
      }
      if (parameter.ToString() == "ReturnToMenu")
      {
        viewModel.SelectedViewModel = new ModuleMenuViewModel();
      }
      */
    }
  }
}
