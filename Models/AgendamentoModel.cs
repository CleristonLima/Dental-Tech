using DentalPlus.Uteis;
using Mysqlx.Crud;
using System.Data;

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

        public string NamePatient { get; set; }

        public string IdDoctor {  get; set; }

        public string NameDoctor { get; set; }

        public string IdMedicalConsultation { get; set; }

        public string NameMedicalConsultation { get; set; }

        public DateTime DateConsultationStart { get; set; }

        public DateTime DateConsultationFinish { get; set; }

        public List<AgendamentoModel> ListarTodosAgendamentos()
        {
            List<AgendamentoModel> lista = new List<AgendamentoModel>();
            AgendamentoModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT CMCP.ID_MED_CONS_X_PAT, " +
                               "CP.NAME_PATIENT, " +
                               "CD.NAME_DOCTOR, " +
                               "CMC.DESC_MEDICAL_CONSULTATION, " +
                               "CMCP.DATE_CONSULTATION_START, " +
                               "CMCP.DATE_CONSULTATION_FINISH " +
                        "FROM TB_CLI_MED_CONSUL_X_PATIENT CMCP " +
                        "INNER JOIN TB_CLI_DOCTORS CD ON CD.ID_DOCTOR = CMCP.ID_DOCTOR " +
                        "INNER JOIN TB_CLI_PATIENTS CP ON CP.ID_PATIENTS = CMCP.ID_PATIENTS " +
                        "INNER JOIN TB_CLI_MEDICAL_CONSULTATION CMC ON CMC.ID_MEDICAL_CONSULTATION = CMCP.ID_MEDICAL_CONSULTATION " +
                        "ORDER BY CMCP.DATE_CONSULTATION_START DESC";
            DataTable dt = objDAL.RetDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                item = new AgendamentoModel(_httpContextAccessor)
                {
                    IdMedConsXPat = row["ID_MED_CONS_X_PAT"].ToString(),
                    NamePatient = row["NAME_PATIENT"].ToString(),
                    NameDoctor = row["NAME_DOCTOR"].ToString(),
                    NameMedicalConsultation = row["DESC_MEDICAL_CONSULTATION"].ToString(),
                    DateConsultationStart = DateTime.Parse(row["DATE_CONSULTATION_START"].ToString()),
                    DateConsultationFinish = DateTime.Parse(row["DATE_CONSULTATION_FINISH"].ToString())
                };

                lista.Add(item);
            }

            return lista;
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
                               $"CMCP.DATE_CONSULTATION_FINISH " +
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

                if (DateTime.TryParse(dt.Rows[0]["DATE_CONSULTATION_START"].ToString(), out dateConsultationStart) && DateTime.TryParse(dt.Rows[0]["DATE_CONSULTATION_FINISH"].ToString(), out dateConsultationStart))
                {
                    item = new AgendamentoModel
                    {
                        IdMedConsXPat = dt.Rows[0]["ID_MED_CONS_X_PAT"].ToString(),
                        IdPatients = dt.Rows[0]["ID_PATIENTS"].ToString(),
                        IdDoctor = dt.Rows[0]["ID_DOCTOR"].ToString(),
                        IdMedicalConsultation = dt.Rows[0]["ID_MEDICAL_CONSULTATION"].ToString(),
                        DateConsultationStart = DateTime.Parse(dt.Rows[0]["DATE_CONSULTATION_START"].ToString()),
                        DateConsultationFinish = DateTime.Parse(dt.Rows[0]["DATE_CONSULTATION_FINISH"].ToString())
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
    }
}
