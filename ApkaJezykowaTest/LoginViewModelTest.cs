using NUnit.Framework;
using ApkaJezykowa.MVVM.ViewModel;
using System.Net;
using Moq;
using System.Security;

namespace ApkaJezykowaTest
{
  public class LoginViewModelTests : LoginViewModel
  {
    
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void RegisterEmailVerification()
    {
      var LoginViewModel = new LoginViewModel();

      LoginViewModel.RUsername = "admin";
      LoginViewModel.RPassword = new NetworkCredential("", "admin").SecurePassword;
      LoginViewModel.Email = "xd";
      LoginViewModel.RPasswordRepeat = new NetworkCredential("", "admin").SecurePassword;
      LoginViewModel.Country = "Poland";
      LoginViewModel.RegisterCommand.Execute(null);
      Assert.AreEqual(LoginViewModel.RegisterMessage, "* nieprawid³owa nazwa Emaila", "Powinno wykryæ b³êny mail");
    }
    [Test]
    public void RegisterUsernameVerification()
    {
      var LoginViewModel = new LoginViewModel();

      LoginViewModel.RUsername = "admin";
      LoginViewModel.RPassword = new NetworkCredential("", "admin").SecurePassword;
      LoginViewModel.Email = "xd@gmail.com";
      LoginViewModel.RPasswordRepeat = new NetworkCredential("", "admin").SecurePassword;
      LoginViewModel.Country = "Poland";
      LoginViewModel.RegisterCommand.Execute(null);
      Assert.AreEqual(LoginViewModel.RegisterMessage, "* Login lub email ju¿ istniej¹ w bazie danych", "U¿ytkownik powinien figurowaæ w bazie danych");
    }
    [Test]
    public void RegisterEmailVerification2()
    {
      var LoginViewModel = new LoginViewModel();

      LoginViewModel.RUsername = "admun";
      LoginViewModel.RPassword = new NetworkCredential("", "admin").SecurePassword;
      LoginViewModel.Email = "p@gmail.com";
      LoginViewModel.RPasswordRepeat = new NetworkCredential("", "admin").SecurePassword;
      LoginViewModel.Country = "Poland";
      LoginViewModel.RegisterCommand.Execute(null);
      Assert.AreEqual(LoginViewModel.RegisterMessage, "* Login lub email ju¿ istniej¹ w bazie danych", "U¿ytkownik powinien figurowaæ w bazie danych");
    }
    [Test]
    public void RegisterPasswordVerification()
    {
      var LoginViewModel = new LoginViewModel();

      LoginViewModel.RUsername = "admen";
      LoginViewModel.RPassword = new NetworkCredential("", "admin").SecurePassword;
      LoginViewModel.Email = "xd@gmail.com";
      LoginViewModel.RPasswordRepeat = new NetworkCredential("", "admun").SecurePassword;
      LoginViewModel.Country = "Poland";
      LoginViewModel.RegisterCommand.Execute(null);
      Assert.AreEqual(LoginViewModel.RegisterMessage, "* Has³a nie s¹ identyczne", "Powinno wykryæ ró¿ne has³a");
    }
    [Test]
    public void LoginUsernameVerification()
    {
      var LoginViewModel = new LoginViewModel();

      LoginViewModel.Username = "adm3n";
      LoginViewModel.Password = new NetworkCredential("", "admin").SecurePassword;
      LoginViewModel.LoginCommand.Execute(null);
      Assert.AreEqual(LoginViewModel.ErrorMessage, "* B³êdny Login lub Has³o", "Powinno wykryæ b³êdn¹ nazwê u¿ytkownika");
    }
    [Test]
    public void LoginPasswordVerification()
    {
      var LoginViewModel = new LoginViewModel();

      LoginViewModel.Username = "admin";
      LoginViewModel.Password = new NetworkCredential("", "admen").SecurePassword;
      LoginViewModel.LoginCommand.Execute(null);
      Assert.AreEqual(LoginViewModel.ErrorMessage, "* B³êdny Login lub Has³o", "Powinno wykryæ b³êdne has³o");
    }
    [Test]
    public void TestTest()
    {
      Assert.AreEqual(true, true, "p");
    }
  }
}