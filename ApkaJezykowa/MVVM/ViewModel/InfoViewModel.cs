using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.ViewModel
{
  internal class InfoViewModel : BaseViewModel
  {
    private UserAccountModel _currentUserAccount;

    private IUserRepository userRepository;

    public UserAccountModel CurrentUserAccount
    {
      get { return _currentUserAccount; }
      set { _currentUserAccount = value; OnPropertyChanged(nameof(CurrentUserAccount)); }
    }
    public InfoViewModel()
    {
      userRepository = new UserRepository();
      CurrentUserAccount = new UserAccountModel();
      LoadCurrentUserData();
    }
    private void LoadCurrentUserData()
    {
      if (UserModel.Instance.Username != null)
      {
        CurrentUserAccount.Username = $"Nazwa użytkownika: {UserModel.Instance.Username}";
        CurrentUserAccount.Email = $"E-Mail: {UserModel.Instance.Email}";
        CurrentUserAccount.Country = $"Kraj: {UserModel.Instance.Country}";
        Console.WriteLine("Działa?");
      }
      else
      {
        Console.WriteLine("Nie działa :(");
      }
    }
  }
}

