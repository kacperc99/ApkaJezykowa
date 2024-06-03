using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class ChooseRightPhraseModel
  {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [BsonElement("_id")]
    public int Id { get; set; }
    /// <summary>
    /// Gets or Sets SituationDescription
    /// </summary>
    [BsonElement("Situation_Description")]
    public string SituationDescription { get; set; }
    /// <summary>
    /// Gets or Sets TTSPhrase
    /// </summary>
    [BsonElement("TTS_Phrase")]
    public string TTSPhrase { get; set; }
    /// <summary>
    /// Gets or Sets Answer
    /// </summary>
    [BsonElement("Answer")]
    public string Answer { get; set; }
    /// <summary>
    /// Gets or Sets IdListening
    /// </summary>
    [BsonElement("Id_Listening")]
    public int IdListening { get; set; }
    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()
    {
      var sb = new StringBuilder();
      sb.Append("class ChooseRightPhraseEntity {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  SituationDescription: ").Append(SituationDescription).Append("\n");
      sb.Append("  TTSPhrase: ").Append(TTSPhrase).Append("\n");
      sb.Append("  Answer: ").Append(Answer).Append("\n");
      sb.Append("  IdListening: ").Append(IdListening).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }
    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson()
    {
      return this.ToJson<ChooseRightPhraseModel>();
    }
  }
}
