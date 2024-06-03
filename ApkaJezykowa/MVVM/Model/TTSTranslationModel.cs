using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class TTSTranslationModel
  {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [BsonElement("_id")]
    public int Id { get; set; }
    /// <summary>
    /// Gets or Sets PhraseTranslated
    /// </summary>
    [BsonElement("Phrase_Translated")]
    public string PhraseTranslated { get; set; }
    /// <summary>
    /// Gets or Sets IdListening
    /// </summary>
    [BsonElement("Id_Listening")]
    public int IdListening { get; set; }
    /// <summary>
    /// Gets or Sets IdTTSPhrase
    /// </summary>
    [BsonElement("Id_TTS_Phrase")]
    public int IdTTSPhrase { get; set; }
    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    /*public override string ToString()
    {
      var sb = new StringBuilder();
      sb.Append("class TTSTranslationEntity {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  PhraseTranslated: ").Append(PhraseTranslated).Append("\n");
      sb.Append("  IdListening: ").Append(IdListening).Append("\n");
      sb.Append("  IdTTSPhrase: ").Append(IdTTSPhrase).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }
    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson()
    {
      return this.ToJson<TTSTranslationModel>();
    }*/
  }
}
