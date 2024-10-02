using DentalPlus.Uteis;
using System.Data;

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

        public string NameDoctor { get; set; }

        public string IdPatients { get; set; }

        public string NamePatient { get; set; }

        public string IdPayment { get; set; }

        public string DescPayment { get; set; }

        public string IdMedicalConsultation { get; set; }

        public string DescMedicalConsultation { get; set; }

        public double Qty {  get; set; }

        public double ValueConsultation { get; set; }

        public double TotalValue { get; set; }

        public string userId { get; set; }

        public decimal GetConsultaPreco(string idConsulta)
        {
            DAL objDAL = new DAL();

            string query = "SELECT VALUE_CONSULTATION FROM TB_CLI_MEDICAL_CONSULTATION WHERE IdMedicalConsultation = @Id";
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

    }
}
