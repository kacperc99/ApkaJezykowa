using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class ReadingListModel
  {
    public int Id_Reading { get; set; }
    public string Reading_Language { get; set; }
    public string Reading_Title { get; set; }
    public int? Id_Vocabulary { get; set; }
  }
}
