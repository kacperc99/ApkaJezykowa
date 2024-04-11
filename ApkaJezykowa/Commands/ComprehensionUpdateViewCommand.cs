using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.Commands
{
  public class ComprehensionUpdateViewCommand : ICommand
  {
    string Lang;
    string Title;
    string TTS_Text;
    public ObservableCollection<TextQuestionTestModel> textQuestions = new ObservableCollection<TextQuestionTestModel>();
    public ObservableCollection<string> correctAnswers = new ObservableCollection<string>();
    public string Translated_Text;
    private ComprehensionViewModel viewModel;
    public ComprehensionUpdateViewCommand(ComprehensionViewModel viewModel, ObservableCollection<TextQuestionTestModel> textQuestions, ObservableCollection<string> correctAnswers, string Translated_Text, string TTS_Text, string Title, string lang)
    {
      this.textQuestions = textQuestions;
      this.correctAnswers = correctAnswers;
      this.Translated_Text = Translated_Text;
      this.viewModel = viewModel;
      this.Lang = lang;
      this.TTS_Text = TTS_Text;
      this.Title = Title;
    }
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
      throw new NotImplementedException();
    }

    public void Execute(object parameter)
    {
      if (parameter.ToString() == "GoToQuestions")
      {
        viewModel.SelectedViewModel = new ComprehensionTestViewModel(textQuestions, correctAnswers, Translated_Text, TTS_Text, Title);
      }
      if (parameter.ToString() == "ReturnToMenu")
      {
        viewModel.SelectedViewModel = new VocabularyMenuViewModel(Lang);
      }
    }
  }
}
