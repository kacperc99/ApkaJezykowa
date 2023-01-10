using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  internal class FrenchExerciseMenuUpdateViewCommand : BaseViewModel, ICommand 
  {
    private FrenchExerciseMenuViewModel viewModel;

    public FrenchExerciseMenuUpdateViewCommand(FrenchExerciseMenuViewModel viewModel)
    {
      this.viewModel = viewModel;
    }
   

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }
    public void Execute(object parameter)
    {
      Console.WriteLine("Clicked!");
      if (parameter.ToString() == "ArticlesExercise")
      {
        ExerciseLevelModel.Instance.Level = 1;
        ExerciseLevelModel.Instance.Language = "Francuski";
        viewModel.SelectedViewModel = new FrenchExerciseViewModel();
        Console.WriteLine(ExerciseLevelModel.Instance.Level.ToString());
        Console.WriteLine("Pierwszy Parametr");
      }
      if (parameter.ToString() == "BasicSentencesExercise")
      {
        ExerciseLevelModel.Instance.Level = 2;
        ExerciseLevelModel.Instance.Language = "Francuski";
        viewModel.SelectedViewModel = new FrenchExerciseViewModel();
        Console.WriteLine(ExerciseLevelModel.Instance.Level.ToString());
        Console.WriteLine("Drugi Parametr");
      }
      if (parameter.ToString() == "ReturnToMenu")
      {
        viewModel.SelectedViewModel = new FrenchViewModel();
      }
    }
  }
}
