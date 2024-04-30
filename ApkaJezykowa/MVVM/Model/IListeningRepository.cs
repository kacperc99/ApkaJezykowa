using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public interface IListeningRepository
  {
    ObservableCollection<TTS> GetPhrases(int id, string Language);
    void GetAnswers(ObservableCollection<TaskTemplate> data, int id, string Lang);
    ObservableCollection<TTS> GetTestPhrases(int id, string Language);
    void GetTestAnswers(ObservableCollection<TaskTemplate> data, int id, string Lang);
  }
}
