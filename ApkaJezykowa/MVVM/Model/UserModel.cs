using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Windows.Security.Authentication.OnlineId;

namespace ApkaJezykowa.MVVM.Model
{
  public class UserModel
  {
    [BsonElement("_id"), BsonRepresentation(BsonType.Int32)]
    public int Id { get; set; }
    [BsonElement("Username"), BsonRepresentation(BsonType.String)]
    public string Username { get; set; }
    [BsonElement("Password"), BsonRepresentation(BsonType.String)]
    public string Password { get; set; }
    [BsonElement("Email"), BsonRepresentation(BsonType.String)]
    public string Email { get; set; }
    [BsonElement("Country"), BsonRepresentation(BsonType.String)]
    public string Country { get; set; }
    public UserModel() { }
    public static readonly UserModel Instance = new UserModel();

    /*public override string ToString()
    {
      var sb = new StringBuilder();
      sb.Append("class UserEntity {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Username: ").Append(Username).Append("\n");
      sb.Append("  Password: ").Append(Password).Append("\n");
      sb.Append("  Email: ").Append(Email).Append("\n");
      sb.Append("  Country: ").Append(Country).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }
    public string ToJson()
    {
      return this.ToJson<UserModel>();
    }*/
  }
}
