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
    public int id;
    public string Lang;
    private BaseViewModel _selectedViewModel;

    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set { _selectedViewModel = value; OnPropertyChanged(nameof(SelectedViewModel)); }
    }
    public ICommand TestInfoUpdateViewCommand { get; set; }
    public TestInfoViewModel(string Lang, int id)
    {
      this.Lang = Lang;
      this.id = id;
      TestInfoUpdateViewCommand = new TestInfoUpdateViewCommand(this, Lang, id);
    }
  }
}
