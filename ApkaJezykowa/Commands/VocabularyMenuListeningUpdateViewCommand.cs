using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  public class VocabularyMenuListeningUpdateViewCommand : ICommand
  {
    public VocabularyMenuViewModel viewModel;
    public string Lang;
    public VocabularyMenuListeningUpdateViewCommand(string Lang, VocabularyMenuViewModel viewModel)
    {
      this.Lang = Lang;
      this.viewModel = viewModel;
    }
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      viewModel.SelectedViewModel = new TTSPhraseViewModel((int)parameter, Lang);
    }
  }
}
