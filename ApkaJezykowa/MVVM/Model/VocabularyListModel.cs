using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class VocabularyListModel
  {
    public int Id_Vocabulary {  get; set; }
    public string Vocabulary_Parameter {  get; set; }
    public decimal Vocabulary_Level {  get; set; }
    public int? Id_Course { get; set; }
    public ObservableCollection<ListeningListModel> ListeningList { get; set; }
    public ObservableCollection<ReadingListModel> ReadingList { get; set; }
  }
}
