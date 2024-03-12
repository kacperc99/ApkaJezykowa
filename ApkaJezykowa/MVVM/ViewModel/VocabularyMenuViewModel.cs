using ApkaJezykowa.MVVM.Model;
using ApkaJezykowa.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ApkaJezykowa.MVVM.ViewModel
{
  public class VocabularyMenuViewModel : BaseViewModel
  {
    public ObservableCollection<VocabularyListModel> _vocabularyList = new ObservableCollection<VocabularyListModel>();
    private BaseViewModel _selectedViewModel;
    private IVocabularyRepository vocabularyRepository;
    public ObservableCollection<VocabularyListModel> VocabularyList { get { return _vocabularyList; } set { _vocabularyList = value; OnPropertyChanged(nameof(VocabularyList)); } }
    public BaseViewModel SelectedViewModel
    {
      get { return _selectedViewModel; }
      set
      {
        _selectedViewModel = value;
        OnPropertyChanged(nameof(SelectedViewModel));
      }
    }
    public ICommand VocabularyMenuUpdateViewCommand { get; set; }

    public VocabularyMenuViewModel(string Lang) 
    {
      vocabularyRepository = new VocabularyRepository();
      vocabularyRepository.ObtainVocabList(VocabularyList, Lang, Properties.Settings.Default.Language);
      //VocabularyMenuUpdateViewCommand = new VocabularyMenuUpdateViewCommand(this);
    }
  }
}
