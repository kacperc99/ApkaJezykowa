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
  internal class FrenchExerciseUpdateViewCommand : ICommand
  {
    private FrenchExerciseViewModel viewModel;

    public FrenchExerciseUpdateViewCommand(FrenchExerciseViewModel viewModel)
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
      if(parameter.ToString() == "NextTask" && TestModel.instance.TestMode==true)
      {
        viewModel.SelectedViewModel = new FrenchExerciseViewModel();
      }
      if (parameter.ToString() == "ReturnToMenu")
      {
        Console.WriteLine("Że to niby działa?");
        if (TestModel.instance.TestMode)
          TestModel.instance.TestMode = false;
        viewModel.SelectedViewModel = new FrenchExerciseMenuViewModel();
      }
    }
  }
}
