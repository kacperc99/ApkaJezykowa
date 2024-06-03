using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class ReadingListModel
  {
    [BsonElement("_id")]
    public int Id_Reading { get; set; }
    [BsonElement("Commprehension_Language")]
    public string Reading_Language { get; set; }
    [BsonElement("Commprehension_Title")]
    public string Reading_Title { get; set; }
    [BsonElement("Id_Vocabulary")]
    public int? Id_Vocabulary { get; set; }
  }
}
