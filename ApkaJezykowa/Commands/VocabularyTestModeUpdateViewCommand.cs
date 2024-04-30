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
    public int id;
    public string Lang;
    public VocabularyTestModeUpdateViewCommand(VocabularyTestModeViewModel viewModel, int id, string Lang)
    {
      this.viewModel = viewModel;
      this.id = id;
      this.Lang = Lang;
    }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      if(parameter.ToString()=="Continue")
        viewModel.SelectedViewModel = new TTSPhraseViewModel(id, Lang, true);
      if(parameter.ToString()=="ReturnToMenu")
        viewModel.SelectedViewModel = new VocabularyMenuViewModel(Lang);
    }
  }
}
