using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  internal class VocabularyModel
  {
    [BsonElement("_id")]
    public int Id { get; set; }
    /// <summary>
    /// Gets or Sets VocabularyLevel
    /// </summary>
    [BsonElement("Vocabulary_Level")]
    public decimal VocabularyLevel { get; set; }
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
      sb.Append("class VocabularyEntity {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  VocabularyLevel: ").Append(VocabularyLevel).Append("\n");
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
      return this.ToJson<VocabularyModel>();
    }*/
  }
}
