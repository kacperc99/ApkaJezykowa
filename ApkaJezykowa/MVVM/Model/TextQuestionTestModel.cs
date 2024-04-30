using ApkaJezykowa.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class TextQuestionTestModel : BaseViewModel
  {
    public int Id_Text_Question { get; set; }
    public string Question { get; set; }
    public string Answer1 { get; set; }
    public string Answer2 { get; set; }
    public string Answer3 { get; set; }
    public string Answer4 { get; set; }
    public string _answer_Tip;
    public string Answer_Tip { get { return _answer_Tip; } set { _answer_Tip = value; OnPropertyChanged(nameof(Answer_Tip)); } }
    public int Id_Reading_Text { get; set; }
    public string GroupName { get; set; }
  }
  /*public class QuestionAnswers
  {
    public string Correct_Answer { get; set; }
    public string Wrong_Answer { get; set; }
    public string Wrong_Answer_2 { get; set; }
    public string Wrong_Answer_3 { get; set; }
  }*/
}
