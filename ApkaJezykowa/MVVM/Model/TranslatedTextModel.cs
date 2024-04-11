using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class TranslatedTextModel
  {
    public int? Id_Translation { get; set; }
    public string Translation { get; set; }
    public int? Id_Comprehension { get; set; }
    public int? Id_Reading_Text { get; set; }
  }
}
