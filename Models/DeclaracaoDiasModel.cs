using DentalPlus.Uteis;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DentalPlus.Models
{
    public class DeclaracaoDiasModel
    {
        public string IdDaysDeclaration { get; set; }

        public string IdPatients { get; set; }

        [Required(ErrorMessage = "Por favor selecione o paciente!")]
        public string NamePatient { get; set; }

        public string RgRne { get; set; }

        [Required(ErrorMessage = "Por favor selecione o Medico!")]
        public string IdDoctor { get; set; }

        public string NameDoctor { get; set; }

        public string CrmCro { get; set; }

        [Required(ErrorMessage = "Por favor informe a data de emissão!")]
        public DateOnly DateIssue { get; set; }

        [Required(ErrorMessage = "Por favor a quantidade de dias de licença!")]
        public string DaysDeclaration { get; set; }

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

        public void GravarDias()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Se for uma inserção, preenche o campo USER_INSERT
            sql = "INSERT INTO TB_MR_DAYS_DECLARATION (ID_PATIENTS, ID_DOCTOR, DATE_ISSUE, DAYS_DECLARATION, REASON, USER_INSERT, DATE_INSERT) " +
                                 "VALUES (@IdPatients, @IdDoctor, @DateIssue, @DaysDeclaration, @Reason, @userInsert, @dateInsert)";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdPatients", IdPatients);
            command.Parameters.AddWithValue("@IdDoctor", IdDoctor);
            command.Parameters.AddWithValue("@DateIssue", DateIssue.ToDateTime(TimeOnly.MinValue));
            command.Parameters.AddWithValue("@DaysDeclaration", DaysDeclaration);
            command.Parameters.AddWithValue("@Reason", Reason);
            command.Parameters.AddWithValue("@userInsert", userId);
            command.Parameters.AddWithValue("@dateInsert", currentDateTime);

            objDAL.ExecutarComandoSQL(command);

        }
    }
}
