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
  internal class LoginUpdateViewCommand : ICommand
  {
    private LoginViewModel viewModel;

    public LoginUpdateViewCommand(LoginViewModel viewModel)
    {
      this.viewModel = viewModel;
    }


    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      if(VisibilityModel.Instance.IsViewVisibleLogin==false)
        return true;
      else
        return false;
    }
    public void Execute(object parameter)
    {
      Console.WriteLine("Clicked!");
      if (VisibilityModel.Instance.IsViewVisibleLogin == false && parameter.ToString()=="Login")
      {
        viewModel.SelectedViewModel = new MainViewModel();
      }
      if (parameter.ToString() == "Test")
      {
        viewModel.SelectedViewModel = new MainViewModel();
      }

    }
  }
}

