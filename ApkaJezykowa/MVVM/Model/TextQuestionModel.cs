using MongoDB.Bson.Serialization.Attributes;
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
    [BsonElement("_id")]
    public int Id_Text_Question { get; set; }
    [BsonElement("Question")]
    public string Question { get; set; }
    [BsonElement("Correct_Answer")]
    public string Correct_Answer { get; set; }
    [BsonElement("Wrong_Answer")]
    public string Wrong_Answer { get; set; }
    [BsonElement("Wrong_Answer_2")]
    public string Wrong_Answer_2 { get; set; }
    [BsonElement("Wrong_Answer_3")]
    public string Wrong_Answer_3 { get; set; }
    [BsonElement("Answer_Tip")]
    public string Answer_Tip { get; set; }
    [BsonElement("Id_Reading_Text")]
    public int Id_Reading_Text { get; set; }
  }
}
