using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.ViewModel
{
  public class InfoViewModel : BaseViewModel
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
    public void LoadCurrentUserData()
    {
      AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.UnauthenticatedPrincipal);
      var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
      if (user != null)
      {
        CurrentUserAccount.Username = $"Nazwa użytkownika: {user.Username}";
        CurrentUserAccount.Email = $"E-Mail: {user.Email}";
        CurrentUserAccount.Country = $"Kraj: {user.Country}";
        Console.WriteLine("Działa?");
      }
      else
      {
        Console.WriteLine("Nie działa :(");
      }
    }
  }
}

