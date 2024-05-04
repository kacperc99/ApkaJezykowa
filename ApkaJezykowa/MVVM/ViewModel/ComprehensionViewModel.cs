using ApkaJezykowa.Commands;
using ApkaJezykowa.Keys;
using ApkaJezykowa.Main;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;
//using AzureVaultKeyAccessProvider.Keys;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO.Pipes;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.MVVM.ViewModel
{
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public class ComprehensionViewModel : BaseViewModel
  {
    string Lang;
    int Id_Comprehension;
    int Id_Vocabulary;
    string _title;
    string _tTS_Text;
    byte[] _illustration;
    bool _buttonSwitch = false;
    bool IsTestMode = false;
    int points;
    public string Accent;
    public string Lang_Accent;
    public string Voice;
    ObservableCollection<TextWordbookModel> _textWordbook = new ObservableCollection<TextWordbookModel>();
    ObservableCollection<TextQuestionTestModel> textQuestions = new ObservableCollection<TextQuestionTestModel>();
    ObservableCollection<string> correctAnswers = new ObservableCollection<string>();
    public BaseViewModel _selectedViewModel;
    public string Title { get { return _title; } set { _title = value; OnPropertyChanged(nameof(Title)); } }
    public string TTS_Text { get { return _tTS_Text; } set { _tTS_Text = value; OnPropertyChanged(nameof(TTS_Text)); } }
    public byte[] Illustration { get { return _illustration; } set { _illustration = value; OnPropertyChanged(nameof(Illustration)); } }
    public bool ButtonSwitch { get { return _buttonSwitch; } set { _buttonSwitch = value; OnPropertyChanged(nameof(ButtonSwitch)); } }
    public ObservableCollection<TextWordbookModel> TextWordBook { get { return _textWordbook; } set { _textWordbook = value; OnPropertyChanged(nameof(TextWordBook)); } }
    string Translated_Text;
    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set { _selectedViewModel = value; OnPropertyChanged(nameof(SelectedViewModel)); }
    }
    private IComprehensionRepository comprehensionRepository;
    private IVocabularyRepository vocabularyRepository;
    public ICommand ComprehensionUpdateViewCommand { get; set; }
    public ICommand MoveToNextScreenCommand { get; }
    public ComprehensionViewModel(int Id_Comprehension, string Lang)
    {
      this.Id_Comprehension = Id_Comprehension;
      this.Lang = Lang;
      comprehensionRepository = new ComprehensionRepository();
      vocabularyRepository = new VocabularyRepository();
      //wrzuć wszystko przez repo to var text czy coś
      MoveToNextScreenCommand = new RelayCommand(ExecuteMoveToNextScreenCommand);
      //ComprehensionUpdateViewCommand = new ComprehensionUpdateViewCommand(this, textQuestions, correctAnswers, Translated_Text, TTS_Text, Title, Lang);
      var result = comprehensionRepository.Obtain_Text(Id_Comprehension);
      this.Title = result.Text_Title;
      this.TTS_Text = result.TTS_Text;
      this.Illustration = result.Illustration;
      comprehensionRepository.Obtain_Dictionary(result.Id_Reading_Text, TextWordBook);
      var result2 = vocabularyRepository.GetAccent(Lang);
      Accent = result2.Accent;
      Lang_Accent = result2.Lang;
      Voice = result2.Voice;
      //ReadText();
      Data_Obtainer(Id_Comprehension, result.Id_Reading_Text);

      //a tutaj pobierz słownik i asynchronicznie w tle pobierz resztę danych
    }
    public ComprehensionViewModel(int Id_Vocabulary, string Lang, bool IsTestMode, int points)
    {
      this.Id_Vocabulary = Id_Vocabulary;
      this.Lang = Lang;
      this.IsTestMode = IsTestMode;
      this.points = points;
      comprehensionRepository = new ComprehensionRepository();
      vocabularyRepository = new VocabularyRepository();
      MoveToNextScreenCommand = new RelayCommand(ExecuteMoveToNextScreenCommand);
      //ComprehensionUpdateViewCommand = new ComprehensionUpdateViewCommand(this, textQuestions, correctAnswers, Translated_Text, TTS_Text, Title, Lang, true, points);
      //this.Id_Comprehension = comprehensionRepository.Get_Comprehension_Int(Id_Vocabulary);
      var result = comprehensionRepository.Obtain_Test_Text(Id_Vocabulary);
      this.Title = result.Text_Title;
      this.TTS_Text = result.TTS_Text;
      this.Illustration = result.Illustration;
      comprehensionRepository.Obtain_Dictionary(result.Id_Reading_Text, TextWordBook);
      var result2 = vocabularyRepository.GetAccent(Lang);
      Accent = result2.Accent;
      Lang_Accent = result2.Lang;
      Voice = result2.Voice;
      Data_Obtainer(Id_Comprehension, result.Id_Reading_Text);

      //a tutaj pobierz słownik i asynchronicznie w tle pobierz resztę danych
    }
    async Task Data_Obtainer(int id, int reader_id)
    {
      //Translated_Text = Task.Run(()=>comprehensionRepository.Obtain_Translation(Id_Comprehension)).ToString();
      Translated_Text = await Get_Translation(id);
      textQuestions = await Get_Questions(reader_id);
      ButtonSwitch = true;
      ReadText();
    }
    Task<string> Get_Translation(int id)
    {
      return Task.Run(() => comprehensionRepository.Obtain_Translation(Id_Comprehension));
    }
    Task<ObservableCollection<TextQuestionTestModel>> Get_Questions(int id)
    {
      return Task.Run(() => comprehensionRepository.Obtain_Questions(id, correctAnswers));
    }
    async void ReadText()
    {
      var Key = SpeechServiceKey.Instance.Key;
      var Region = SpeechServiceKey.Instance.Region;
      var speechConfig = SpeechConfig.FromSubscription(Key, Region);
      speechConfig.SpeechRecognitionLanguage = Accent;
      speechConfig.SpeechSynthesisVoiceName = Voice;
      using (var synthesizer = new Microsoft.CognitiveServices.Speech.SpeechSynthesizer(speechConfig))
      {
        await synthesizer.SpeakTextAsync(TTS_Text);
      }
    }
    void ExecuteMoveToNextScreenCommand(object parameter)
    {
      if(IsTestMode)
        ComprehensionUpdateViewCommand = new ComprehensionUpdateViewCommand(this, textQuestions, correctAnswers, Translated_Text, TTS_Text, Title, Lang, true, points);
      else
        ComprehensionUpdateViewCommand = new ComprehensionUpdateViewCommand(this, textQuestions, correctAnswers, Translated_Text, TTS_Text, Title, Lang);
      ComprehensionUpdateViewCommand.Execute("GoToQuestions");
    }
  }
}
