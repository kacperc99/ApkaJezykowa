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
  public class FrenchViewModel : BaseViewModel
  {

    private BaseViewModel _selectedViewModel;
    private IUser_CourseRepository user_CourseRepository;
    public bool _check;

    public bool Check { get ; set; }
    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set
      {
        _selectedViewModel = value;
        OnPropertyChanged(nameof(SelectedViewModel));

      }
    }
    public ICommand FrenchUpdateViewCommand { get; set; }
    public FrenchViewModel()
    {
      FrenchUpdateViewCommand = new FrenchUpdateViewCommand(this);
      user_CourseRepository = new User_CourseRepository();
      SetCourseLevel();
    }

    public void SetCourseLevel()
    {
      AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.UnauthenticatedPrincipal);
      if (user_CourseRepository.IsUserSignedIn(Thread.CurrentPrincipal.Identity.Name, "Francuski") == false && Thread.CurrentPrincipal.Identity.Name != "")
      {

        AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.UnauthenticatedPrincipal);
        user_CourseRepository.Add(Thread.CurrentPrincipal.Identity.Name, "Francuski");
      }
      else
        Check = true;
    }
    
  }
}
