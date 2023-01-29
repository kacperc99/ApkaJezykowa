using NUnit.Framework;
using System;
using ApkaJezykowa.MVVM.ViewModel;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.MVVM.View;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Principal;
using System.Net;
using System.Data.SqlClient;

namespace ApkaJezykowaTest
{
  public class MainViewModelTests : MainViewModel
  {
    [SetUp]
    public void Setup()
    {

    }

    [Test]
    public void UsernameVerification()
    {
      string Username = "admin";
      Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
      var MainViewModel = new MainViewModel();
      MainViewModel.LoadCurrentUserData();
      Assert.AreEqual(MainViewModel.WelcomeMessage, $"Witaj, admin", "musi pyknąć");
      
    }
  }
}
