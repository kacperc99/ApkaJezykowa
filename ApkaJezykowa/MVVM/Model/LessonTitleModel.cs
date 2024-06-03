using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class LessonTitleModel
  {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [BsonElement("_id")]
    public int Id { get; set; }
    /// <summary>
    /// Gets or Sets LessonLanguage
    /// </summary>
    [BsonElement("Lesson_Language")]
    public string LessonLanguage { get; set; }
    /// <summary>
    /// Gets or Sets LessonTitle
    /// </summary>
    [BsonElement("Lesson_Title")]
    public string LessonTitle { get; set; }
    /// <summary>
    /// Gets or Sets IdLesson
    /// </summary>
    [BsonElement("Id_Lesson")]
    public int IdLesson { get; set; }
    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    /*public override string ToString()
    {
      var sb = new StringBuilder();
      sb.Append("class LessonTitleEntity {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  LessonLanguage: ").Append(LessonLanguage).Append("\n");
      sb.Append("  LessonTitle: ").Append(LessonTitle).Append("\n");
      sb.Append("  IdLesson: ").Append(IdLesson).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }
    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson()
    {
      return this.ToJson<LessonTitleModel>();
    }*/
  }
}
