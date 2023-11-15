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
using System.Collections.ObjectModel;

namespace ApkaJezykowa.MVVM.ViewModel
{
  public class Clicker : BaseViewModel
  {
    public string _language;
    public byte[] _icon;

    public string Language {
      get { return _language; }
      set { _language = value; OnPropertyChanged(nameof(Language)); }
    }
    public byte[] Icon { 
      get { return _icon; } 
      set { _icon = value; OnPropertyChanged(nameof(Icon)); } 
    }
  }
  public class LessonsViewModel : BaseViewModel
  {
    public ObservableCollection<Clicker> buttons = new ObservableCollection<Clicker>();
    private BaseViewModel _selectedViewModel;
    private ILessonRepository lessonRepository;
    public ObservableCollection<Clicker> Buttons { 
      get { return buttons; } 
      set { buttons = value; OnPropertyChanged(nameof(Buttons)); } 
    }
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
      lessonRepository = new LessonRepository();
      LessonUpdateViewCommand = new LessonUpdateViewComand(this);
      LoadButtons();
    }
    public void LoadButtons()
    {
      lessonRepository.GetButtons(Buttons);
    }
    
  }
}
