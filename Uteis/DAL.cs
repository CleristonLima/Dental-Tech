using System.Data;
using MySql.Data.MySqlClient;

namespace DentalPlus.Uteis
{
    public class DAL
    {
        private static string Server = "localhost";
        private static string Database = "db_odontology";
        private static string User = "root";
        private static string Password = "";
        private static string ConnectionString = $"Server={Server};Database={Database};Uid={User};Pwd={Password};Sslmode=none;Charset=utf8;";
        private static MySqlConnection Connection;

        public DAL()
        {
            Connection = new MySqlConnection(ConnectionString);
            Connection.Open();
        }

        // Espera um parametro do tipo string contendo um comando do tipo SELECT
        public DataTable RetDataTable(string sql)
        {
            DataTable data = new DataTable();
            MySqlCommand Command = new MySqlCommand(sql, Connection);
            MySqlDataAdapter da = new MySqlDataAdapter(Command);
            da.Fill(data);
            return data;
        }

        // Recebe o comando preparado
        public DataTable RetDataTable(MySqlCommand Command)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                Command.Connection = conn;

                MySqlDataAdapter da = new MySqlDataAdapter(Command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Espera um parametro do tipo String contendo um comando SQL do tipo INSERT, UPDATE e DELETE
        public void ExecutarComandoSQL(string sql)
        {
            MySqlCommand Command = new MySqlCommand(sql, Connection);
            Command.ExecuteNonQuery();
        }
    }
}