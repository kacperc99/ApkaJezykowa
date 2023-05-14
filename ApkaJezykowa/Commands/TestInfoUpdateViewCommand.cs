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
  internal class TestInfoUpdateViewCommand : ICommand
  {
    private TestInfoViewModel viewModel;
    private IExerciseRepository exerciseRepository;

    public TestInfoUpdateViewCommand(TestInfoViewModel viewModel)
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
      Console.WriteLine("Clicked!");
      if (parameter.ToString() == "Continue")
      {
        TestModel.instance.TestMode = true;
        exerciseRepository.Enter_Test_Mode(ExerciseLevelModel.Instance.id, ExerciseLevelModel.Instance.Language);
        TestModel.instance.Test_Points = 0;
        viewModel.SelectedViewModel = new FrenchExerciseViewModel();
      }
      if (parameter.ToString() == "ReturnToMenu")
      {
        viewModel.SelectedViewModel = new FrenchExerciseMenuViewModel();
      }
    }
  }
}
