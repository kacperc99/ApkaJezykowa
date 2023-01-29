using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ApkaJezykowa.Commands;
using ApkaJezykowa.Main;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;

namespace ApkaJezykowa.MVVM.ViewModel
{
  public class EnglishViewModel : BaseViewModel
  {

    private BaseViewModel _selectedViewModel;
    private IUser_CourseRepository user_CourseRepository;
    public bool _check;

    public bool Check { get; set; }

    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set
      {
        _selectedViewModel = value;
        OnPropertyChanged(nameof(SelectedViewModel));

      }
    }
    public ICommand EnglishUpdateViewCommand { get; set; }
    public EnglishViewModel()
    {
      EnglishUpdateViewCommand = new EnglishUpdateViewCommand(this);
      user_CourseRepository = new User_CourseRepository();
      SetCourseLevel();
    }

    private void SetCourseLevel()
    {
      AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.UnauthenticatedPrincipal);
      if (user_CourseRepository.IsUserSignedIn(Thread.CurrentPrincipal.Identity.Name, "Angielski") == false && Thread.CurrentPrincipal.Identity.Name != "")
      {
        AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.UnauthenticatedPrincipal);
        user_CourseRepository.Add(Thread.CurrentPrincipal.Identity.Name, "Angielski");
      }
      else
        Check = true;
    }
  }
}
