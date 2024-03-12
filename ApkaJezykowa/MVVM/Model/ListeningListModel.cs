using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class ListeningListModel
  {
    public int Id_Listening {  get; set; }
    public string Listening_Language {  get; set; }
    public string Listening_Title { get; set; }
    public int? Id_Vocabulary { get; set; }
  }
}
