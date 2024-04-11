using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class TextWordbookModel
  {
    public int? Id_Text_Wordbook {  get; set; }
    public string Word { get; set; }
    public string Translated_Word { get; set; }
    public int? Id_Comprehension {  get; set; }
    public int? Id_Reading_Text { get; set; }
  }
}
