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
    private ExerciseLevelModel _level;

    public ExerciseLevelModel Level { get { return _level; } set { _level = value; OnPropertyChanged(nameof(Level)); } }

    public FrenchExerciseMenuUpdateViewCommand(FrenchExerciseMenuViewModel viewModel)
    {
      this.viewModel = viewModel;
      Level = new ExerciseLevelModel();
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
        Level.Level = 1;
        viewModel.SelectedViewModel = new FrenchExerciseViewModel(Level.Level);
        Console.WriteLine(Level.Level.ToString());
        Console.WriteLine("Pierwszy Parametr");
      }
      if (parameter.ToString() == "BasicSentencesExercise")
      {
        Level.Level = 2;
        viewModel.SelectedViewModel = new FrenchExerciseViewModel(Level.Level);
        Console.WriteLine(Level.Level.ToString());
        Console.WriteLine("Drugi Parametr");
      }
      /*if (parameter.ToString() == "EnglishExercise")
      {
        viewModel.SelectedViewModel = new EnglishExerciseViewModel();
      }*/
    }
  }
}
