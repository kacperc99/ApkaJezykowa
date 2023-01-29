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
      Assert.AreEqual(LoginViewModel.RegisterMessage, "* nieprawid�owa nazwa Emaila", "Powinno wykry� b��ny mail");
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
      Assert.AreEqual(LoginViewModel.RegisterMessage, "* Login lub email ju� istniej� w bazie danych", "U�ytkownik powinien figurowa� w bazie danych");
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
      Assert.AreEqual(LoginViewModel.RegisterMessage, "* Login lub email ju� istniej� w bazie danych", "U�ytkownik powinien figurowa� w bazie danych");
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
      Assert.AreEqual(LoginViewModel.RegisterMessage, "* Has�a nie s� identyczne", "Powinno wykry� r�ne has�a");
    }
    [Test]
    public void LoginUsernameVerification()
    {
      var LoginViewModel = new LoginViewModel();

      LoginViewModel.Username = "adm3n";
      LoginViewModel.Password = new NetworkCredential("", "admin").SecurePassword;
      LoginViewModel.LoginCommand.Execute(null);
      Assert.AreEqual(LoginViewModel.ErrorMessage, "* B��dny Login lub Has�o", "Powinno wykry� b��dn� nazw� u�ytkownika");
    }
    [Test]
    public void LoginPasswordVerification()
    {
      var LoginViewModel = new LoginViewModel();

      LoginViewModel.Username = "admin";
      LoginViewModel.Password = new NetworkCredential("", "admen").SecurePassword;
      LoginViewModel.LoginCommand.Execute(null);
      Assert.AreEqual(LoginViewModel.ErrorMessage, "* B��dny Login lub Has�o", "Powinno wykry� b��dne has�o");
    }
    [Test]
    public void TestTest()
    {
      Assert.AreEqual(true, true, "p");
    }
  }
}