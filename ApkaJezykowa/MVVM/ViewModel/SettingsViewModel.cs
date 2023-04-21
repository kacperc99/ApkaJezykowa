using ApkaJezykowa.MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ApkaJezykowa.MVVM.ViewModel
{
  internal class SettingsViewModel : BaseViewModel
  {
    Uri _filename;

    public Uri Filename { get { return _filename; } set { _filename = value; OnPropertyChanged(nameof(Filename)); } }

    public SettingsViewModel()
    {
      test();
    }

    void test()
    {
      var SettingsView = new SettingsView();
      //Filename = new Uri((BitmapImage)SettingsView.ImageDisplay.Source);
    }
  }
}
