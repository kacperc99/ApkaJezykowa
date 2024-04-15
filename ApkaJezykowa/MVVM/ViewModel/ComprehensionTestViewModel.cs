using ApkaJezykowa.Main;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.MVVM.ViewModel
{
  public class ComprehensionTestViewModel : BaseViewModel
  {
    string _translated_Text;
    string _tTS_Text;
    string _title;
    string _score;
    int points;
    bool Enabler = true;
    bool IsTestMode = false;
    ObservableCollection<TextQuestionTestModel> textQuestions = new ObservableCollection<TextQuestionTestModel>();
    ObservableCollection<string> correctAnswers = new ObservableCollection<string>();
    string[] answers;
    public BaseViewModel _selectedViewModel;

    public string Translated_Text { get { return _translated_Text; } set { _translated_Text = value; OnPropertyChanged(nameof(Translated_Text)); } }
    public string TTS_Text { get { return _tTS_Text; } set { _tTS_Text = value; OnPropertyChanged(nameof(TTS_Text)); } }
    public string Title { get { return _title; } set { _title = value;OnPropertyChanged(nameof(Title)); } }
    public string Score { get { return _score; } set { _score = value; OnPropertyChanged(nameof(Score)); } }
    public ObservableCollection<TextQuestionTestModel> TextQuestions { get { return textQuestions; } set { textQuestions = value; OnPropertyChanged(nameof(TextQuestions)); } }
    public ObservableCollection<string> CorrectAnswers { get { return correctAnswers; } set { correctAnswers = value; OnPropertyChanged(nameof(CorrectAnswers)); } }  
    public ICommand MarkAnswerCommand { get; set; }
    public ICommand CheckAnswersCommand { get; set; }
    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set { _selectedViewModel = value; OnPropertyChanged(nameof(SelectedViewModel)); }
    }
    public ComprehensionTestViewModel(ObservableCollection<TextQuestionTestModel> textQuestions, ObservableCollection<string> correctAnswers, string _translated_Text, string _tTS_Text, string _title) 
    { 
      this.Translated_Text = _translated_Text;
      this.CorrectAnswers = correctAnswers;
      this.TextQuestions = textQuestions;
      this.TTS_Text = _tTS_Text;
      this.Title = _title;
      this.answers = new string[correctAnswers.Count()];
      this.points = 0;
      MarkAnswerCommand = new RelayCommand(ExecuteMarkAnswerCommand);
      CheckAnswersCommand = new RelayCommand(ExecuteCheckAnswersCommand);
    }
    public ComprehensionTestViewModel(ObservableCollection<TextQuestionTestModel> textQuestions, ObservableCollection<string> correctAnswers, string _translated_Text, string _tTS_Text, string _title, bool IsTestMode, int points)
    {
      this.Translated_Text = _translated_Text;
      this.CorrectAnswers = correctAnswers;
      this.TextQuestions = textQuestions;
      this.TTS_Text = _tTS_Text;
      this.Title = _title;
      this.answers = new string[correctAnswers.Count()];
      this.points = 0;
      MarkAnswerCommand = new RelayCommand(ExecuteMarkAnswerCommand);
      CheckAnswersCommand = new RelayCommand(ExecuteCheckAnswersCommand);
      this.IsTestMode = IsTestMode;
      this.points = points;
    }

    public void ExecuteMarkAnswerCommand(object parameter)
    {
      var values = (object[]) parameter;
      var GroupName = (int)values[0];
      var Answer = (string)values[1];
      answers[GroupName] = Answer;
    }
    public void ExecuteCheckAnswersCommand(object parameter)
    {
      Enabler = false;
      if(!IsTestMode)
      {
        for (int i = 0; i < CorrectAnswers.Count; i++)
        {
          if (answers[i] == CorrectAnswers[i])
            points++;
        }
        if (points > 7)
          Score = "Wynik: " + points.ToString() + ". Gratulujemy wyniku!";
        else
          Score = "Wynik: " + points.ToString();
      }
      else
      {
        for (int i = 0; i < CorrectAnswers.Count; i++)
        {
          if (answers[i] == CorrectAnswers[i])
            points++;
        }
        if (points > 16)
          Score = "Wynik: " + points.ToString() + ". Osiągnąłeś kolejny poziom nauki!";
        else
          Score = "Wynik: " + points.ToString();
      }
    }
  }
}
