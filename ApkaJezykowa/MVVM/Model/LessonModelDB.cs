using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class LessonModelDB
  {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [BsonElement("_id")]
    public int Id { get; set; }
    /// <summary>
    /// Gets or Sets LessonLevel
    /// </summary>
    [BsonElement("Lesson_Level")]
    public int LessonLevel { get; set; }
    /// <summary>
    /// Gets or Sets IdCourse
    /// </summary>
    [BsonElement("Id_Course")]
    public int IdCourse { get; set; }
    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    /*public override string ToString()
    {
      var sb = new StringBuilder();
      sb.Append("class LessonEntity {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  LessonLevel: ").Append(LessonLevel).Append("\n");
      sb.Append("  IdCourse: ").Append(IdCourse).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }
    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson()
    {
      return this.ToJson<LessonModelDB>();
    }*/
  }
}
