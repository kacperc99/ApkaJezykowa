using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  public class TTSPhraseUpdateViewCommand : ICommand
  {
    public TTSPhraseViewModel viewModel;
    int Id_Listening;
    int Id_Vocabulary;
    int points;
    string Lang;
    bool IsTestMode = false;
    public TTSPhraseUpdateViewCommand(int Id_Listening, string Lang, int points, TTSPhraseViewModel viewModel)
    {
      this.Id_Listening = Id_Listening;
      this.points = points;
      this.Lang = Lang;
      this.viewModel = viewModel;
    }
    public TTSPhraseUpdateViewCommand(int Id_Vocabulary, string Lang, int points, TTSPhraseViewModel viewModel, bool IsTestMode)
    {
      this.Id_Vocabulary = Id_Vocabulary;
      this.points = points;
      this.Lang = Lang;
      this.viewModel = viewModel;
      this.IsTestMode = true;
    }
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public void Execute(object parameter)
    {
      if (parameter.ToString() == "Next")
        if(IsTestMode==false)
          viewModel.SelectedViewModel = new ChooseRightPhraseViewModel(Id_Listening,Lang,points);
        else
          viewModel.SelectedViewModel = new ChooseRightPhraseViewModel(Id_Vocabulary, Lang, points, true);
      if (parameter.ToString() == "ReturnToMenu")
        viewModel.SelectedViewModel = new VocabularyMenuViewModel(Lang);
    }
  }
}
