using ApkaJezykowa.Commands;
using ApkaJezykowa.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.MVVM.ViewModel
{
  internal class TestInfoViewModel : BaseViewModel
  {
    private BaseViewModel _selectedViewModel;

    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set { _selectedViewModel = value; OnPropertyChanged(nameof(SelectedViewModel)); }
    }
    public ICommand TestInfoUpdateViewCommand { get; set; }
    public TestInfoViewModel()
    {
      TestInfoUpdateViewCommand = new TestInfoUpdateViewCommand(this);
    }
  }
}
