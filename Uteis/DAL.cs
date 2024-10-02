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

        // Método para abrir uma conexão e garantir que ela seja fechada após o uso
        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        // Retorna um DataTable com base em uma string SQL
         public DataTable RetDataTable(string sql)
         {
             using (MySqlConnection conn = GetConnection())
             {
                 conn.Open();
                 using (MySqlCommand Command = new MySqlCommand(sql, conn))
                 {
                     MySqlDataAdapter da = new MySqlDataAdapter(Command);
                     DataTable data = new DataTable();
                     da.Fill(data);
                     return data;
                 }
             }
         }

        public DataTable RetDataTableObj(string sql, Dictionary<string, object> parameters)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand command = new MySqlCommand(sql, conn))
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }

                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    DataTable data = new DataTable();
                    da.Fill(data);
                    return data;
                }
            }
        }

        // Retorna um DataTable com base em um MySqlCommand preparado
        public DataTable RetDataTable(MySqlCommand Command)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                Command.Connection = conn;

                MySqlDataAdapter da = new MySqlDataAdapter(Command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        // Executa comandos SQL de INSERT, UPDATE e DELETE
        public void ExecutarComandoSQL(string sql)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                using (MySqlCommand Command = new MySqlCommand(sql, conn))
                {
                    Command.ExecuteNonQuery();
                }
            }
        }

        // Executa comandos SQL preparados, como no caso de parâmetros
        public void ExecutarComandoSQL(MySqlCommand Command)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                Command.Connection = conn;
                Command.ExecuteNonQuery();
            }
        }

        public object RetornarValor(string sql)
        {
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                return cmd.ExecuteScalar();
            }
        }
    }
}