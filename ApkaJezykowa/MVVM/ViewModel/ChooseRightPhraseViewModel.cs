using ApkaJezykowa.Commands;
using ApkaJezykowa.Keys;
using ApkaJezykowa.Main;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;
using Microsoft.CognitiveServices.Speech;
using Syncfusion;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.MVVM.ViewModel
{
  public class TaskTemplate
  {
    public string _description;
    public string _tts_phrase;
    public string _correct_answer;
    public ObservableCollection<string> _answers = new ObservableCollection<string>();
  }
  public class ChooseRightPhraseViewModel : BaseViewModel
  {
    public ObservableCollection<TaskTemplate> data = new ObservableCollection<TaskTemplate>();
    int Id_Listening;
    int Id_Vocabulary;
    string Lang;
    bool IsTestMode = false;
    int points;
    string chosen_answer;
    public string Accent;
    public string Lang_Accent;
    public string Voice;
    public bool _enabler;
    bool is_done=false;
    public bool _button_Enabler;
    int count = 0;
    string _description;
    string _tTS_Phrase;
    string _correct_Answer;
    string _answer1;
    string _answer2;
    string _answer3;
    string _answer4;
    string _colour1;
    string _colour2;
    string _colour3;
    string _colour4;
    string _score;
    string _next_Button_Text;
    private IListeningRepository listeningRepository;
    private IVocabularyRepository vocabularyRepository;
    public BaseViewModel _selectedViewModel;
    public string Description { get {  return _description; } set { _description = value; OnPropertyChanged(nameof(Description)); } }
    public string TTS_Phrase { get { return _tTS_Phrase; } set { _tTS_Phrase = value; OnPropertyChanged(nameof(TTS_Phrase)); } }
    public string Correct_Answer { get { return _correct_Answer; } set { _correct_Answer = value; OnPropertyChanged(nameof(Correct_Answer)); } }
    public string Answer1 { get { return _answer1; } set { _answer1 = value; OnPropertyChanged(nameof(Answer1)); } }
    public string Answer2 { get { return _answer2; } set { _answer2 = value; OnPropertyChanged(nameof(Answer2)); } }
    public string Answer3 { get { return _answer3; } set { _answer3 = value; OnPropertyChanged(nameof(Answer3)); } }
    public string Answer4 { get { return _answer4; } set { _answer4 = value; OnPropertyChanged(nameof(Answer4)); } }
    public string Colour1 { get { return _colour1; } set { _colour1 = value; OnPropertyChanged(nameof(Colour1)); } }
    public string Colour2 { get { return _colour2; } set { _colour2 = value; OnPropertyChanged(nameof(Colour2)); } }
    public string Colour3 { get { return _colour3; } set { _colour3 = value; OnPropertyChanged(nameof(Colour3)); } }
    public string Colour4 { get { return _colour4; } set { _colour4 = value; OnPropertyChanged(nameof(Colour4)); } }
    public string Score { get { return _score; } set { _score = value; OnPropertyChanged(nameof(Score)); } }
    public string Next_Button_Text { get { return _next_Button_Text; } set { _next_Button_Text = value; OnPropertyChanged(nameof(Next_Button_Text)); } }
    public bool Enabler { get { return _enabler; } set { _enabler = value; OnPropertyChanged(nameof(Enabler)); } }
    public bool Button_Enabler { get { return _button_Enabler; } set { _button_Enabler = value; OnPropertyChanged(nameof(Button_Enabler)); } }
    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set { _selectedViewModel = value; OnPropertyChanged(nameof(SelectedViewModel)); }
    }
    public ICommand MarkAnswer { get; set; }
    public ICommand Play { get; }
    public ICommand ChooseRightPhraseUpdateViewCommand { get; set; }
    public ICommand Check { get; set; }
    public ChooseRightPhraseViewModel(int Id_Listening, string Lang, int points) 
    { 
      this.Id_Listening = Id_Listening;
      this.Lang = Lang;
      this.points = points;
      Button_Enabler = true;
      listeningRepository = new ListeningRepository();
      vocabularyRepository = new VocabularyRepository();
      ChooseRightPhraseUpdateViewCommand = new ChooseRightPhraseUpdateViewCommand(this, Lang);
      var result = vocabularyRepository.GetAccent(Lang);
      Accent = result.Accent;
      Lang_Accent = result.Lang;
      Voice = result.Voice;
      MarkAnswer = new RelayCommand(ExecuteMarkAnswer);
      Play = new RelayCommand(ExecutePlay);
      Check = new RelayCommand(ExecuteCheck);
      listeningRepository.GetAnswers(data, Id_Listening, Lang);
      GetQuestion();
    }
    public ChooseRightPhraseViewModel(int Id_Vocabulary, string Lang, int points, bool IsTestMode)
    {
      this.Id_Vocabulary = Id_Vocabulary;
      this.Lang = Lang;
      this.points = points;
      this.IsTestMode = IsTestMode;
      Button_Enabler = true;
      listeningRepository = new ListeningRepository();
      vocabularyRepository = new VocabularyRepository();
      //ChooseRightPhraseUpdateViewCommand = new ChooseRightPhraseUpdateViewCommand(this, Lang, Id_Vocabulary, points);
      var result = vocabularyRepository.GetAccent(Lang);
      Accent = result.Accent;
      Lang_Accent = result.Lang;
      Voice = result.Voice;
      MarkAnswer = new RelayCommand(ExecuteMarkAnswer);
      Play = new RelayCommand(ExecutePlay);
      Check = new RelayCommand(ExecuteCheck);
      listeningRepository.GetTestAnswers(data, Id_Vocabulary, Lang);
      GetQuestion();
    }
    void GetQuestion()
    {
      Description = data[count]._description;
      TTS_Phrase = data[count]._tts_phrase;
      Correct_Answer = data[count]._correct_answer;
      Answer1 = data[count]._answers[0];
      Answer2 = data[count]._answers[1];
      Answer3 = data[count]._answers[2];
      Answer4 = data[count]._answers[3];
      Colour1 = "CadetBlue";
      Colour2 = "CadetBlue";
      Colour3 = "CadetBlue";
      Colour4 = "CadetBlue";
      Next_Button_Text = "Skip";
      Enabler = true;
    }
    public async void ExecutePlay(object obj)
    {
      var Key = SpeechServiceKey.Instance.Key;
      var Region = SpeechServiceKey.Instance.Region;
      var speechConfig = SpeechConfig.FromSubscription(Key, Region);
      speechConfig.SpeechRecognitionLanguage = Accent;
      speechConfig.SpeechSynthesisVoiceName = Voice;
      using (var synthesizer = new Microsoft.CognitiveServices.Speech.SpeechSynthesizer(speechConfig))
      {
        await synthesizer.SpeakTextAsync(TTS_Phrase);
      }
    }
    public void ExecuteMarkAnswer(object parameter)
    {
      switch(parameter.ToString())
      {
        case "Answer1":
          Colour1 = "Yellow";
          Colour2 = "CadetBlue";
          Colour3 = "CadetBlue";
          Colour4 = "CadetBlue";
          chosen_answer = Answer1;
          Next_Button_Text = "Check";
          break;
        case "Answer2":
          Colour2 = "Yellow";
          Colour1 = "CadetBlue";
          Colour3 = "CadetBlue";
          Colour4 = "CadetBlue";
          chosen_answer = Answer2;
          Next_Button_Text = "Check";
          break;
        case "Answer3":
          Colour3 = "Yellow";
          Colour2 = "CadetBlue";
          Colour1 = "CadetBlue";
          Colour4 = "CadetBlue";
          chosen_answer = Answer3;
          Next_Button_Text = "Check";
          break;
        case "Answer4":
          Colour4 = "Yellow";
          Colour2 = "CadetBlue";
          Colour3 = "CadetBlue";
          Colour1 = "CadetBlue";
          chosen_answer = Answer4;
          Next_Button_Text = "Check";
          break;
      }
    }
    public void ExecuteCheck(object parameter) 
    {
      
      if(chosen_answer == Correct_Answer && is_done==false)
      {
        Enabler = false;
        points++;
        Next_Button_Text = "Next";
        //I'm aware that overall design of this section is terrible, I will redesign it later, for now it just has to work
        if (chosen_answer == Answer1)
          Colour1 = "Green";
        else if (chosen_answer == Answer2)
          Colour2 = "Green";
        else if (chosen_answer == Answer3)
          Colour3 = "Green";
        else if (chosen_answer == Answer4)
          Colour4 = "Green";

        is_done = true;
      }
      else if (chosen_answer != Correct_Answer && is_done == false)
      {
        Enabler = false;
        Next_Button_Text = "Next";
        if (Correct_Answer == Answer1)
          Colour1 = "Green";
        else if (Correct_Answer == Answer2)
          Colour2 = "Green";
        else if (Correct_Answer == Answer3)
          Colour3 = "Green";
        else if (Correct_Answer == Answer4)
          Colour4 = "Green";
        if (chosen_answer == Answer1)
          Colour1 = "Red";
        else if (chosen_answer == Answer2)
          Colour2 = "Red";
        else if (chosen_answer == Answer3)
          Colour3 = "Red";
        else if (chosen_answer == Answer4)
          Colour4 = "Red";

        is_done = true;
      }
      else if (chosen_answer == null || is_done == true)
      {
        if (count < data.Count() - 1)
        {
          is_done = false;
          count++;
          GetQuestion();
        }
        else
        {
          if(IsTestMode)
          {
            ChooseRightPhraseUpdateViewCommand = new ChooseRightPhraseUpdateViewCommand(this, Lang, Id_Vocabulary, points);
            ChooseRightPhraseUpdateViewCommand.Execute("Test");
          }
          else
          {
            if (points > 7)
              Score = "Your score: " + points.ToString() + ". Congratulations!";
            else
              Score = "Your score: " + points.ToString();
            Button_Enabler = false;
            Enabler = false;
            //switch screen
          }
        }
      }
    }
  }
}
