using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  internal class VocabularyTestModeUpdateViewCommand : ICommand
  {
    private VocabularyTestModeViewModel viewModel;

    public VocabularyTestModeUpdateViewCommand(VocabularyTestModeViewModel viewModel)
    {
      this.viewModel = viewModel;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      throw new NotImplementedException();
    }

    public void Execute(object parameter)
    {
      throw new NotImplementedException();
    }
  }
}
