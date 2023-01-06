using System;
using System.Collections.Generic;
using System.Linq;
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
  internal class EnglishViewModel : BaseViewModel
  {

    private BaseViewModel _selectedViewModel;
    private IUser_CourseRepository user_CourseRepository;

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
      if (user_CourseRepository.IsUserSignedIn(Thread.CurrentPrincipal.Identity.Name, "Angielski") == false)
        user_CourseRepository.Add(Thread.CurrentPrincipal.Identity.Name, "Angielski");
    }
  }
}
