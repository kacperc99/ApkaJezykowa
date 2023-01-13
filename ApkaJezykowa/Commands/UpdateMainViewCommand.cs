using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  internal class UpdateMainViewCommand : ICommand
  {
    private MainViewModel viewModel;

    public UpdateMainViewCommand(MainViewModel viewModel)
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
      if (parameter.ToString() == "Home")
      {
        viewModel.MainView = new MainViewModel();
      }
      if (parameter.ToString() == "Logout")
      {
        Thread.CurrentPrincipal = null;
        viewModel.MainView = new LoginViewModel();
      }
    }
  }
}
