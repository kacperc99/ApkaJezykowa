using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using ApkaJezykowa.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  internal class Pars
  {
    public string par { get; set; }
    public int id { get; set; }
  }

  internal class FrenchExerciseMenuUpdateViewCommand : BaseViewModel, ICommand 
  {
    private FrenchExerciseMenuViewModel viewModel;
    public List<Pars> pars = new List<Pars>();
    private IExerciseRepository exerciseRepository;
    public FrenchExerciseMenuUpdateViewCommand(FrenchExerciseMenuViewModel viewModel)
    {
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
      exerciseRepository.Obtain_Pars(pars, ExerciseLevelModel.Instance.Language);
      Console.WriteLine("Clicked!");
      foreach(var s in pars)
      {
        if (parameter.ToString() == s.par)
        {
          ExerciseLevelModel.Instance.id = s.id;
          viewModel.SelectedViewModel = new FrenchExerciseViewModel();
        }
      }
      /*
      if (parameter.ToString() == "ArticlesExercise")
      {
        ExerciseLevelModel.Instance.Level = 1;
        viewModel.SelectedViewModel = new FrenchExerciseViewModel();
      }
      if (parameter.ToString() == "BasicSentencesExercise")
      {
        ExerciseLevelModel.Instance.Level = 2;
        viewModel.SelectedViewModel = new FrenchExerciseViewModel();
      }
      if (parameter.ToString() == "ReturnToMenu")
      {
        viewModel.SelectedViewModel = new FrenchViewModel();
      }
      */
    }
  }
}
