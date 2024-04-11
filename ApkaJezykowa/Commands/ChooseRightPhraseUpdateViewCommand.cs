using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  internal class ChooseRightPhraseUpdateViewCommand : ICommand
  {
    public string Lang;
    private ChooseRightPhraseViewModel viewModel;
    public ChooseRightPhraseUpdateViewCommand(ChooseRightPhraseViewModel viewModel, string lang)
    {
      this.viewModel = viewModel;
      Lang = lang;
    }
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      throw new NotImplementedException();
    }

    public void Execute(object parameter)
    {
      if (parameter.ToString() == "ReturnToMenu")
      {
        viewModel.SelectedViewModel = new VocabularyMenuViewModel(Lang);
      }
    }
  }
}
