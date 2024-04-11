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
    int points;
    string Lang;
    public TTSPhraseUpdateViewCommand(int Id_Listening, string Lang, int points, TTSPhraseViewModel viewModel)
    {
      this.Id_Listening = Id_Listening;
      this.points = points;
      this.Lang = Lang;
      this.viewModel = viewModel;
    }
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      throw new NotImplementedException();
    }

    public void Execute(object parameter)
    {
      if (parameter.ToString() == null)
        viewModel.SelectedViewModel = new ChooseRightPhraseViewModel(Id_Listening,Lang,points);
      if (parameter.ToString() == "ReturnToMenu")
        viewModel.SelectedViewModel = new VocabularyMenuViewModel(Lang);
    }
  }
}
