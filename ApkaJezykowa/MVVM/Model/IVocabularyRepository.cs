using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public interface IVocabularyRepository
  {
    void ObtainVocabList(ObservableCollection<VocabularyListModel> VocabularyList, string Country, string Language);
  }
}
