using DentalPlus.Uteis;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.ComponentModel.DataAnnotations;
using System.Data;
using DataTable = System.Data.DataTable;

namespace DentalPlus.Models
{
    public class AgendamentoModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AgendamentoModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public AgendamentoModel()
        {

        }

        public string IdMedConsXPat { get; set; }

        public string IdPatients { get; set; }

        [Required(ErrorMessage = "Informe o paciente!")]
        public string NamePatient { get; set; }

        public string CpfCnpj { get; set; }

        public string PhoneNumber1 { get; set; }

        public string PhoneNumber2 { get; set; }

        public string IdDoctor {  get; set; }

        [Required(ErrorMessage = "Informe o médico!")]
        public string NameDoctor { get; set; }

        public string CrmCro { get; set; }

        public string IdMedicalConsultation { get; set; }

        [Required(ErrorMessage = "Informe o tipo de consulta medica!")]
        public string NameMedicalConsultation { get; set; }

        [Required(ErrorMessage = "Informe a data e hora do inicio da consulta!")]
        public DateTime DateConsultationStart { get; set; }

        [Required(ErrorMessage = "Informe a data e hora do final da consulta!")]
        public DateTime DateConsultationFinish { get; set; }

        [Required(ErrorMessage = "Informe o status da consulta!")]
        public string StatusConsultation { get; set; }

        public string Reason { get; set; }

        public string LinkPhoto { get; set; }

        public DateTime DateCancellation { get; set; }

        public string userId { get; set; }

        public string ErrorMessage { get; set; }

        public List<AgendamentoModel> ListarTodosAgendamentos()
        {
            List<AgendamentoModel> lista = new List<AgendamentoModel>();
            AgendamentoModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT CMCP.ID_MED_CONS_X_PAT, " +
                               "CP.LINK_PHOTO, " +
                               "CP.NAME_PATIENT, " +
                               "CP.CPF_CNPJ, " +
                               "CD.NAME_DOCTOR, " +
                               "CD.CRM_CRO, " +
                               "CMC.DESC_MEDICAL_CONSULTATION, " +
                               "CMCP.DATE_CONSULTATION_START, " +
                               "CMCP.DATE_CONSULTATION_FINISH, " +
                               "CMCP.STATUS_CONSULTATION, " +
                               "CMCP.REASON " +
                        "FROM TB_CLI_MED_CONSUL_X_PATIENT CMCP " +
                        "INNER JOIN TB_CLI_DOCTORS CD ON CD.ID_DOCTOR = CMCP.ID_DOCTOR " +
                        "INNER JOIN TB_CLI_PATIENTS CP ON CP.ID_PATIENTS = CMCP.ID_PATIENTS " +
                        "INNER JOIN TB_CLI_MEDICAL_CONSULTATION CMC ON CMC.ID_MEDICAL_CONSULTATION = CMCP.ID_MEDICAL_CONSULTATION " +
                        "WHERE CMCP.STATUS_CONSULTATION NOT IN ('Cancelada', 'Realizada') " +
                        "ORDER BY CMCP.DATE_CONSULTATION_START DESC";

            DataTable dt = objDAL.RetDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                item = new AgendamentoModel(_httpContextAccessor)
                {
                    IdMedConsXPat = row["ID_MED_CONS_X_PAT"].ToString(),
                    LinkPhoto = row["LINK_PHOTO"].ToString(),
                    NamePatient = row["NAME_PATIENT"].ToString(),
                    CpfCnpj = row["CPF_CNPJ"].ToString(),
                    NameDoctor = row["NAME_DOCTOR"].ToString(),
                    CrmCro = row["CRM_CRO"].ToString(),
                    NameMedicalConsultation = row["DESC_MEDICAL_CONSULTATION"].ToString(),
                    DateConsultationStart = DateTime.Parse(row["DATE_CONSULTATION_START"].ToString()),
                    DateConsultationFinish = DateTime.Parse(row["DATE_CONSULTATION_FINISH"].ToString()),
                    StatusConsultation = row["STATUS_CONSULTATION"].ToString(),
                    Reason = row["REASON"].ToString(),
                };

                lista.Add(item);
            }

            return lista;
        }

        public List<Tuple<DateTime, DateTime>> ObterHorariosOcupados(DateTime data, string idDoctor)
        {
            List<Tuple<DateTime, DateTime>> horariosOcupados = new List<Tuple<DateTime, DateTime>>();
            DAL objDAL = new DAL();

            string sql = "SELECT DATE_CONSULTATION_START, DATE_CONSULTATION_FINISH " +
                         "FROM TB_CLI_MED_CONSUL_X_PATIENT " +
                         "WHERE DATE_CONSULTATION_START >= @dataInicio " +
                         "AND DATE_CONSULTATION_START < @dataFim " +
                         "AND ID_DOCTOR = @idDoctor " +
                         "AND STATUS_CONSULTATION != 'Cancelada'";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@dataInicio", data.Date);
            command.Parameters.AddWithValue("@dataFim", data.Date.AddDays(1));
            command.Parameters.AddWithValue("@idDoctor", idDoctor);

            DataTable dt = objDAL.RetDataTable(command);

            foreach (DataRow row in dt.Rows)
            {
                DateTime inicio = DateTime.Parse(row["DATE_CONSULTATION_START"].ToString());
                DateTime fim = DateTime.Parse(row["DATE_CONSULTATION_FINISH"].ToString());
                horariosOcupados.Add(new Tuple<DateTime, DateTime>(inicio, fim));
            }

            return horariosOcupados;
        }

        public bool HorarioDisponivel(DateTime inicio, DateTime fim, string idDoctor)
        {
            DAL objDAL = new DAL();

            string sql = "SELECT COUNT(*) " +
                   "FROM TB_CLI_MED_CONSUL_X_PATIENT " +
                   "WHERE ID_DOCTOR = @IdDoctor " +
                   "AND STATUS_CONSULTATION != 'Cancelada' " +
                   "AND((DATE_CONSULTATION_START < @Fim AND DATE_CONSULTATION_FINISH > @Inicio))" ;

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdDoctor", idDoctor);
            command.Parameters.AddWithValue("@Inicio", inicio);
            command.Parameters.AddWithValue("@Fim", fim);

            int count = Convert.ToInt32(objDAL.RetDataTable(command).Rows[0][0]);

            if (count > 0) {

                ErrorMessage = "Não é possivel agendar nesse horário pois ja existe paciente com consulta nesse horario ";
            }

            return count == 0; // Retorna verdadeiro se não houver conflito
        }

        public AgendamentoModel RetornarAgendamento(int? id)
        {
            AgendamentoModel item = null;
            DAL objDAL = new DAL();
            string sql = $"SELECT CMCP.ID_MED_CONS_X_PAT, " +
                               $"CP.ID_PATIENTS, " +
                               $"CD.ID_DOCTOR, " +
                               $"CMC.ID_MEDICAL_CONSULTATION, " +
                               $"CMCP.DATE_CONSULTATION_START, " +
                               $"CMCP.DATE_CONSULTATION_FINISH, " +
                               $"CMCP.STATUS_CONSULTATION, " +
                               $"CMCP.REASON " +
                        $"FROM TB_CLI_MED_CONSUL_X_PATIENT CMCP " +
                        $"INNER JOIN TB_CLI_DOCTORS CD ON CD.ID_DOCTOR = CMCP.ID_DOCTOR " +
                        $"INNER JOIN TB_CLI_PATIENTS CP ON CP.ID_PATIENTS = CMCP.ID_PATIENTS " +
                        $"INNER JOIN TB_CLI_MEDICAL_CONSULTATION CMC ON CMC.ID_MEDICAL_CONSULTATION = CMCP.ID_MEDICAL_CONSULTATION " +
                        $"WHERE CMCP.ID_MED_CONS_X_PAT = '{id}' ORDER BY CMCP.DATE_CONSULTATION_START DESC";
            DataTable dt = objDAL.RetDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                DateTime dateConsultationStart;
                DateTime dateConsultationFinish;

                if (DateTime.TryParse(dt.Rows[0]["DATE_CONSULTATION_START"].ToString(), out dateConsultationStart) && DateTime.TryParse(dt.Rows[0]["DATE_CONSULTATION_FINISH"].ToString(), out dateConsultationFinish))
                {
                    item = new AgendamentoModel
                    {
                        IdMedConsXPat = dt.Rows[0]["ID_MED_CONS_X_PAT"].ToString(),
                        IdPatients = dt.Rows[0]["ID_PATIENTS"].ToString(),
                        IdDoctor = dt.Rows[0]["ID_DOCTOR"].ToString(),
                        IdMedicalConsultation = dt.Rows[0]["ID_MEDICAL_CONSULTATION"].ToString(),
                        DateConsultationStart = dateConsultationStart,
                        DateConsultationFinish = dateConsultationFinish,
                        StatusConsultation = dt.Rows[0]["STATUS_CONSULTATION"].ToString(),
                        Reason = dt.Rows[0]["REASON"].ToString()
                    };
                }
                else
                {
                    throw new FormatException("A data de consulta não está no formato correto.");
                }
            }

            return item;
        }

        public List<AgendamentoModel> ListarTodasConsultasAmanha()
        {
            List<AgendamentoModel> lista = new List<AgendamentoModel>();
            AgendamentoModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT CMCP.ID_MED_CONS_X_PAT, " +
                               "CP.ID_PATIENTS, " +
                               "CP.NAME_PATIENT, " +
                               "CMC.ID_MEDICAL_CONSULTATION, " +
                               "CMC.DESC_MEDICAL_CONSULTATION, " +
                               "CMCP.DATE_CONSULTATION_START, " +
                               "CMCP.DATE_CONSULTATION_FINISH " +
                        "FROM TB_CLI_MED_CONSUL_X_PATIENT CMCP " +
                        "INNER JOIN TB_CLI_PATIENTS CP " +
                                "ON CP.ID_PATIENTS = CMCP.ID_PATIENTS " +
                        "INNER JOIN TB_CLI_MEDICAL_CONSULTATION CMC " +
                                "ON CMC.ID_MEDICAL_CONSULTATION = CMCP.ID_MEDICAL_CONSULTATION " +
                        "WHERE CMCP.DATE_CONSULTATION_START BETWEEN DATE_ADD(CURDATE(), INTERVAL 1 DAY)" +
                        "AND DATE_ADD(CURDATE(), INTERVAL 2 DAY) -INTERVAL 1 SECOND " +
                        "ORDER BY CMCP.DATE_CONSULTATION_START ASC";

            DataTable dt = objDAL.RetDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                item = new AgendamentoModel(_httpContextAccessor)
                {
                    IdMedConsXPat = row["ID_MED_CONS_X_PAT"].ToString(),
                    IdPatients = row["ID_PATIENTS"].ToString(),
                    NamePatient = row["NAME_PATIENT"].ToString(),
                    IdMedicalConsultation = row["ID_MEDICAL_CONSULTATION"].ToString(),
                    NameMedicalConsultation = row["DESC_MEDICAL_CONSULTATION"].ToString(),
                    DateConsultationStart = DateTime.Parse(row["DATE_CONSULTATION_START"].ToString()),
                    DateConsultationFinish = DateTime.Parse(row["DATE_CONSULTATION_FINISH"].ToString())
                };

                lista.Add(item);
            }

            return lista;
        }

        public AgendamentoModel RetornarAgendamentoParaCancelar(int? id)
        {
            AgendamentoModel item = null;
            DAL objDAL = new DAL();
            string sql = $"SELECT CMCP.ID_MED_CONS_X_PAT, " +
                               $"CP.ID_PATIENTS, " +
                               $"CD.ID_DOCTOR, " +
                               $"CMC.ID_MEDICAL_CONSULTATION " +
                        $"FROM TB_CLI_MED_CONSUL_X_PATIENT CMCP " +
                        $"INNER JOIN TB_CLI_DOCTORS CD ON CD.ID_DOCTOR = CMCP.ID_DOCTOR " +
                        $"INNER JOIN TB_CLI_PATIENTS CP ON CP.ID_PATIENTS = CMCP.ID_PATIENTS " +
                        $"INNER JOIN TB_CLI_MEDICAL_CONSULTATION CMC ON CMC.ID_MEDICAL_CONSULTATION = CMCP.ID_MEDICAL_CONSULTATION " +
                        $"WHERE CMCP.ID_MED_CONS_X_PAT = '{id}'";
            DataTable dt = objDAL.RetDataTable(sql);

            if (dt.Rows.Count > 0)
            {

                    item = new AgendamentoModel
                    {
                        IdMedConsXPat = dt.Rows[0]["ID_MED_CONS_X_PAT"].ToString(),
                        IdPatients = dt.Rows[0]["ID_PATIENTS"].ToString(),
                        IdDoctor = dt.Rows[0]["ID_DOCTOR"].ToString(),
                        IdMedicalConsultation = dt.Rows[0]["ID_MEDICAL_CONSULTATION"].ToString()
                    };
            }

            return item;
        }

        public AgendamentoModel RetornarPacienteParaEnviarMensagem(int? id)
        {
            AgendamentoModel item = null;
            DAL objDAL = new DAL();
            string sql = $"SELECT CMCP.ID_MED_CONS_X_PAT, " +
                               $"CP.ID_PATIENTS, " +
                               $"CP.PHONE_NUMBER_1, " +
                               $"CP.PHONE_NUMBER_2, " +
                               $"CMCP.DATE_CONSULTATION_START, " +
                               $"CD.ID_DOCTOR, " +
                               $"CMC.ID_MEDICAL_CONSULTATION " +
                        $"FROM TB_CLI_MED_CONSUL_X_PATIENT CMCP " +
                        $"INNER JOIN TB_CLI_DOCTORS CD ON CD.ID_DOCTOR = CMCP.ID_DOCTOR " +
                        $"INNER JOIN TB_CLI_PATIENTS CP ON CP.ID_PATIENTS = CMCP.ID_PATIENTS " +
                        $"INNER JOIN TB_CLI_MEDICAL_CONSULTATION CMC ON CMC.ID_MEDICAL_CONSULTATION = CMCP.ID_MEDICAL_CONSULTATION " +
                        $"WHERE CMCP.ID_MED_CONS_X_PAT = '{id}'";
            DataTable dt = objDAL.RetDataTable(sql);

            if (dt.Rows.Count > 0)
            {

                item = new AgendamentoModel
                {
                    IdMedConsXPat = dt.Rows[0]["ID_MED_CONS_X_PAT"].ToString(),
                    IdPatients = dt.Rows[0]["ID_PATIENTS"].ToString(),
                    PhoneNumber1 = dt.Rows[0]["PHONE_NUMBER_1"].ToString(),
                    PhoneNumber2 = dt.Rows[0]["PHONE_NUMBER_2"].ToString(),
                    DateConsultationStart = DateTime.Parse(dt.Rows[0]["DATE_CONSULTATION_START"].ToString()),
                    IdDoctor = dt.Rows[0]["ID_DOCTOR"].ToString(),
                    IdMedicalConsultation = dt.Rows[0]["ID_MEDICAL_CONSULTATION"].ToString()

                };
            }

            return item;
        }

        public List<AgendamentoModel> ListarTodasConsultasRealizadas()
        {
            List<AgendamentoModel> lista = new List<AgendamentoModel>();
            AgendamentoModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT CMCP.ID_MED_CONS_X_PAT, " +
                               "CP.LINK_PHOTO, " +
                               "CP.NAME_PATIENT, " +
                               "CP.CPF_CNPJ, " +
                               "CD.NAME_DOCTOR, " +
                               "CD.CRM_CRO, " +
                               "CMC.DESC_MEDICAL_CONSULTATION, " +
                               "CMCP.DATE_CONSULTATION_START, " +
                               "CMCP.DATE_CONSULTATION_FINISH, " +
                               "CMCP.STATUS_CONSULTATION, " +
                               "CMCP.REASON " +
                        "FROM TB_CLI_MED_CONSUL_X_PATIENT CMCP " +
                        "INNER JOIN TB_CLI_DOCTORS CD ON CD.ID_DOCTOR = CMCP.ID_DOCTOR " +
                        "INNER JOIN TB_CLI_PATIENTS CP ON CP.ID_PATIENTS = CMCP.ID_PATIENTS " +
                        "INNER JOIN TB_CLI_MEDICAL_CONSULTATION CMC ON CMC.ID_MEDICAL_CONSULTATION = CMCP.ID_MEDICAL_CONSULTATION " +
                        "WHERE CMCP.STATUS_CONSULTATION = 'Realizada' " +
                        "ORDER BY CMCP.DATE_CONSULTATION_START DESC";

            DataTable dt = objDAL.RetDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                item = new AgendamentoModel(_httpContextAccessor)
                {
                    IdMedConsXPat = row["ID_MED_CONS_X_PAT"].ToString(),
                    LinkPhoto = row["LINK_PHOTO"].ToString(),
                    NamePatient = row["NAME_PATIENT"].ToString(),
                    CpfCnpj = row["CPF_CNPJ"].ToString(),
                    NameDoctor = row["NAME_DOCTOR"].ToString(),
                    CrmCro = row["CRM_CRO"].ToString(),
                    NameMedicalConsultation = row["DESC_MEDICAL_CONSULTATION"].ToString(),
                    DateConsultationStart = DateTime.Parse(row["DATE_CONSULTATION_START"].ToString()),
                    DateConsultationFinish = DateTime.Parse(row["DATE_CONSULTATION_FINISH"].ToString()),
                    StatusConsultation = row["STATUS_CONSULTATION"].ToString(),
                    Reason = row["REASON"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }

        public List<AgendamentoModel> ListarTodasConsultasCanceladas()
        {
            List<AgendamentoModel> lista = new List<AgendamentoModel>();
            AgendamentoModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT CMCP.ID_MED_CONS_X_PAT, " +
                               "CP.LINK_PHOTO, " +
                               "CP.NAME_PATIENT, " +
                               "CP.CPF_CNPJ, " +
                               "CD.NAME_DOCTOR, " +
                               "CD.CRM_CRO, " +
                               "CMC.DESC_MEDICAL_CONSULTATION, " +
                               "CMCP.DATE_CONSULTATION_START, " +
                               "CMCP.DATE_CONSULTATION_FINISH, " +
                               "CMCP.DATE_CANCELLATION, " +
                               "CMCP.STATUS_CONSULTATION, " +
                               "CMCP.REASON " +
                        "FROM TB_CLI_MED_CONSUL_X_PATIENT CMCP " +
                        "INNER JOIN TB_CLI_DOCTORS CD ON CD.ID_DOCTOR = CMCP.ID_DOCTOR " +
                        "INNER JOIN TB_CLI_PATIENTS CP ON CP.ID_PATIENTS = CMCP.ID_PATIENTS " +
                        "INNER JOIN TB_CLI_MEDICAL_CONSULTATION CMC ON CMC.ID_MEDICAL_CONSULTATION = CMCP.ID_MEDICAL_CONSULTATION " +
                        "WHERE CMCP.STATUS_CONSULTATION = 'Cancelada' " +
                        "AND CMCP.DATE_CANCELLATION IS NOT NULL " +
                        "ORDER BY CMCP.DATE_CONSULTATION_START DESC";

            DataTable dt = objDAL.RetDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                item = new AgendamentoModel(_httpContextAccessor)
                {
                    IdMedConsXPat = row["ID_MED_CONS_X_PAT"].ToString(),
                    LinkPhoto = row["LINK_PHOTO"].ToString(),
                    NamePatient = row["NAME_PATIENT"].ToString(),
                    CpfCnpj = row["CPF_CNPJ"].ToString(),
                    NameDoctor = row["NAME_DOCTOR"].ToString(),
                    CrmCro = row["CRM_CRO"].ToString(),
                    NameMedicalConsultation = row["DESC_MEDICAL_CONSULTATION"].ToString(),
                    DateConsultationStart = DateTime.Parse(row["DATE_CONSULTATION_START"].ToString()),
                    DateConsultationFinish = DateTime.Parse(row["DATE_CONSULTATION_FINISH"].ToString()),
                    DateCancellation = DateTime.Parse(row["DATE_CANCELLATION"].ToString()),
                    StatusConsultation = row["STATUS_CONSULTATION"].ToString(),
                    Reason = row["REASON"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }

        public void Gravar()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(IdMedConsXPat))
            {
                // Se for uma atualização, preenche o campo USER_UPDATE
                sql = "UPDATE TB_CLI_MED_CONSUL_X_PATIENT SET ID_PATIENTS = @IdPatients, " +
                        "ID_DOCTOR = @IdDoctor, " +
                        "ID_MEDICAL_CONSULTATION = @IdMedicalConsultation, " +
                        "DATE_CONSULTATION_START = @DateConsultationStart, " +
                        "DATE_CONSULTATION_FINISH = @DateConsultationFinish, " +
                        "STATUS_CONSULTATION = @StatusConsultation, " +
                        "REASON = @Reason, " +
                        "USER_UPDATE = @userUpdate, " +
                        "DATE_UPDATE = @dateUpdate " +
                        "WHERE ID_MED_CONS_X_PAT = @IdMedConsXPat " ;

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@IdPatients", IdPatients);
                command.Parameters.AddWithValue("@IdDoctor", IdDoctor);
                command.Parameters.AddWithValue("@IdMedicalConsultation", IdMedicalConsultation);
                command.Parameters.AddWithValue("@DateConsultationStart", DateConsultationStart);
                command.Parameters.AddWithValue("@DateConsultationFinish", DateConsultationFinish);
                command.Parameters.AddWithValue("@StatusConsultation", StatusConsultation);
                command.Parameters.AddWithValue("@Reason", Reason);
                command.Parameters.AddWithValue("@userUpdate", userId);
                command.Parameters.AddWithValue("@dateUpdate", currentDateTime);
                command.Parameters.AddWithValue("@IdMedConsXPat", IdMedConsXPat);

                objDAL.ExecutarComandoSQL(command);
            }
            else
            {
                // Se for uma inserção, preenche o campo USER_INSERT
                sql = "INSERT INTO TB_CLI_MED_CONSUL_X_PATIENT (ID_PATIENTS, ID_DOCTOR, ID_MEDICAL_CONSULTATION, DATE_CONSULTATION_START, DATE_CONSULTATION_FINISH, STATUS_CONSULTATION, REASON, USER_INSERT, DATE_INSERT) VALUES (@IdPatients, @IdDoctor, @IdMedicalConsultation, @DateConsultationStart, @DateConsultationFinish, @StatusConsultation, @Reason, @userInsert, @dateInsert)";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@IdPatients", IdPatients);
                command.Parameters.AddWithValue("@IdDoctor", IdDoctor);
                command.Parameters.AddWithValue("@IdMedicalConsultation", IdMedicalConsultation);
                command.Parameters.AddWithValue("@StatusConsultation", StatusConsultation);
                command.Parameters.AddWithValue("@DateConsultationStart", DateConsultationStart);
                command.Parameters.AddWithValue("@DateConsultationFinish", DateConsultationFinish);
                command.Parameters.AddWithValue("@Reason", Reason);
                command.Parameters.AddWithValue("@userInsert", userId);
                command.Parameters.AddWithValue("@dateInsert", currentDateTime);

                objDAL.ExecutarComandoSQL(command);
            }
        }

        public void GravarCancelamento()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(IdMedConsXPat))
            {
                // Se for uma atualização, preenche o campo USER_UPDATE
                sql = "UPDATE TB_CLI_MED_CONSUL_X_PATIENT SET DATE_CANCELLATION = @DateCancellation, " +
                        "STATUS_CONSULTATION = 'Cancelada', " +
                        "REASON = @Reason, " +
                        "USER_UPDATE = @userUpdate, " +
                        "DATE_UPDATE = @dateUpdate " +
                        "WHERE ID_MED_CONS_X_PAT = @IdMedConsXPat ";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@DateCancellation", DateCancellation);
                command.Parameters.AddWithValue("@Reason", Reason);
                command.Parameters.AddWithValue("@userUpdate", userId);
                command.Parameters.AddWithValue("@dateUpdate", currentDateTime);
                command.Parameters.AddWithValue("@IdMedConsXPat", IdMedConsXPat);

                objDAL.ExecutarComandoSQL(command);
            }
        }
    }
}
