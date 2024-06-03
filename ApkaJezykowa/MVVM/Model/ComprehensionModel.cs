using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class ComprehensionModel
  {
      /// <summary>
      /// Gets or Sets Id
      /// </summary>
      [BsonElement("_id")]
      public int Id { get; set; }
      /// <summary>
      /// Gets or Sets CommprehensionLanguage
      /// </summary>
      [BsonElement("Commprehension_Language")]
      public string CommprehensionLanguage { get; set; }
      /// <summary>
      /// Gets or Sets CommprehensionTitle
      /// </summary>
      [BsonElement("Commprehension_Title")]
      public string CommprehensionTitle { get; set; }
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
        sb.Append("class ComprehensionEntity {\n");
        sb.Append("  Id: ").Append(Id).Append("\n");
        sb.Append("  CommprehensionLanguage: ").Append(CommprehensionLanguage).Append("\n");
        sb.Append("  CommprehensionTitle: ").Append(CommprehensionTitle).Append("\n");
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
        return this.ToJson<ComprehensionModel>();
      }*/
    }
}
