using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class ExerciseContentModel
  {
    [BsonElement("_id")]
    public int Id { get; set; }
    [BsonElement("Task")]
    public string Task { get; set; }
    [BsonElement("Answer")]
    public string Answer { get; set; }
    [BsonElement("Answer2")]
    public string Answer2 { get; set; }
    [BsonElement("Answer3")]
    public string Answer3 { get; set; }
    [BsonElement("Tip")]
    public string Tip { get; set; }
    [BsonElement("Id_Exercise")]
    public int Id_Exercise { get; set; }
  }
}
