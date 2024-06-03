using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class ReadingTextModel
  {
    [BsonElement("_id")]
    public int Id_Reading_Text { get; set; }
    [BsonElement("Only_Test_Mode")]
    public bool Only_Test_Mode { get; set; }
    [BsonElement("Text_Title")]
    public string Text_Title { get; set; }
    [BsonElement("TTS_Text")]
    public string TTS_Text { get; set; }
    [BsonElement("Illustration")]
    public byte[] Illustration { get; set; }
  }
}
