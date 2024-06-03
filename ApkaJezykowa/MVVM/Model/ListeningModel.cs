using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class ListeningModel
  {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [BsonElement("_id")]
    public int Id { get; set; }
    /// <summary>
    /// Gets or Sets ListeningLanguage
    /// </summary>
    [BsonElement("Listening_Language")]
    public string ListeningLanguage { get; set; }
    /// <summary>
    /// Gets or Sets ListeningTitle
    /// </summary>
    [BsonElement("Listening_Title")]
    public string ListeningTitle { get; set; }
    /// <summary>
    /// Gets or Sets IdVocabulary
    /// </summary>
    [BsonElement("Id_Vocabulary")]
    public int IdVocabulary { get; set; }
    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    /*public override string ToString()
    {
      var sb = new StringBuilder();
      sb.Append("class ListeningEntity {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  ListeningLanguage: ").Append(ListeningLanguage).Append("\n");
      sb.Append("  ListeningTitle: ").Append(ListeningTitle).Append("\n");
      sb.Append("  IdVocabulary: ").Append(IdVocabulary).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }
    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson()
    {
      return this.ToJson<ListeningModel>();
    }*/
  }
}
