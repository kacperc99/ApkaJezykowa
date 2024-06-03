using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApkaJezykowa.MVVM.Model
{
  public class LessonImageModel
  {
    [BsonElement("_id")]
    public int Id { get; set; }
    [BsonElement("Id_Lesson_Content")]
    public int IdLessonContent { get; set; }
    [BsonElement("Lesson_Image")]
    public byte[] Image { get; set; }
    [BsonElement("Image_Desc")]
    public string Description { get; set; }
  }
}
