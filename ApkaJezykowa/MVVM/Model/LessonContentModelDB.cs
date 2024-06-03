using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  internal class LessonContentModelDB
  {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [BsonElement("_id")]
    public int Id { get; set; }
    /// <summary>
    /// Gets or Sets LessonText
    /// </summary>
    [BsonElement("Lesson_Text")]
    public string LessonText { get; set; }
    /// <summary>
    /// Gets or Sets IdLessonTitle
    /// </summary>
    [BsonElement("Id_Lesson_Title")]
    public int IdLessonTitle { get; set; }
    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    /*public override string ToString()
    {
      var sb = new StringBuilder();
      sb.Append("class LessonContentEntity {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  LessonText: ").Append(LessonText).Append("\n");
      sb.Append("  IdLessonTitle: ").Append(IdLessonTitle).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }
    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson()
    {
      return this.ToJson<LessonContentModel>();
    }*/
  }
}
