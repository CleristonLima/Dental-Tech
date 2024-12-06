using DentalPlus.Uteis;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DentalPlus.Models
{
    public class DeclaracaoHorasModel
    {
        public string IdHoursDeclaration { get; set; }
                
        public string IdPatients { get; set; }

        [Required(ErrorMessage = "Por favor selecione o paciente!")]
        public string NamePatient { get; set; }

        [Required(ErrorMessage = "Por favor selecione o Medico!")]
        public string IdDoctor { get; set; }

        public string NameDoctor { get; set; }

        public string RgRne { get; set; }

        public string CrmCro { get; set; }

        [Required(ErrorMessage = "Por favor informe a data de emissão!")]
        public DateOnly DateIssue {  get; set; }

        [Required(ErrorMessage = "Por favor informe a hora de chegada!")]
        public TimeSpan HourStart { get; set; }

        [Required(ErrorMessage = "Por favor informe a hora de saída!")]
        public TimeSpan HourFinish { get; set; }

        [Required(ErrorMessage = "Por favor informe o motivo!")]
        public string Reason { get; set; }

        public string userId { get; set; }


        public void ConsultarNomePaciente()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            sql = "SELECT NAME_PATIENT, RG_RNE FROM TB_CLI_PATIENTS WHERE ID_PATIENTS = @IdPatients";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdPatients", IdPatients);

            DataTable dt = objDAL.RetDataTable(command);

            if (dt.Rows.Count > 0)
            {
                NamePatient = dt.Rows[0]["NAME_PATIENT"].ToString();
                RgRne = dt.Rows[0]["RG_RNE"].ToString();

            }
        }

        public void ConsultarNomeMedico()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            sql = "SELECT NAME_DOCTOR, CRM_CRO FROM TB_CLI_DOCTORS WHERE ID_DOCTOR = @IdDoctor";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdDoctor", IdDoctor);

            DataTable dt = objDAL.RetDataTable(command);

            if (dt.Rows.Count > 0)
            {
                NameDoctor = dt.Rows[0]["NAME_DOCTOR"].ToString();
                CrmCro = dt.Rows[0]["CRM_CRO"].ToString();

            }
        }

        public void GravarHoras()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Se for uma inserção, preenche o campo USER_INSERT
            sql = "INSERT INTO TB_MR_HOURS_DECLARATION (ID_PATIENTS, ID_DOCTOR, DATE_ISSUE, HOUR_START, HOUR_FINISH, REASON, USER_INSERT, DATE_INSERT) " +
                                 "VALUES (@IdPatients, @IdDoctor, @DateIssue, @HourStart, @HourFinish, @Reason, @userInsert, @dateInsert)";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdPatients", IdPatients);
            command.Parameters.AddWithValue("@IdDoctor", IdDoctor);
            command.Parameters.AddWithValue("@DateIssue", DateIssue.ToDateTime(TimeOnly.MinValue));
            command.Parameters.AddWithValue("@HourStart", HourStart.ToString(@"hh\:mm"));
            command.Parameters.AddWithValue("@HourFinish", HourFinish.ToString(@"hh\:mm"));
            command.Parameters.AddWithValue("@Reason", Reason);
            command.Parameters.AddWithValue("@userInsert", userId);
            command.Parameters.AddWithValue("@dateInsert", currentDateTime);

            objDAL.ExecutarComandoSQL(command);

        }

        
    }
}
