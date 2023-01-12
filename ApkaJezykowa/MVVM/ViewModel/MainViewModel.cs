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
        private BaseViewModel _mainView;
        private UserAccountModel _currentUserAccount;
        string _welcomeMessage;
        bool _isViewVisible = true;

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
    public BaseViewModel MainView
    {
      get { return _mainView; }
      set
      {
        _mainView = value;
        OnPropertyChanged(nameof(MainView));

      }
    }
    public string WelcomeMessage { get { return _welcomeMessage; } set { _welcomeMessage = value; OnPropertyChanged(nameof(WelcomeMessage)); } }
    public bool IsViewVisible { get { return _isViewVisible; } set { _isViewVisible = value; OnPropertyChanged(nameof(IsViewVisible)); } }
    public ICommand UpdateViewCommand { get; set; }
    public ICommand UpdateMainViewCommand { get; set; }
        public MainViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            UpdateMainViewCommand = new UpdateMainViewCommand(this);
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            LoadCurrentUserData();
        }


    private void LoadCurrentUserData()
        {
            IsViewVisible = true;

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
          CurrentUserAccount.Username = UserModel.Instance.Username;
          CurrentUserAccount.DisplayName = $"Witaj, {UserModel.Instance.Username}";
          WelcomeMessage = $"Witaj, {UserModel.Instance.Username}";
          Console.WriteLine("Nie działa :(");

      }
    }
  }
}
