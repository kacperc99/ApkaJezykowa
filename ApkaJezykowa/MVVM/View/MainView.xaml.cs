using ApkaJezykowa.MVVM.ViewModel;
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
  /// Logika interakcji dla klasy MainWindow.xaml
  /// </summary>
  public partial class MainWindow : UserControl
  {
    public MainWindow()
    {
      InitializeComponent();

      DataContext = new MainViewModel();
    }

    private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
    {
      if (tg_btn.IsChecked == true)
      {
        tt_home.Visibility = Visibility.Collapsed;
        tt_lessons.Visibility = Visibility.Collapsed;
        tt_dictionary.Visibility = Visibility.Collapsed;
        tt_information.Visibility = Visibility.Collapsed;
        tt_settings.Visibility = Visibility.Collapsed;
        tt_logout.Visibility = Visibility.Collapsed;
      }
      else
      {
        tt_home.Visibility = Visibility.Visible;
        tt_lessons.Visibility = Visibility.Visible;
        tt_dictionary.Visibility = Visibility.Visible;
        tt_information.Visibility = Visibility.Visible;
        tt_settings.Visibility = Visibility.Visible;
        tt_logout.Visibility = Visibility.Visible;
      }
    }

    private void tg_btn_Unchecked(object sender, RoutedEventArgs e)
    {
      img_bg.Opacity = 0.2;
    }

    private void tg_btn_Checked(object sender, RoutedEventArgs e)
    {
      img_bg.Opacity = 0.1;
    }

    private void BG_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      tg_btn.IsChecked = false;
    }

    private void LVI_end_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      //Close();
    }
    private void Logout_Click(object sender, RoutedEventArgs e)
    {
      
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
      var window = Window.GetWindow(this);
      window.Close();
    }

    
  }
}
