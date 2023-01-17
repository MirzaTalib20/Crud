using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Dapper;
using CrudOneMore.Models;


namespace CrudOneMore.Models
{
    public  class DapperORM
    {
        private static string connectionString = @"Data Source=DESKTOP-4QTM0CJ;Initial Catalog=DapperDB;User ID=sa;Password=123";
        




        public static void ExecuteWithoutReturn (string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
               
                con.Open();
                
                con.Execute(procedureName, param, commandType: CommandType.StoredProcedure);
            }
        }

        public static T ExecuteReturnScalar<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                
                con.Open();
                
                return (T) Convert.ChangeType(con.Execute(procedureName, param, commandType: CommandType.StoredProcedure),typeof(T));
            }
        }




        public static IEnumerable<T> ReturnList<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
             
                return con.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure);
            }
        }
    }


}