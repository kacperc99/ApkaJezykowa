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
      _connectionString = @"Server=DESKTOP-LFET3LQ\LINGUONATOR2; Database=userDB; Integrated Security=true";
    }
    protected SqlConnection GetConnection()
    {
      return new SqlConnection(_connectionString);
    }
  }
}
