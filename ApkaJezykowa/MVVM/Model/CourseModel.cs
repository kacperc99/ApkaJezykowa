using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  [BsonIgnoreExtraElements]
  public class CourseModel
  {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [BsonElement("_id")]
    public int Id { get; set; }
    /// <summary>
    /// Gets or Sets CourseName
    /// </summary>
    [BsonElement("Course_Name")]
    public string CourseName { get; set; }
    /// <summary>
    /// Gets or Sets Image
    /// </summary>
    [BsonElement("Image")]
    public byte[] Image { get; set; }
    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    /*public override string ToString()
    {
      var sb = new StringBuilder();
      sb.Append("class CourseEntity {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CourseName: ").Append(CourseName).Append("\n");
      sb.Append("  Image: ").Append(Image).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }
    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson()
    {
      return this.ToJson<CourseModel>();
    }*/
  }
}
