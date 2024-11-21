using DentalPlus.Uteis;
using MySql.Data.MySqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Emit;

namespace DentalPlus.Models
{
    public class ContasPagarModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContasPagarModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ContasPagarModel()
        {

        }
        public string IdAccountPay {  get; set; }

        public string DescAccountPay { get; set; }

        public decimal ValueAccountPay { get; set; }

        public string Month {  get; set; }

        public string Year { get; set; }

        public DateOnly Expiration_Date {  get; set; }

        public DateOnly? Date_Payment { get; set; }

        public string Status { get; set; }

        public string userId { get; set; }

        public List<ContasPagarModel> ListarTodosContas()
        {
            List<ContasPagarModel> lista = new List<ContasPagarModel>();
            ContasPagarModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT ID_ACCOUNT_PAY, " +
	                          "DESC_ACCOUNT_PAY, " +
                              "VALUE_ACCOUNT_PAY, " +
                              "MONTH, " +
                              "YEAR, " +
                              "EXPIRATION_DATE, " +
                              "DATE_PAYMENT, " +
                              "STATUS " +
                        "FROM TB_MB_ACCOUNTS_PAYABLE " +
                        "ORDER BY MONTH, YEAR DESC";
            DataTable dt = objDAL.RetDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                item = new ContasPagarModel(_httpContextAccessor)
                {
                    IdAccountPay = row["ID_ACCOUNT_PAY"].ToString(),
                    DescAccountPay = row["DESC_ACCOUNT_PAY"].ToString(),
                    ValueAccountPay = Convert.ToDecimal(row["VALUE_ACCOUNT_PAY"].ToString()),
                    Month = row["MONTH"].ToString(),
                    Year = row["YEAR"].ToString(),
                    Expiration_Date = DateOnly.FromDateTime(DateTime.Parse(row["EXPIRATION_DATE"].ToString())),
                    Date_Payment = row["DATE_PAYMENT"] != DBNull.Value? DateOnly.FromDateTime(DateTime.Parse(row["DATE_PAYMENT"].ToString())) : null,
                    Status = row["STATUS"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }

        public ContasPagarModel RetornarConta(int? id)
        {

            ContasPagarModel item = null;
            DAL objDAL = new DAL();
            string sql = $"SELECT ID_ACCOUNT_PAY, " +
                              $"DESC_ACCOUNT_PAY, " +
                              $"VALUE_ACCOUNT_PAY, " +
                              $"MONTH, " +
                              $"YEAR, " +
                              $"EXPIRATION_DATE, " +
                              $"STATUS " +
                        $"FROM TB_MB_ACCOUNTS_PAYABLE " +
                        $"WHERE ID_ACCOUNT_PAY = '{id}' " +
                        $"ORDER BY MONTH, YEAR DESC";
            DataTable dt = objDAL.RetDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                DateTime expirationDate;

                if (DateTime.TryParse(dt.Rows[0]["EXPIRATION_DATE"].ToString(), out expirationDate))
                {
                    item = new ContasPagarModel
                    {
                        IdAccountPay = dt.Rows[0]["ID_ACCOUNT_PAY"].ToString(),
                        DescAccountPay = dt.Rows[0]["DESC_ACCOUNT_PAY"].ToString(),
                        ValueAccountPay = Convert.ToDecimal(dt.Rows[0]["VALUE_ACCOUNT_PAY"].ToString()),
                        Month = dt.Rows[0]["MONTH"].ToString(),
                        Year = dt.Rows[0]["YEAR"].ToString(),
                        Expiration_Date = DateOnly.FromDateTime(DateTime.Parse(dt.Rows[0]["EXPIRATION_DATE"].ToString())),
                        Status = dt.Rows[0]["STATUS"].ToString()
                    };
                }
                else
                {
                    // Lidar com o caso em que a data não está no formato esperado
                    throw new FormatException("A data de nascimento não está no formato correto.");
                }
            }

            return item;
        }

        public void Gravar()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(IdAccountPay))
            {
                // Se for uma atualização, preenche o campo USER_UPDATE
                sql = "UPDATE TB_MB_ACCOUNTS_PAYABLE SET DESC_ACCOUNT_PAY = @DescAccountPay, " +
                    "VALUE_ACCOUNT_PAY = @ValueAccountPay, " +
                    "MONTH = @Month, " +
                    "YEAR = @Year, " +
                    "EXPIRATION_DATE = @Expiration_Date, " +
                    "DATE_PAYMENT = @Date_Payment, " +
                    "STATUS = @Status, " +
                    "USER_UPDATE = @userUpdate, " +
                    "DATE_UPDATE = @dateUpdate " +
                    "WHERE ID_ACCOUNT_PAY = @IdAccountPay";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@DescAccountPay", DescAccountPay);
                command.Parameters.AddWithValue("@ValueAccountPay", ValueAccountPay);
                command.Parameters.AddWithValue("@Month", Month);
                command.Parameters.AddWithValue("@Year", Year);
                command.Parameters.AddWithValue("@Expiration_Date", Expiration_Date.ToDateTime(TimeOnly.MinValue));
                command.Parameters.AddWithValue("@Date_Payment", Date_Payment.HasValue ? Date_Payment.Value.ToDateTime(TimeOnly.MinValue) : (object)DBNull.Value);
                command.Parameters.AddWithValue("@Status", Status);
                command.Parameters.AddWithValue("@userUpdate", userId);
                command.Parameters.AddWithValue("@dateUpdate", currentDateTime);
                command.Parameters.AddWithValue("@IdAccountPay", IdAccountPay);

                objDAL.ExecutarComandoSQL(command);
            }
            else
            {
                // Se for uma inserção, preenche o campo USER_INSERT
                sql = "INSERT INTO TB_MB_ACCOUNTS_PAYABLE (DESC_ACCOUNT_PAY, VALUE_ACCOUNT_PAY, MONTH, YEAR, EXPIRATION_DATE, STATUS, USER_INSERT, DATE_INSERT) " +
                                     "VALUES (@DescAccountPay, @ValueAccountPay, @Month, @Year, @Expiration_Date, @Status, @userInsert, @dateInsert)";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@DescAccountPay", DescAccountPay);
                command.Parameters.AddWithValue("@ValueAccountPay", ValueAccountPay);
                command.Parameters.AddWithValue("@Month", Month);
                command.Parameters.AddWithValue("@Year", Year);
                command.Parameters.AddWithValue("@Expiration_Date", Expiration_Date.ToDateTime(TimeOnly.MinValue));
                command.Parameters.AddWithValue("@Status", Status);
                command.Parameters.AddWithValue("@userInsert", userId);
                command.Parameters.AddWithValue("@dateInsert", currentDateTime);
                command.Parameters.AddWithValue("@IdAccountPay", IdAccountPay);

                objDAL.ExecutarComandoSQL(command);
            }
        }

        public void Excluir(int id)
        {
            DAL objDAL = new DAL();
            string sql = "DELETE FROM TB_MB_ACCOUNTS_PAYABLE WHERE ID_ACCOUNT_PAY = @IdAccountPay";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdAccountPay", id);

            objDAL.ExecutarComandoSQL(command);
        }
    }
}
