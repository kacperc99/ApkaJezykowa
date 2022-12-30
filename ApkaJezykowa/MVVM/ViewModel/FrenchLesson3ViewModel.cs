using ApkaJezykowa.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.MVVM.ViewModel
{
  internal class FrenchLesson3ViewModel : BaseViewModel
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
    public ICommand FrenchLesson3UpdateViewCommand { get; set; }
    public FrenchLesson3ViewModel()
    {
      FrenchLesson3UpdateViewCommand = new FrenchLesson3UpdateViewCommand(this);
    }
  }
}
