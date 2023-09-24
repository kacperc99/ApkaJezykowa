using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApkaJezykowa.Main;
using System.Windows.Input;
using ApkaJezykowa.Commands;
using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;

namespace ApkaJezykowa.MVVM.ViewModel
{
  class LessonsViewModel : BaseViewModel
  {
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
    public ICommand LessonUpdateViewCommand { get; set; }
    public LessonsViewModel()
    {
      
      LessonUpdateViewCommand = new LessonUpdateViewComand(this);
      
    }
    
  }
}
