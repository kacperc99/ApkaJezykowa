using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  internal class FrenchLesson3UpdateViewCommand : ICommand
  {
    private FrenchLesson3ViewModel viewModel;


    public FrenchLesson3UpdateViewCommand(FrenchLesson3ViewModel viewModel)
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
      if (parameter.ToString() == "French1")
      {
        viewModel.SelectedViewModel = new FrenchLessonViewModel();
      }
      if (parameter.ToString() == "French2")
      {
        viewModel.SelectedViewModel = new FrenchLesson2ViewModel();
      }
      if (parameter.ToString() == "French3")
      {
        viewModel.SelectedViewModel = new FrenchLesson3ViewModel();
      }
    }
  }
}
