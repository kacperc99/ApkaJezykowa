using ApkaJezykowa.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.MVVM.ViewModel
{
  internal class VocabularyTestModeViewModel : BaseViewModel
  {
    public int id;
    public string Lang;
    private BaseViewModel _selectedViewModel;

    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set { _selectedViewModel = value; OnPropertyChanged(nameof(SelectedViewModel)); }
    }
    public ICommand VocabularyTestModeUpdateViewCommand { get; set; }
    public VocabularyTestModeViewModel(string Lang, int id)
    {
      this.Lang = Lang;
      this.id = id;
      VocabularyTestModeUpdateViewCommand = new VocabularyTestModeUpdateViewCommand(this);
    }
  }
}
