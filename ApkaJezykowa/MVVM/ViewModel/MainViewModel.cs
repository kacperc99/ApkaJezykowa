using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApkaJezykowa.Main;
using System.Windows.Input;
using ApkaJezykowa.Commands;
using ApkaJezykowa.MVVM.Model;
using System.Threading;
using ApkaJezykowa.Repositories;

namespace ApkaJezykowa.MVVM.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;
        private UserAccountModel _currentUserAccount;
        string _welcomeMessage;
        //bool _isViewVisible = true;

    private IUserRepository userRepository;

        public UserAccountModel CurrentUserAccount
        {
            get { return _currentUserAccount; }
            set { _currentUserAccount = value; OnPropertyChanged(nameof(CurrentUserAccount)); }
        }

        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set { _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));

            }
        }
    public string WelcomeMessage { get { return _welcomeMessage; } set { _welcomeMessage = value; OnPropertyChanged(nameof(WelcomeMessage)); } }
    //public bool IsViewVisible { get { return _isViewVisible; } set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); } }
    public ICommand UpdateViewCommand { get; set; }
       public ICommand LogoutCommand { get; }
        public MainViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentUserData();
      LogoutCommand = new RelayCommand(ExecuteLogoutCommand, CanExecuteLogoutCommand);
        }

    private bool CanExecuteLogoutCommand(object arg)
    {
      return true;
    }

    private void ExecuteLogoutCommand(object obj)
    {
      //IsViewVisible = false;
    }

    private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if(user!=null)
            {
                  CurrentUserAccount.Username = user.Username;
                  CurrentUserAccount.DisplayName = $"Witaj, {user.Username}";
        WelcomeMessage = $"Witaj, {user.Username}";
        Console.WriteLine("Działa?");
      }
            else
            {
        Console.WriteLine("Nie działa :(");
      }
        }
  }
}
