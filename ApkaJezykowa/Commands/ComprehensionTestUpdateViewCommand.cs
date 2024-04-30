using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  public class ComprehensionTestUpdateViewCommand : ICommand
  {
    public ComprehensionTestViewModel viewModel;
    public string Lang;

    public ComprehensionTestUpdateViewCommand(string lang, ComprehensionTestViewModel viewModel)
    {
      this.Lang = lang;
      this.viewModel = viewModel;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      viewModel.SelectedViewModel = new VocabularyMenuViewModel(Lang);
    }
  }
}
