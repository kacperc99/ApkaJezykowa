using ApkaJezykowa.Commands;
using ApkaJezykowa.Main;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.MVVM.ViewModel
{
  public class LoginViewModel : BaseViewModel
  {
    string _username;
    SecureString _password;
    string _errorMessage;
    public bool _isViewVisible = true;

    string _rUsername;
    SecureString _rPassword;
    string _email;
    SecureString _rPasswordRepeat;
    string _country;
    string _status;
    string _registerMessage;

    private IUserRepository userRepository;

    public string Username { get { return _username; } set { _username = value; OnPropertyChanged(nameof(Username)); } }
    public SecureString Password { get { return _password; } set { _password = value; OnPropertyChanged(nameof(Password)); } }
    public string ErrorMessage { get { return _errorMessage; } set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); } }
    public bool IsViewVisible { get { return _isViewVisible; } set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); } }

    public string RUsername { get { return _rUsername; } set { _rUsername = value; OnPropertyChanged(nameof(RUsername)); } }
    public SecureString RPassword { get { return _rPassword; } set { _rPassword = value; OnPropertyChanged(nameof(RPassword)); } }
    public string Email { get { return _email; } set { _email = value; OnPropertyChanged(nameof(Email)); } }
    public SecureString RPasswordRepeat { get { return _rPasswordRepeat; } set { _rPasswordRepeat = value; OnPropertyChanged(nameof(RPasswordRepeat)); } }
    public string Country { get { return _country; } set { _country = value; OnPropertyChanged(nameof(Country)); } }
    public string Status { get { return _status; } set { _status = "user"; OnPropertyChanged(nameof(Status)); } }
    public string RegisterMessage { get { return _registerMessage; } set { _registerMessage = value; OnPropertyChanged(nameof(RegisterMessage)); } }  
    public ICommand LoginCommand { get; }
    public ICommand RecoverPasswordCommand { get; }
    public ICommand ShowPasswordCommand { get; }
    public ICommand RememberPasswordCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand RegisterCommand { get; set; }

    private BaseViewModel _selectedViewModel;

    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set
      {
        _selectedViewModel = value;
        OnPropertyChanged(nameof(SelectedViewModel));

      }
    }

    public ICommand LoginUpdateViewCommand { get; set; }
    public LoginViewModel()
    {
      userRepository = new UserRepository();
      RegisterCommand = new RelayCommand(ExecuteRegisterCommand, CanExecuteRegisterCommand);
      LoginCommand = new RelayCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
      RecoverPasswordCommand = new RelayCommand(p => ExecuteRecoverPassCommand("", ""));
      LoginUpdateViewCommand = new LoginUpdateViewCommand(this);
    }

    private bool CanExecuteRegisterCommand(object obj)
    {
      bool validData;
      if (string.IsNullOrWhiteSpace(RUsername) || RPassword == null || string.IsNullOrWhiteSpace(Email) || RPasswordRepeat == null || string.IsNullOrWhiteSpace(Country))
        validData = false;
      else validData = true;
      return validData;
    }

    private void ExecuteRegisterCommand(object obj)
    {
      if(Email.Contains("@gmail.com") || Email.Contains("@wp.pl") || Email.Contains("@onet.pl") || Email.Contains("@vp.pl") || Email.Contains("@interia.pl"))
      { 
      var isNewUser = userRepository.FindUser(new System.Net.NetworkCredential(RUsername, Email));
      if (isNewUser)
      {
        RegisterMessage = "* Login lub email już istnieją w bazie danych";
      }
      else
      {
        string RPasswort = new NetworkCredential("", RPassword).Password;
        string RPasswortRepeat = new NetworkCredential("", RPasswordRepeat).Password;
        if (RPasswort==RPasswortRepeat)
       {
          Console.WriteLine(RPasswort);
          Console.WriteLine(RPasswortRepeat);
          Console.WriteLine("Pykło");
          userRepository.Add(RUsername,RPassword,Email,Country);
          RegisterMessage = "* Konto zostało założone!";
        }
        if(RPasswortRepeat!=RPasswort)
        {
          Console.WriteLine(RPasswort);
          Console.WriteLine(RPasswortRepeat);
          Console.WriteLine("A tu nie pykło");
          RegisterMessage = "* Hasła nie są identyczne";
          Console.WriteLine(value:RPasswort);
          Console.WriteLine(value:RPasswortRepeat);
        }
      }
      }
      else
      {
        RegisterMessage = "* nieprawidłowa nazwa Emaila";
      }
    }

    private bool CanExecuteLoginCommand(object obj)
    {
      bool validData;
      if (string.IsNullOrWhiteSpace(Username) || Password == null)
        validData = false;
      else validData = true;
      return validData;
    }
    private void ExecuteLoginCommand(object obj)
    {
      var isValidUser = userRepository.AuthenticateUser(new System.Net.NetworkCredential(Username, Password));
      if (isValidUser)
      {
        var user = userRepository.GetByUsername(Username);
        if (user != null)
        {
          UserModel.Instance.Username = user.Username;
        }
        VisibilityModel.Instance.IsViewVisibleLogin = false;
      }
      else
      {
        ErrorMessage = "* Błędny Login lub Hasło";
      }
    }


    private void ExecuteRecoverPassCommand(string username, string email)
    {

    }
  }
}
