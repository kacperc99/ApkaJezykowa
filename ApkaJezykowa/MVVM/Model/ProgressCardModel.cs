using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  internal class ProgressCardModel
  {

      /// <summary>
      /// Gets or Sets Id
      /// </summary>
      [BsonElement("_id")]
      public int Id { get; set; }
      /// <summary>
      /// Gets or Sets LangCourse
      /// </summary>
      [BsonElement("Lang_Course")]
      public string LangCourse { get; set; }
      /// <summary>
      /// Gets or Sets Lang
      /// </summary>
      [BsonElement("Lang")]
      public string Lang { get; set; }
      /// <summary>
      /// Gets or Sets ExerciseUserLevel
      /// </summary>
      [BsonElement("Exercise_User_Level")]
      public decimal ExerciseUserLevel { get; set; }
      /// <summary>
      /// Gets or Sets ListeningUserLevel
      /// </summary>
      [BsonElement("Listening_User_Level")]
      public decimal ListeningUserLevel { get; set; }
      /// <summary>
      /// Gets or Sets TextUserLevel
      /// </summary>
      [BsonElement("Text_User_Level")]
      public decimal TextUserLevel { get; set; }
      /// <summary>
      /// Gets or Sets IdUser
      /// </summary>
      [BsonElement("Id_User")]
      public int IdUser { get; set; }
  }
}
