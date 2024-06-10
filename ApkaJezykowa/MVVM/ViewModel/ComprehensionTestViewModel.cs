using ApkaJezykowa.Commands;
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
    bool _enabler;
    bool _tip_Enabler;
    bool IsTestMode = false;
    string Lang;
    ObservableCollection<TextQuestionTestModel> textQuestions = new ObservableCollection<TextQuestionTestModel>();
    ObservableCollection<string> correctAnswers = new ObservableCollection<string>();
    string[] answers;
    public BaseViewModel _selectedViewModel;

    public string Translated_Text { get { return _translated_Text; } set { _translated_Text = value; OnPropertyChanged(nameof(Translated_Text)); } }
    public string TTS_Text { get { return _tTS_Text; } set { _tTS_Text = value; OnPropertyChanged(nameof(TTS_Text)); } }
    public string Title { get { return _title; } set { _title = value;OnPropertyChanged(nameof(Title)); } }
    public string Score { get { return _score; } set { _score = value; OnPropertyChanged(nameof(Score)); } }
    public bool Enabler { get { return _enabler; } set { _enabler = value; OnPropertyChanged(nameof(Enabler)); } }
    public bool Tip_Enabler { get { return _tip_Enabler; } set { _tip_Enabler = value; OnPropertyChanged(nameof(Tip_Enabler)); } }
    public ObservableCollection<TextQuestionTestModel> TextQuestions { get { return textQuestions; } set { textQuestions = value; OnPropertyChanged(nameof(TextQuestions)); } }
    public ObservableCollection<string> CorrectAnswers { get { return correctAnswers; } set { correctAnswers = value; OnPropertyChanged(nameof(CorrectAnswers)); } }  
    public ICommand MarkAnswerCommand { get; set; }
    public ICommand CheckAnswersCommand { get; set; }
    public ICommand ComprehensionTestUpdateViewCommand { get; set; }
    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set { _selectedViewModel = value; OnPropertyChanged(nameof(SelectedViewModel)); }
    }
    public ComprehensionTestViewModel(ObservableCollection<TextQuestionTestModel> textQuestions, ObservableCollection<string> correctAnswers, string _translated_Text, string _tTS_Text, string _title, string Lang) 
    { 
      this.Translated_Text = _translated_Text;
      this.CorrectAnswers = correctAnswers;
      this.TextQuestions = textQuestions;
      this.TTS_Text = _tTS_Text;
      this.Title = _title;
      this.answers = new string[correctAnswers.Count()];
      this.points = 0;
      this.Lang = Lang;
      Enabler = true;
      Tip_Enabler = false;
      ComprehensionTestUpdateViewCommand = new ComprehensionTestUpdateViewCommand(Lang, this);
      MarkAnswerCommand = new RelayCommand(ExecuteMarkAnswerCommand);
      CheckAnswersCommand = new RelayCommand(ExecuteCheckAnswersCommand);
    }
    public ComprehensionTestViewModel(ObservableCollection<TextQuestionTestModel> textQuestions, ObservableCollection<string> correctAnswers, string _translated_Text, string _tTS_Text, string _title, string Lang, bool IsTestMode, int points)
    {
      this.Translated_Text = _translated_Text;
      this.CorrectAnswers = correctAnswers;
      this.TextQuestions = textQuestions;
      this.TTS_Text = _tTS_Text;
      this.Title = _title;
      this.answers = new string[correctAnswers.Count()];
      this.points = 0;
      ComprehensionTestUpdateViewCommand = new ComprehensionTestUpdateViewCommand(Lang, this);
      MarkAnswerCommand = new RelayCommand(ExecuteMarkAnswerCommand);
      CheckAnswersCommand = new RelayCommand(ExecuteCheckAnswersCommand);
      Enabler = true;
      Tip_Enabler = false;
      this.IsTestMode = IsTestMode;
      this.points = points;
    }

    public void ExecuteMarkAnswerCommand(object parameter)
    {
      var values = (object[]) parameter;
      var GroupName = values[0];
      var Answer = values[1];
      answers[Int32.Parse(GroupName.ToString())] = Answer.ToString();
    }
    public void ExecuteCheckAnswersCommand(object parameter)
    {
      Enabler = false;
      if(!IsTestMode)
      {
        for (int i = 0; i < CorrectAnswers.Count; i++)
        {
          if (answers[i] == CorrectAnswers[i])
          {
            points++;
            TextQuestions[i].Answer_Tip = "Correct answer!";
          }
        }
        Tip_Enabler = true;
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
          {
            points++;
          }
        }
        Tip_Enabler = true;
        if (points > 15)
          Score = "Wynik: " + points.ToString() + ". Osiągnąłeś kolejny poziom nauki!";
        else
          Score = "Wynik: " + points.ToString();
      }
    }
  }
}
