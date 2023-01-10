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
      _connectionString = @"Data Source=SQL8004.site4now.net;Initial Catalog=db_a92e16_linguonator;User Id=db_a92e16_linguonator_admin;Password=sexsiman4";
    }
    protected SqlConnection GetConnection()
    {
      return new SqlConnection(_connectionString);
    }
  }
}
