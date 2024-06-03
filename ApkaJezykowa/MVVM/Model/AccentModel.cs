using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class AccentModel
  {
    [BsonElement("_id")]
    public int Id_Accent { get; set; }
    [BsonElement("Accent")]
    public string Accent { get; set; }
    [BsonElement("Lang")]
    public string Lang { get; set; }
    [BsonElement("Voice")]
    public string Voice { get; set; }
    [BsonElement("Id_Course")]
    public int Id_Course { get; set; }
  }
}
