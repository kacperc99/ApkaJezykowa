using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class TextQuestionModel
  {
    public int Id_Text_Question { get; set; }
    public string Question { get; set; }
    public string Correct_Answer { get; set; }
    public string Wrong_Answer { get; set; }
    public string Wrong_Answer_2 { get; set; }
    public string Wrong_Answer_3 { get; set; }
    public string Answer_Tip { get; set; }
    public int Id_Reading_Text { get; set; }
  }
}
