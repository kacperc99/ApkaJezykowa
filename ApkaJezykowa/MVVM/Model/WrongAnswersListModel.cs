using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class WrongAnswersListModel
  {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [BsonElement("_id")]
    public int Id { get; set; }
    /// <summary>
    /// Gets or Sets WrongAnswer
    /// </summary>
    [BsonElement("Wrong_Answer")]
    public string WrongAnswer { get; set; }
    /// <summary>
    /// Gets or Sets IdChooseRightPhrase
    /// </summary>
    [BsonElement("Id_Choose_Right_Phrase")]
    public int IdChooseRightPhrase { get; set; }
    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    /*public override string ToString()
    {
      var sb = new StringBuilder();
      sb.Append("class WrongAnswersListEntity {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  WrongAnswer: ").Append(WrongAnswer).Append("\n");
      sb.Append("  IdChooseRightPhrase: ").Append(IdChooseRightPhrase).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }
    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson()
    {
      return this.ToJson<WrongAnswersListModel>();
    }*/
  }
}
