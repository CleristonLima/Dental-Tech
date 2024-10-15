using DentalPlus.Uteis;
using DocumentFormat.OpenXml.Spreadsheet;
using MySql.Data.MySqlClient;

namespace DentalPlus.Models
{
    public class ReceituarioModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReceituarioModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ReceituarioModel()
        {

        }

        public string IdMrRecipe { get; set; }

        public string IdPatients { get; set; }

        public string Symptoms { get; set; }

        public DateOnly dateIssue { get; set; }

        public string IdMedicinePrescription { get; set; }

        public string IdProductRevenue { get; set; }

        public double Qty { get; set; }

        public string userId { get; set; }


        public void Gravar()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Se for uma inserção, preenche o campo USER_INSERT
            sql = "INSERT INTO TB_MR_RECIPE (ID_PATIENTS, SYMPTOMS, DATE_ISSUE, USER_INSERT, DATE_INSERT) " +
                                 "VALUES (@IdPatients, @Symptoms, @dateIssue, @userInsert, @dateInsert)";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdPatients", IdPatients);
            command.Parameters.AddWithValue("@Symptoms", Symptoms);
            command.Parameters.AddWithValue("@dateIssue", dateIssue.ToDateTime(TimeOnly.MinValue));
            command.Parameters.AddWithValue("@userInsert", userId);
            command.Parameters.AddWithValue("@dateInsert", currentDateTime);

            objDAL.ExecutarComandoSQL(command);

        }
    }
}
