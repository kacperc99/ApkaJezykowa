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
  internal class LanguagesViewModelTest
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void FrenchConditionTest()
    {
      string Username = "admin";
      Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
      var FrenchViewModel = new FrenchViewModel();
      Assert.IsTrue((bool)FrenchViewModel.Check, "Użytkownik nie powinien mieć przypisanego poziomu");
    }
    [Test]
    public void EnglishConditionTest()
    {
      string Username = "admin";
      Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
      var EnglishViewModel = new EnglishViewModel();
      Assert.IsTrue(EnglishViewModel.Check, "Użytkownik nie powinien mieć przypisanego poziomu");
    }
    [Test]
    public void GermanConditionTest()
    {
      string Username = "admin";
      Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
      var GermanViewModel = new GermanViewModel();
      Assert.IsTrue(GermanViewModel.Check, "Użytkownik nie powinien mieć przypisanego poziomu");
    }
  }
}
