using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class ListeningListModel
  {
    [BsonElement("_id")]
    public int Id_Listening {  get; set; }
    [BsonElement("Listening_Language")]
    public string Listening_Language {  get; set; }
    [BsonElement("Listening_Title")]
    public string Listening_Title { get; set; }
    [BsonElement("Id_Vocabulary")]
    public int? Id_Vocabulary { get; set; }
  }
}
