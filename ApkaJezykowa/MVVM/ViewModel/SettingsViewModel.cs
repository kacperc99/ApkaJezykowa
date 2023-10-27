using ApkaJezykowa.Main;
using ApkaJezykowa.MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ApkaJezykowa.MVVM.ViewModel
{
  internal class SettingsViewModel : BaseViewModel
  {
    Uri _filename;
    public string _language;
    public Uri Filename { get { return _filename; } set { _filename = value; OnPropertyChanged(nameof(Filename)); } }
    public string Language { get { return _language; } set { _language = value; OnPropertyChanged(nameof(Language)); } }

  public ICommand LanguageCommand { get; set; }
    public SettingsViewModel()
    {
      test();
      LanguageCommand = new RelayCommand(ExecuteLanguageCommand);
    }

    void test()
    {
      //var SettingsView = new SettingsView();
      //Filename = new Uri((BitmapImage)SettingsView.ImageDisplay.Source);
    }
    public void ExecuteLanguageCommand(object obj)
    {
      Properties.Settings.Default.Language = Language;
      Properties.Settings.Default.Save();
    }
  }
}
