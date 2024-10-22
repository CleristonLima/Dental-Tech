using DentalPlus.Uteis;
using MySql.Data.MySqlClient;
using System.Data;

namespace DentalPlus.Models
{
    public class DeclaracaoHorasModel
    {
        public string IdHoursDeclaration { get; set; }

        public string IdPatients { get; set; }

        public string NamePatient { get; set; }

        public DateOnly DateIssue {  get; set; }
        
        public TimeSpan HourStart { get; set; }

        public TimeSpan HourFinish { get; set; }

        public string Reason { get; set; }

        public string userId { get; set; }

        public void GravarHoras()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Se for uma inserção, preenche o campo USER_INSERT
            sql = "INSERT INTO TB_MR_HOURS_DECLARATION (ID_PATIENTS, DATE_ISSUE, HOUR_START, HOUR_FINISH, REASON, USER_INSERT, DATE_INSERT) " +
                                 "VALUES (@IdPatients, @DateIssue, @HourStart, @HourFinish, @Reason, @userInsert, @dateInsert)";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdPatients", IdPatients);
            command.Parameters.AddWithValue("@DateIssue", DateIssue.ToDateTime(TimeOnly.MinValue));
            command.Parameters.AddWithValue("@HourStart", HourStart.ToString(@"hh\:mm"));
            command.Parameters.AddWithValue("@HourFinish", HourFinish.ToString(@"hh\:mm"));
            command.Parameters.AddWithValue("@Reason", Reason);
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
    }
}
