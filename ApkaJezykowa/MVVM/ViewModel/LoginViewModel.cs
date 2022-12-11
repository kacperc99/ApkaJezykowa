using ApkaJezykowa.Commands;
using ApkaJezykowa.Main;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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

    /*string _username;
    SecureString _password;
    string _errorMessage;
    bool _isViewVisible = true;

    private IUserRepository userRepository;

    public string Username { get { return _username; } set { _username = value; OnPropertyChanged(nameof(Username)); } }
    public SecureString Password { get { return _password; } set { _password = value; OnPropertyChanged(nameof(Password)); } }
    public string ErrorMessage { get { return _errorMessage; } set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); } }
    public bool IsViewVisible { get { return _isViewVisible; } set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); } }
    public ICommand LoginCommand { get; }
    public ICommand RecoverPasswordCommand { get; }
    public ICommand ShowPasswordCommand { get; }
    public ICommand RememberPasswordCommand { get; }
    public ICommand LogoutCommand { get; }
    public LoginViewModel()
    {
      userRepository = new UserRepository();
      LoginCommand = new RelayCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
      RecoverPasswordCommand = new RelayCommand(p => ExecuteRecoverPassCommand("", ""));
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
      if(isValidUser)
      {
        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
        IsViewVisible = false;
        //LoginUpdateViewCommand = LoginUpdateViewCommand(this);
      }
      else
      {
        ErrorMessage = "* Błędny Login lub Hasło";
      }
    }
    private void ExecuteRecoverPassCommand(string username, string email)
    {

    }*/
    string _username;
    SecureString _password;
    string _errorMessage;
    bool _isViewVisible = true;

    private IUserRepository userRepository;

    public string Username { get { return _username; } set { _username = value; OnPropertyChanged(nameof(Username)); } }
    public SecureString Password { get { return _password; } set { _password = value; OnPropertyChanged(nameof(Password)); } }
    public string ErrorMessage { get { return _errorMessage; } set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); } }
    public bool IsViewVisible { get { return _isViewVisible; } set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); } }
    public ICommand LoginCommand { get; }
    public ICommand RecoverPasswordCommand { get; }
    public ICommand ShowPasswordCommand { get; }
    public ICommand RememberPasswordCommand { get; }
    public ICommand LogoutCommand { get; }

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
      LoginCommand = new RelayCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
      RecoverPasswordCommand = new RelayCommand(p => ExecuteRecoverPassCommand("", ""));
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
        Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);
        IsViewVisible = false;
        LoginUpdateViewCommand = new LoginUpdateViewCommand(this);
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
