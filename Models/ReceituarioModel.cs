using DentalPlus.Uteis;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc.Rendering;
using MySql.Data.MySqlClient;
using System.Data;

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

        public string IdDoctor { get; set; }

        public string NamePatient {  get; set; }

        public string NameDoctor { get; set; }

        public string Symptoms { get; set; }

        public DateOnly dateIssue { get; set; }

        public string IdMedicinePrescription { get; set; }

        public string IdProductRevenue { get; set; }

        public double Qty { get; set; }

        public string TypeMedication { get; set; }

        public string HourMedication { get; set; }

        public string TimeMedication { get; set; }

        public string DaysMedication { get; set; }

        public string userId { get; set; }

        public List<ProdutoReceitaModel> Medicamentos { get; set; } = new List<ProdutoReceitaModel>();


        public void Gravar()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Se for uma inserção, preenche o campo USER_INSERT
            sql = "INSERT INTO TB_MR_RECIPE (ID_PATIENTS, ID_DOCTOR, SYMPTOMS, DATE_ISSUE, USER_INSERT, DATE_INSERT) " +
                                 "VALUES (@IdPatients, @IdDoctor, @Symptoms, @dateIssue, @userInsert, @dateInsert)";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdPatients", IdPatients);
            command.Parameters.AddWithValue("@IdDoctor", IdDoctor);
            command.Parameters.AddWithValue("@Symptoms", Symptoms);
            command.Parameters.AddWithValue("@dateIssue", dateIssue.ToDateTime(TimeOnly.MinValue));
            command.Parameters.AddWithValue("@userInsert", userId);
            command.Parameters.AddWithValue("@dateInsert", currentDateTime);

            objDAL.ExecutarComandoSQL(command);

        }

        public void ConsultarNomePaciente()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            sql = "SELECT NAME_PATIENT FROM TB_CLI_PATIENTS WHERE ID_PATIENTS = @IdPatients";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdPatients", IdPatients);

            DataTable dt = objDAL.RetDataTable(command);

            if (dt.Rows.Count > 0)
            {
                NamePatient = dt.Rows[0]["NAME_PATIENT"].ToString();
            
            }
        }

        public void ConsultarNomeMedico()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            sql = "SELECT NAME_DOCTOR FROM TB_CLI_DOCTORS WHERE ID_DOCTOR = @IdDoctor";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdDoctor", IdDoctor);

            DataTable dt = objDAL.RetDataTable(command);

            if (dt.Rows.Count > 0)
            {
                NameDoctor = dt.Rows[0]["NAME_DOCTOR"].ToString();

            }
        }

        public List<SelectListItem> ConsultarRemedio()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            sql = "SELECT ID_PRODUCT_REVENUE, NAME_PRODUCT FROM TB_PR_PRODUCT_REVENUE ";

            DataTable dt = objDAL.RetDataTable(sql);

            List<SelectListItem> remedios = new List<SelectListItem>();

            foreach (DataRow row in dt.Rows)
            {
                remedios.Add(new SelectListItem
                {
                    Value = row["ID_PRODUCT_REVENUE"].ToString(),
                    Text = row["NAME_PRODUCT"].ToString()
                });
            }

            return remedios;
    }

        public void GravarRemedios()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

           /* foreach (var medicamento in Medicamentos)
            {
                // Se for uma inserção, preenche o campo USER_INSERT
                sql = "INSERT INTO TB_MR_MEDICINE_PRESCRIPTIONS (ID_MR_RECIPE, ID_PRODUCT_REVENUE, QTY, REASON, USER_INSERT, DATE_INSERT) " +
                                 "VALUES (@IdMrRecipe, @IdProductRevenue, @Qty, @Reason, @userInsert, @dateInsert)";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@IdMrRecipe", IdMrRecipe);
                command.Parameters.AddWithValue("@IdProductRevenue", IdProductRevenue);
                command.Parameters.AddWithValue("@Qty", Qty);
                command.Parameters.AddWithValue("@Reason", Reason);
                command.Parameters.AddWithValue("@userInsert", userId);
                command.Parameters.AddWithValue("@dateInsert", currentDateTime);

                objDAL.ExecutarComandoSQL(command);
            }*/

        }
    }
}
