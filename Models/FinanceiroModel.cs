using DentalPlus.Uteis;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;

namespace DentalPlus.Models
{
    public class FinanceiroModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FinanceiroModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public FinanceiroModel()
        {

        }
        public string IdMbSale {  get; set; }

        public string IdDoctor { get; set; }

        [Required(ErrorMessage = "Informe o médico!")]
        public string NameDoctor { get; set; }

        public string IdPatients { get; set; }

        public string NamePatient { get; set; }

        public string IdPayment { get; set; }

        public string DescPayment { get; set; }

        public string IdMedicalConsultation { get; set; }

        public string DescMedicalConsultation { get; set; }

        public double Qty {  get; set; }

        public decimal ValueConsultation { get; set; }

        public decimal TotalValue { get; set; }

        public string userId { get; set; }

        public decimal GetConsultaPreco(string idConsulta)
        {
            DAL objDAL = new DAL();

            string query = "SELECT VALUE_CONSULTATION FROM TB_CLI_MEDICAL_CONSULTATION WHERE ID_MEDICAL_CONSULTATION = @Id";
            var parameters = new Dictionary<string, object>
            {
                    { "@Id", idConsulta }
            };
            DataTable dt = objDAL.RetDataTableObj(query, parameters);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToDecimal(dt.Rows[0]["VALUE_CONSULTATION"]);
            }
            else
            {
                throw new Exception("Consulta não encontrada.");
            }
        }

        public void Gravar()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Garantir que o valor total esteja no formato correto
            decimal totalValueFormatted = Math.Round(TotalValue, 2, MidpointRounding.AwayFromZero);

            // Inserir na tabela TB_MB_SALE
            sql = "INSERT INTO TB_MB_SALE (ID_DOCTOR, ID_PATIENTS, ID_PAYMENT, TOTAL_VALUE, USER_INSERT, DATE_INSERT) " +
                                "VALUES (@IdDoctor, @IdPatients, @IdPayment, @TotalValue, @userInsert, @dateInsert)";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdDoctor", IdDoctor);
            command.Parameters.AddWithValue("@IdPatients", IdPatients);
            command.Parameters.AddWithValue("@IdPayment", IdPayment);
            command.Parameters.AddWithValue("@TotalValue", totalValueFormatted);
            command.Parameters.AddWithValue("@userInsert", userId);
            command.Parameters.AddWithValue("@dateInsert", currentDateTime);

            objDAL.ExecutarComandoSQL(command);


        }
    }

}
