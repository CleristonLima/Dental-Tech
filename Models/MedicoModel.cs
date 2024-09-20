using DentalPlus.Uteis;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DentalPlus.Models
{
    public class MedicoModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MedicoModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public MedicoModel()
        {

        }
        public string IdDoctor { get; set; }

        [Required(ErrorMessage = "Informe o nome completo do médico!")]
        public string NameDoctor { get; set; }

        [Required(ErrorMessage = "Informe o CRM/CRO!")]
        public string CRM_CRO { get; set; }

        [Required(ErrorMessage = "Informe a especialidade!")]
        public string Speciality { get; set; }

        [Required(ErrorMessage = "Informe o numero de telefone")]
        public string Phone_number_1 { get; set; }

        public string Phone_number_2 { get; set; }

        [Required(ErrorMessage = "Informe o Email!")]
        public string Email { get; set; }

        public string userId { get; set; }

        public List<MedicoModel> ListarTodosMedicos()
        {
            List<MedicoModel> lista = new List<MedicoModel>();
            MedicoModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT ID_DOCTOR, NAME_DOCTOR, CRM_CRO, SPECIALITY, PHONE_NUMBER_1, PHONE_NUMBER_2, EMAIL FROM TB_CLI_DOCTORS " +
                         "ORDER BY NAME_DOCTOR ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                item = new MedicoModel(_httpContextAccessor)
                {
                    IdDoctor = row["ID_DOCTOR"].ToString(),
                    NameDoctor = row["NAME_DOCTOR"].ToString(),
                    CRM_CRO = row["CRM_CRO"].ToString(),
                    Speciality = row["SPECIALITY"].ToString(),
                    Phone_number_1 = row["PHONE_NUMBER_1"].ToString(),
                    Phone_number_2 = row["PHONE_NUMBER_2"].ToString(),
                    Email = row["EMAIL"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }

        public MedicoModel RetornarMedico(int? id)
        {

            MedicoModel item = null;
            DAL objDAL = new DAL();
            string sql = $"SELECT ID_DOCTOR, NAME_DOCTOR, CRM_CRO, SPECIALITY, PHONE_NUMBER_1, PHONE_NUMBER_2, EMAIL FROM TB_CLI_DOCTORS " +
                        $"WHERE ID_DOCTOR = '{id}' ORDER BY NAME_DOCTOR ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                item = new MedicoModel(_httpContextAccessor)
                {
                    IdDoctor = dt.Rows[0]["ID_DOCTOR"].ToString(),
                    NameDoctor = dt.Rows[0]["NAME_DOCTOR"].ToString(),
                    CRM_CRO = dt.Rows[0]["CRM_CRO"].ToString(),
                    Speciality = dt.Rows[0]["SPECIALITY"].ToString(),
                    Phone_number_1 = dt.Rows[0]["PHONE_NUMBER_1"].ToString(),
                    Phone_number_2 = dt.Rows[0]["PHONE_NUMBER_2"].ToString(),
                    Email = dt.Rows[0]["EMAIL"].ToString()
                };
            }

            return item;
        }

        public bool CRM_CROExiste(string crm_cro, string idDoctor = null)
        {
            DAL objDAL = new DAL();
            string sql = "SELECT COUNT(*) FROM TB_CLI_DOCTORS WHERE CRM_CRO = @CrmCro";

            if (!string.IsNullOrEmpty(idDoctor))
            {
                sql += " AND ID_DOCTOR != @IdDoctor";
            }

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@CrmCro", crm_cro);

            if (!string.IsNullOrEmpty(idDoctor))
            {
                command.Parameters.AddWithValue("@IdDoctor", idDoctor);
            }

            int count = Convert.ToInt32(objDAL.RetDataTable(command).Rows[0][0]);
            return count > 0;
        }

        public void Gravar()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(IdDoctor))
            {
                // Verifica duplicidade excluindo o registro atual
                if (CRM_CROExiste(CRM_CRO, IdDoctor))
                {
                    throw new Exception("CPF ou CNPJ já existe cadastrado no sistema.");
                }
                // Se for uma atualização, preenche o campo USER_UPDATE
                sql = "UPDATE TB_CLI_DOCTORS SET NAME_DOCTOR = @nameDoctor, " +
                    "CRM_CRO = @crmCro, " +
                    "SPECIALITY = @speciality, " +
                    "PHONE_NUMBER_1 = @phoneNumber1, " +
                    "PHONE_NUMBER_2 = @phoneNumber2,  " +
                    "EMAIL = @email,  " +
                    "USER_UPDATE = @userUpdate, " +
                    "DATE_UPDATE = @dateUpdate " +
                    "WHERE ID_DOCTOR = @idDoctor";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@nameDoctor", NameDoctor);
                command.Parameters.AddWithValue("@crmCro", CRM_CRO);
                command.Parameters.AddWithValue("@speciality", Speciality);
                command.Parameters.AddWithValue("@phoneNumber1", Phone_number_1);
                command.Parameters.AddWithValue("@phoneNumber2", Phone_number_2);
                command.Parameters.AddWithValue("@email", Email);
                command.Parameters.AddWithValue("@userUpdate", userId);
                command.Parameters.AddWithValue("@dateUpdate", currentDateTime);
                command.Parameters.AddWithValue("@idDoctor", IdDoctor);

                objDAL.ExecutarComandoSQL(command);
            }
            else
            {
                if (CRM_CROExiste(CRM_CRO))
                {
                    throw new Exception("CRM/CRO já existe cadastrado no sistema.");
                }

                // Se for uma inserção, preenche o campo USER_INSERT
                sql = "INSERT INTO TB_CLI_DOCTORS (NAME_DOCTOR, CRM_CRO, SPECIALITY, PHONE_NUMBER_1, PHONE_NUMBER_2, EMAIL, USER_INSERT, DATE_INSERT) " +
                                     "VALUES (@nameDoctor, @crmCro, @speciality, @phoneNumber1, @phoneNumber2, @email, @userInsert, @dateInsert)";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@nameDoctor", NameDoctor);
                command.Parameters.AddWithValue("@crmCro", CRM_CRO);
                command.Parameters.AddWithValue("@speciality", Speciality);
                command.Parameters.AddWithValue("@phoneNumber1", Phone_number_1);
                command.Parameters.AddWithValue("@phoneNumber2", Phone_number_2);
                command.Parameters.AddWithValue("@email", Email);
                command.Parameters.AddWithValue("@userInsert", userId);
                command.Parameters.AddWithValue("@dateInsert", currentDateTime);

                objDAL.ExecutarComandoSQL(command);
            }
        }

        public void Excluir(int id)
        {
            DAL objDAL = new DAL();
            string sql = "DELETE FROM TB_CLI_DOCTORS WHERE ID_DOCTOR = @idDoctor";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@idDoctor", id);

            objDAL.ExecutarComandoSQL(command);
        }

    }
}
