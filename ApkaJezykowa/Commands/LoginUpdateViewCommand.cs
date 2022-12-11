using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  internal class LoginUpdateViewCommand : /*LoginViewModel,*/ ICommand
  {
    private LoginViewModel viewModel;

    public LoginUpdateViewCommand(LoginViewModel viewModel)
    {
      this.viewModel = viewModel;
    }


    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      /*if(Check)
        return false;
      else*/
        return true;
    }
    public void Execute(object parameter)
    {
      Console.WriteLine("Clicked!");
      if (parameter.ToString() == "German")
      {
        //viewModel.SelectedViewModel = new GermanViewModel();
      }
      if (parameter.ToString() == "French")
      {
       // viewModel.SelectedViewModel = new FrenchViewModel();
      }
      if (parameter.ToString() == "English")
      {
        //viewModel.SelectedViewModel = new EnglishViewModel();
      }
    }
  }
}

