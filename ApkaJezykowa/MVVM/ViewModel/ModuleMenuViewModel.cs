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
  public class ModuleMenuViewModel : BaseViewModel
  {

    private BaseViewModel _selectedViewModel;
    private IUser_CourseRepository user_CourseRepository;
    public bool _check;
    public byte[] _icon;
    public bool Check { get ; set; }
    public byte[] Icon { get { return _icon; } set { _icon = value; OnPropertyChanged(nameof(Icon)); } }
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
    public ModuleMenuViewModel(string Lang, byte[] Icon)
    {
      this.Icon = Icon;
      FrenchUpdateViewCommand = new FrenchUpdateViewCommand(this, Lang);
      user_CourseRepository = new User_CourseRepository();
      SetCourseLevel(Lang);
    }

    public void SetCourseLevel(string Lang)
    {
      AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.UnauthenticatedPrincipal);
      if (user_CourseRepository.IsUserSignedIn(Thread.CurrentPrincipal.Identity.Name, Lang) == false && Thread.CurrentPrincipal.Identity.Name != "")
      {
        AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.UnauthenticatedPrincipal);
        user_CourseRepository.Add(Thread.CurrentPrincipal.Identity.Name, Lang);
      }
      else
        Check = true;
    }
    
  }
}
