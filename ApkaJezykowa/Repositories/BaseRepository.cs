using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ApkaJezykowa.Keys;

namespace ApkaJezykowa.Repositories
{
  public abstract class BaseRepository
  {
    private readonly string _userConnectionString;
    private readonly string _courseConnectionString;
    public BaseRepository()
    {
      //_userConnectionString = @"Server=DESKTOP-LFET3LQ\LINGUONATOR2; Database=UserBase; MultipleActiveResultSets=true; Integrated Security=true";
      //_courseConnectionString = @"Server=DESKTOP-LFET3LQ\LINGUONATOR2; Database=CourseBase; MultipleActiveResultSets=true; Integrated Security=true";
      //_userConnectionString = @"Server=192.168.50.116,49170; Initial Catalog=UserBase; User ID=app; Password=app; MultipleActiveResultSets=true";
      //_courseConnectionString = @"Server=192.168.50.116,49170; Initial Catalog=CourseBase; User ID=app; Password=app; MultipleActiveResultSets=true";
      _userConnectionString = SpeechServiceKey.Instance.UserBaseConnection;
      _courseConnectionString = SpeechServiceKey.Instance.CourseBaseConnection;
    }
    protected SqlConnection GetUserConnection()
    {
      return new SqlConnection(_userConnectionString);
    }
    protected SqlConnection GetCourseConnection()
    {
      return new SqlConnection(_courseConnectionString);
    }
  }
}
