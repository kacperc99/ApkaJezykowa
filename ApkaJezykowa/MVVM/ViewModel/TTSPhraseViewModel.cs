using ApkaJezykowa.Main;
using ApkaJezykowa.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using System.Windows.Media;
using System.Globalization;
using Syncfusion.Windows.Shared.Resources;
using System.Windows;
using System.Speech.Synthesis;
using ApkaJezykowa.Commands;

namespace ApkaJezykowa.MVVM.ViewModel
{
  public class TTS
  {
    public string _phrase;
    public string _phrase_Translated;
  }
  public class TTSPhraseViewModel : BaseViewModel
  {
    public ObservableCollection<TTS> phrases = new ObservableCollection<TTS>();
    public bool IsTestMode = false;
    public string _task;
    public string _phrase;
    public string _answer;
    public string _colour;
    public string _next_Button_Text;
    public bool _phrase_Switch;
    public bool _button_Switch;
    public bool _speak_Switch;
    public bool _answer_Read_Only;
    public int points;
    public int num;
    public int counter=0;
    public int Id_Listening;
    public int Id_Vocabulary;
    public string Lang;
    public BaseViewModel _selectedViewModel;
    private IListeningRepository listeningRepository;
    public string Task { get { return _task; } set { _task = value; OnPropertyChanged(nameof(Task)); } }
    public string Phrase { get { return _phrase; } set { _phrase = value; OnPropertyChanged(nameof(Phrase)); } }
    public string Answer { get { return _answer; } set { _answer = value; OnPropertyChanged(nameof(Answer)); } }
    public string Colour { get { return _colour; } set { _colour = value; OnPropertyChanged(nameof(Colour)); } }
    public string Next_Button_Text { get { return _next_Button_Text; } set { _next_Button_Text = value; OnPropertyChanged(nameof(Next_Button_Text)); } }
    public bool Phrase_Switch { get { return _phrase_Switch; } set { _phrase_Switch = value; OnPropertyChanged(nameof(Phrase_Switch)); } }
    public bool Button_Switch { get { return _button_Switch; } set { _button_Switch = value; OnPropertyChanged(nameof(Button_Switch)); } }
    public bool Speak_Switch { get { return _speak_Switch; } set { _speak_Switch = value; OnPropertyChanged(nameof(Speak_Switch)); } }
    public bool Answer_Read_Only { get { return _answer_Read_Only; } set { _answer_Read_Only = value; OnPropertyChanged(nameof(Answer_Read_Only)); } }
    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set { _selectedViewModel = value; OnPropertyChanged(nameof(SelectedViewModel)); }
    }
    public ICommand TTSPhraseUpdateViewCommand { get; set; }
    public ICommand Check { get; }
    public ICommand Speak { get; }
    public ICommand Play { get; }
    public TTSPhraseViewModel(int Id_Listening, string Lang) 
    {
      this.Id_Listening = Id_Listening;
      this.Lang = Lang;
      listeningRepository.GetPhrases(phrases, Id_Listening, Lang);
      Check = new RelayCommand(ExecuteCheck);
      Speak = new RelayCommand(ExecuteSpeak);
      Play = new RelayCommand(ExecutePlay);
      TTSPhraseUpdateViewCommand = new TTSPhraseUpdateViewCommand(Id_Listening, Lang, points, this);

      Randomize_Task();
    }
    public TTSPhraseViewModel(int Id_Vocabulary, string Lang, bool IsTestMode)
    {
      this.Id_Vocabulary = Id_Vocabulary;
      this.Lang = Lang;
      this.IsTestMode = true;
      listeningRepository.GetTestPhrases(phrases, Id_Vocabulary, Lang);
      Check = new RelayCommand(ExecuteCheck);
      Speak = new RelayCommand(ExecuteSpeak);
      Play = new RelayCommand(ExecutePlay);
      TTSPhraseUpdateViewCommand = new TTSPhraseUpdateViewCommand(Id_Vocabulary, Lang, points, this, true);

      Randomize_Task();
    }
    void Randomize_Task()
    {
      Random rnd = new Random();
      num = rnd.Next(1, 5);
      switch (num)
      {//najlepiej będzie ustawić tutaj wszystkie zmienne i tyle,
       //a te funkcje niech odpowiadają po prostu za sammo wykonanie sprawdzenia
        case 1:
          //ListenAndWrite();
          Task = "Transcribe the sentence correctly";
          Phrase = null;
          Colour = "Black";
          Button_Switch = true;
          Phrase_Switch = false;
          Speak_Switch = false;
          Answer_Read_Only = false;
          Next_Button_Text = "Next";
          break;
        case 2:
          //ReadItOut();
          Task = "Pronouonce the phrase correctly";
          Phrase = phrases[counter]._phrase;
          Colour = "Black";
          Button_Switch = false;
          Phrase_Switch = true;
          Speak_Switch = true;
          Answer_Read_Only = true;
          Next_Button_Text = "Skip";
          break;
        case 3:
          Task = "Translate the written phrase correctly";
          Phrase = phrases[counter]._phrase;
          Colour = "Black";
          Button_Switch = false;
          Phrase_Switch = true;
          Speak_Switch = false;
          Answer_Read_Only = false;
          Next_Button_Text = "Next";
          //TranslateText();
          break;
        case 4:
          //TranslateSpoken();
          Task = "Transcribe and translate the sentence correctly";
          Phrase = null;
          Colour = "Black";
          Button_Switch = true;
          Phrase_Switch = false;
          Speak_Switch = false;
          Answer_Read_Only = false;
          Next_Button_Text = "Next";
          break;
      }
    }
    void ExecuteCheck(object obj)
    {
      switch (num)
      {
        case 1:
          ListenAndWrite();
          break;
        case 2:
          ReadItOut();
          break;
        case 3:
          TranslateText();
          break;
        case 4:
          TranslateSpoken();
          break;
      }
      if(counter==phrases.Count()-1)
      {
        TTSPhraseUpdateViewCommand.Execute(null);
      }
      else
      {
        counter++;
        Randomize_Task();
      }
    }
    void ExecuteSpeak(object obj)
    {
      SpeechRecognitionEngine p = new SpeechRecognitionEngine(new CultureInfo("fr-FR"));
      Grammar word = new DictationGrammar();
      p.LoadGrammar(word);
      try
      {
        p.SetInputToDefaultAudioDevice();
        RecognitionResult result = p.Recognize();
        Answer = result.Text;
        Next_Button_Text = "Next";
      }
      catch (Exception ex)
      {
        Answer = "";
        MessageBox.Show(ex.ToString());
      }
      finally
      {
        p.UnloadAllGrammars();
      } 
    }
    void ExecutePlay(object obj)
    {
      SpeechSynthesizer tts = new SpeechSynthesizer();
      tts.SelectVoiceByHints(VoiceGender.Male, VoiceAge.Adult, 25,new CultureInfo("fr-FR",false));
      tts.Volume = 40;
      tts.Speak(Phrase);
    }
    void ListenAndWrite()
    {
      if(Answer==Phrase)
        points++;
    }
    void ReadItOut()
    {
      if (Answer == Phrase)
        points++;
    }
    void TranslateText()
    {
      if (Answer == phrases[counter]._phrase_Translated)
        points++;
    }
    void TranslateSpoken()
    {
      if (Answer == phrases[counter]._phrase_Translated)
        points++;
    }
  }
}
