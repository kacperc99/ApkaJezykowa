using ApkaJezykowa.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.MVVM.ViewModel
{
  internal class LessonEditorViewModel : BaseViewModel
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
    public ICommand LessonEditorUpdateViewCommand { get; set; }
    public LessonEditorViewModel()
    {
      LessonEditorUpdateViewCommand = new LessonEditorUpdateViewCommand(this);
    }
  }
}
