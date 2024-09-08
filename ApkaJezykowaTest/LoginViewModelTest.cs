using NUnit.Framework;
using ApkaJezykowa.MVVM.ViewModel;
using System.Net;
using Moq;
using System.Security;
using ApkaJezykowa.Keys;
using ApkaJezykowa.MVVM.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace ApkaJezykowaTest
{
  public class LoginViewModelTests
  {

    [SetUp]
    public void Setup()
    {
      MeasurementModel.Instance.cpu = new("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
      MeasurementModel.Instance.ram = new("Process", "Working Set", Process.GetCurrentProcess().ProcessName);
      MeasurementModel.Instance.Measurement_Results = new List<Tuple<string, List<double>, List<float>, TimeSpan, double, float>>();
      MeasurementModel.Instance.CPU_Vals = new List<double>();
      MeasurementModel.Instance.RAM_Vals = new List<float>();
      SpeechServiceKey.Instance.UserBaseConnection = @"Server=tcp:linguonator2.database.windows.net,1433;Initial Catalog=UserBase;Persist Security Info=False;User ID=r4fxdvt7fu;Password=jkhGKjuFKku653212#123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets=true";
      SpeechServiceKey.Instance.CourseBaseConnection = @"Server=tcp:linguonator2.database.windows.net,1433;Initial Catalog=CourseBase;Persist Security Info=False;User ID=r4fxdvt7fu;Password=jkhGKjuFKku653212#123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets=true";
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
      Assert.AreEqual(LoginViewModel.RegisterMessage, "* Incorrect e-mail", "Powinno wykryæ b³êny mail");
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
      Assert.AreEqual(LoginViewModel.RegisterMessage, "* Login or e-mail already exist in a database", "U¿ytkownik powinien figurowaæ w bazie danych");
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
      Assert.AreEqual(LoginViewModel.RegisterMessage, "* Login or e-mail already exist in a database", "U¿ytkownik powinien figurowaæ w bazie danych");
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
      Assert.AreEqual(LoginViewModel.RegisterMessage, "* Passwords are not identical", "Powinno wykryæ ró¿ne has³a");
    }
    [Test]
    public void LoginUsernameVerification()
    {
      var LoginViewModel = new LoginViewModel();

      LoginViewModel.Username = "adm3n";
      LoginViewModel.Password = new NetworkCredential("", "admin").SecurePassword;
      LoginViewModel.LoginCommand.Execute(null);
      Assert.AreEqual(LoginViewModel.ErrorMessage, "* Incorrect login or password", "Powinno wykryæ b³êdn¹ nazwê u¿ytkownika");
    }
    [Test]
    public void LoginPasswordVerification()
    {
      var LoginViewModel = new LoginViewModel();

      LoginViewModel.Username = "admin";
      LoginViewModel.Password = new NetworkCredential("", "admen").SecurePassword;
      LoginViewModel.LoginCommand.Execute(null);
      Assert.AreEqual(LoginViewModel.ErrorMessage, "* Incorrect login or password", "Powinno wykryæ b³êdne has³o");
    }
    [Test]
    public void TestTest()
    {
      Assert.AreEqual(true, true, "p");
    }
  }
}