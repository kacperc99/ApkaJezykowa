using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.ViewModel;
using ApkaJezykowa.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApkaJezykowa.MVVM.View
{
  /// <summary>
  /// Logika interakcji dla klasy FrenchLessonView.xaml
  /// </summary>
  public partial class FrenchLessonView : UserControl
  {

    public FrenchLessonView()
    {
      InitializeComponent();
    }

    private void LV_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        LV.Height = HeightModel.Instance.Height;
    }

    private void LV2_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        LV2.Height = HeightModel.Instance.Height;
    }

    private void SV_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      SV.Height = HeightModel.Instance.Height - 100;
      SV.Width = HeightModel.Instance.Width - 120;
    }
  }
}
