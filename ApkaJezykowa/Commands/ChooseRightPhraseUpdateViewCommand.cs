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
    int Id_Vocabulary;
    int points;
    private ChooseRightPhraseViewModel viewModel;
    public ChooseRightPhraseUpdateViewCommand(ChooseRightPhraseViewModel viewModel, string lang)
    {
      this.viewModel = viewModel;
      Lang = lang;
    }
    public ChooseRightPhraseUpdateViewCommand(ChooseRightPhraseViewModel viewModel, string lang, int id_Vocabulary, int points)
    {
      this.viewModel = viewModel;
      Lang = lang;
      Id_Vocabulary = id_Vocabulary;
      this.points = points;
    }
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      if (parameter.ToString() == "ReturnToMenu")
      {
        viewModel.SelectedViewModel = new VocabularyMenuViewModel(Lang);
      }
      if (parameter.ToString() == "Test")
      {
        viewModel.SelectedViewModel = new ComprehensionViewModel(Id_Vocabulary, Lang, true, points);
      }
    }
  }
}
