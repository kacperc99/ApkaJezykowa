using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class TextWordbookModel
  {
    [BsonElement("_id")]
    public int? Id_Text_Wordbook {  get; set; }
    [BsonElement("Word")]
    public string Word { get; set; }
    [BsonElement("Translated_Word")]
    public string Translated_Word { get; set; }
    [BsonElement("Id_Compremension")]
    public int? Id_Comprehension {  get; set; }
    [BsonElement("Id_Reading_Text")]
    public int? Id_Reading_Text { get; set; }
  }
}
