using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ApkaJezykowa.MVVM.View;

namespace ApkaJezykowa
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
    /*protected void Login(object sender, StartupEventArgs e)
    {
      var loginView = new LoginView();
      loginView.Show();
    loginView.IsVisibleChanged += (s, ev) =>
    {
      var mainView = new MainWindow();
      if (loginView.IsVisible == false && loginView.IsLoaded)
      {
        mainView.Show();
        loginView.Close();
      }
      if (mainView.IsVisible == false && mainView.IsLoaded)
      {
        loginView.Show();
      }
    };
    }*/
  }
}
