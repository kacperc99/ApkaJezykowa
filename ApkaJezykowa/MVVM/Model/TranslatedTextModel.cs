using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class TranslatedTextModel
  {
    [BsonElement("_id")]
    public int? Id_Translation { get; set; }
    [BsonElement("Translation")]
    public string Translation { get; set; }
    [BsonElement("Id_Comprehension")]
    public int? Id_Comprehension { get; set; }
    [BsonElement("Id_Reading_Text")]
    public int? Id_Reading_Text { get; set; }
  }
}
