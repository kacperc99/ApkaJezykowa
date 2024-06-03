using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  internal class ExerciseModel
  {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [BsonElement("_id")]
    public int Id { get; set; }
    /// <summary>
    /// Gets or Sets ExerciseLanguage
    /// </summary>
    [BsonElement("Exercise_Language")]
    public string ExerciseLanguage { get; set; }
    /// <summary>
    /// Gets or Sets ExerciseLevel
    /// </summary>
    [BsonElement("Exercise_Level")]
    public int ExerciseLevel { get; set; }
    /// <summary>
    /// Gets or Sets ExerciseTitle
    /// </summary>
    [BsonElement("Exercise_Title")]
    public string ExerciseTitle { get; set; }
    /// <summary>
    /// Gets or Sets TaskText
    /// </summary>
    [BsonElement("Task_text")]
    public string TaskText { get; set; }
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
      sb.Append("class ExerciseEntity {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  ExerciseLanguage: ").Append(ExerciseLanguage).Append("\n");
      sb.Append("  ExerciseLevel: ").Append(ExerciseLevel).Append("\n");
      sb.Append("  ExerciseTitle: ").Append(ExerciseTitle).Append("\n");
      sb.Append("  TaskText: ").Append(TaskText).Append("\n");
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
      return this.ToJson<ExerciseModel>();
    }*/
  }
}
