using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ApkaJezykowa.Repositories
{
  public abstract class BaseRepository
  {
    private readonly string _connectionString;
    public BaseRepository()
    {
            //_connectionString = @"Server=DESKTOP-TJ02NPR\LINGUONATOR2; Database=userDB; Integrated Security=true";
            //_connectionString = @"Server=tcp:linguonator2.database.windows.net,1433;Initial Catalog=LINGUONATOR2;Persist Security Info=False;User ID=kc99;Password=EpIcPaSsWoRd!9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
            _connectionString = @"Server=tcp:192.168.50.116,1433,1433; Network Library=DBMSSOCN; Initial Catalog=userDB; User ID=app; Password=app;";
      //_connectionString = @"Server=DESKTOP-LFET3LQ\LINGUONATOR2; Database=userDB; Integrated Security=true";
    }
    protected SqlConnection GetConnection()
    {
      return new SqlConnection(_connectionString);
    }
  }
}
