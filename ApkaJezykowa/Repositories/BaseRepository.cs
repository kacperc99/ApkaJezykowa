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
      _connectionString = @"Server=tcp:linguonator2.database.windows.net,1433;Initial Catalog=LINGUONATOR2;Persist Security Info=False;User ID=kc99;Password=EpIcPaSsWoRd!9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
      
      //_connectionString = @"Server = DESKTOP - LFET3QL\LINGUONATOR2; Database = userDB; Integrated Security = true";
    }
    protected SqlConnection GetConnection()
    {
      return new SqlConnection(_connectionString);
    }
  }
}
