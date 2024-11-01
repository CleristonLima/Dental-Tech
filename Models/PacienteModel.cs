using DentalPlus.Uteis;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Mysqlx.Expr;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Runtime.Intrinsics.Arm;

namespace DentalPlus.Models
{
    public class PacienteModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PacienteModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public PacienteModel()
        {

        }

        public string IdPatients { get; set; }

        [Required(ErrorMessage = "Informe o CPF ou CNPJ do paciente!")]
        public string CpfCnpj { get; set; }

        [Required(ErrorMessage = "Informe o RG ou RNE do paciente!")]
        public string RgRne { get; set; }

        [Required(ErrorMessage = "Informe o nome completo do paciente!")]
        public string NamePatient { get; set; }

        /*[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Range(typeof(DateOnly), "1900-01-01", "3100-12-31", ErrorMessage = "A data de nascimento deve estar entre 01/01/1900 e 31/12/3100.")]*/
        public DateOnly BornDate { get; set; }

        [Required(ErrorMessage = "Informe o CEP do paciente!")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Informe o endereço do paciente!")]
        public string AddressPatient { get; set; }

        [Required(ErrorMessage = "Informe o número do paciente!")]
        public string Number { get; set; }

        public string Complement { get; set; }

        [Required(ErrorMessage = "Informe o bairro do paciente!")]
        public string District { get; set; }

        [Required(ErrorMessage = "Informe a cidade do paciente!")]
        public string City { get; set; }

        [Required(ErrorMessage = "Informe o estado do paciente!")]
        public string IdUf { get; set; }

        public string CodeUf {  get; set; }

        [Required(ErrorMessage = "Informe o telefone do paciente!")]
        public string PhoneNumber1 { get; set; }

        public string PhoneNumber2 { get; set; }

        [Required(ErrorMessage = "Informe o email do paciente!")]
        public string Email { get; set; }

        public string IdPlan { get; set; }

        public string NamePlan { get; set; }

        public string Coverage { get; set; }

        public string CardNumberPlan { get; set; }

        public string LinkPhoto { get; set; }

        public string NomeComCpf
        {
            get
            {
                return $"{NamePatient} - ({CpfCnpj})";
            }
        }

        public string userId { get; set; }

        public List<PacienteModel> ListarTodosPacientes()
        {
            List<PacienteModel> lista = new List<PacienteModel>();
            PacienteModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT CP.ID_PATIENTS, " +
                        "CP.CPF_CNPJ, " +
                        "CP.RG_RNE, " +
                        "CP.NAME_PATIENT, " +
                        "CP.BORN_DATE, " +
                        "CP.ZIP_CODE, " +
                        "CP.ADDRESS_PATIENT, " +
                        "CP.NUMBER, " +
                        "CP.COMPLEMENT, " +
                        "CP.DISTRICT, " +
	                    "CP.CITY, " +
                        "U.CODE_UF, " +
                        "CP.PHONE_NUMBER_1, " +
                        "CP.PHONE_NUMBER_2, " +
                        "CP.EMAIL, " +
                        "CDP.NAME_PLAN, " +
                        "CDP.COVERAGE, " +
                        "CP.CARD_NUMBER_PLAN, " +
                        "CP.LINK_PHOTO " +
                        "FROM TB_CLI_PATIENTS CP " +
                        "INNER JOIN TB_UF U ON U.ID_UF = CP.UF_ID " +
                        "LEFT JOIN TB_CLI_DENTAL_PLAN CDP ON CDP.ID_PLAN = CP.ID_PLAN " +
                        "ORDER BY NAME_PATIENT ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                item = new PacienteModel(_httpContextAccessor)
                {
                    IdPatients = row["ID_PATIENTS"].ToString(),
                    CpfCnpj = row["CPF_CNPJ"].ToString(),
                    RgRne = row["RG_RNE"].ToString(),
                    NamePatient = row["NAME_PATIENT"].ToString(),
                    BornDate = DateOnly.FromDateTime(DateTime.Parse(row["BORN_DATE"].ToString())),
                    ZipCode = row["ZIP_CODE"].ToString(),
                    AddressPatient = row["ADDRESS_PATIENT"].ToString(),
                    Number = row["NUMBER"].ToString(),
                    Complement = row["COMPLEMENT"].ToString(),
                    District = row["DISTRICT"].ToString(),
                    City = row["CITY"].ToString(),
                    CodeUf = row["CODE_UF"].ToString(),
                    PhoneNumber1 = row["PHONE_NUMBER_1"].ToString(),
                    PhoneNumber2 = row["PHONE_NUMBER_2"].ToString(),
                    Email = row["EMAIL"].ToString(),
                    NamePlan = row["NAME_PLAN"].ToString(),
                    Coverage = row["COVERAGE"].ToString(),
                    CardNumberPlan = row["CARD_NUMBER_PLAN"].ToString(),
                    LinkPhoto = row["LINK_PHOTO"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }

        public List<PacienteModel> ListarTodosPacientesAniversariantes()
        {
            List<PacienteModel> lista = new List<PacienteModel>();
            PacienteModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT ID_PATIENTS, " +
                               "LINK_PHOTO, " +
	                           "NAME_PATIENT, " +
                               "BORN_DATE " +
                        "FROM TB_CLI_PATIENTS " +
                        "WHERE MONTH(BORN_DATE) = MONTH(CURDATE()) " +
                        "ORDER BY BORN_DATE ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                item = new PacienteModel(_httpContextAccessor)
                {
                    IdPatients = row["ID_PATIENTS"].ToString(),
                    LinkPhoto = row["LINK_PHOTO"].ToString(),
                    NamePatient = row["NAME_PATIENT"].ToString(),
                    BornDate = DateOnly.FromDateTime(DateTime.Parse(row["BORN_DATE"].ToString()))
                };

                lista.Add(item);
            }

            return lista;
        }

        /*public string ObterNomePorId(int? id)
        {
            if (id == null) return null;

            DAL objDAL = new DAL();
            string sql = "SELECT NAME_PATIENT FROM TB_CLI_PATIENTS WHERE ID_PATIENTS = @IdPatients";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdPatients", id);

            DataTable dt = objDAL.RetDataTable(command);

            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["NAME_PATIENT"].ToString();
            }

            return null; 
        }*/

        public PacienteModel RetornarPaciente(int? id)
        {

            PacienteModel item = null;
            DAL objDAL = new DAL();
            string sql = $"SELECT CP.ID_PATIENTS, " +
                        $"CP.CPF_CNPJ, " +
                        $"CP.RG_RNE, " +
                        $"CP.NAME_PATIENT, " +
                        $"CP.BORN_DATE, " +
                        $"CP.ZIP_CODE, " +
                        $"CP.ADDRESS_PATIENT, " +
                        $"CP.NUMBER, " +
                        $"CP.COMPLEMENT, " +
                        $"CP.DISTRICT, " +
                        $"CP.CITY, " +
                        $"CP.UF_ID, " +
                        $"CP.PHONE_NUMBER_1, " +
                        $"CP.PHONE_NUMBER_2, " +
                        $"CP.EMAIL, " +
                        $"CP.ID_PLAN, " +
                        $"CP.CARD_NUMBER_PLAN, " +
                        $"CP.LINK_PHOTO " +
                        $"FROM TB_CLI_PATIENTS CP " +
                        $"INNER JOIN TB_UF U ON U.ID_UF = CP.UF_ID " +
                        $"LEFT JOIN TB_CLI_DENTAL_PLAN CDP ON CDP.ID_PLAN = CP.ID_PLAN " +
                        $"WHERE CP.ID_PATIENTS = '{id}' ORDER BY CP.NAME_PATIENT ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                DateTime bornDate;

                if (DateTime.TryParse(dt.Rows[0]["BORN_DATE"].ToString(), out bornDate))
                {
                    item = new PacienteModel
                    {
                        IdPatients = dt.Rows[0]["ID_PATIENTS"].ToString(),
                        CpfCnpj = dt.Rows[0]["CPF_CNPJ"].ToString(),
                        RgRne = dt.Rows[0]["RG_RNE"].ToString(),
                        NamePatient = dt.Rows[0]["NAME_PATIENT"].ToString(),
                        BornDate = DateOnly.FromDateTime(DateTime.Parse(dt.Rows[0]["BORN_DATE"].ToString())),
                        ZipCode = dt.Rows[0]["ZIP_CODE"].ToString(),
                        AddressPatient = dt.Rows[0]["ADDRESS_PATIENT"].ToString(),
                        Number = dt.Rows[0]["NUMBER"].ToString(),
                        Complement = dt.Rows[0]["COMPLEMENT"].ToString(),
                        District = dt.Rows[0]["DISTRICT"].ToString(),
                        City = dt.Rows[0]["CITY"].ToString(),
                        IdUf = dt.Rows[0]["UF_ID"].ToString(),
                        PhoneNumber1 = dt.Rows[0]["PHONE_NUMBER_1"].ToString(),
                        PhoneNumber2 = dt.Rows[0]["PHONE_NUMBER_2"].ToString(),
                        Email = dt.Rows[0]["EMAIL"].ToString(),
                        IdPlan = dt.Rows[0]["ID_PLAN"].ToString(),
                        CardNumberPlan = dt.Rows[0]["CARD_NUMBER_PLAN"].ToString(),
                        LinkPhoto = dt.Rows[0]["LINK_PHOTO"].ToString()
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
        public bool CPF_CNPJExiste(string CpfCnpj, string idPatients = null)
        {
            DAL objDAL = new DAL();
            string sql = "SELECT COUNT(*) FROM TB_CLI_PATIENTS WHERE CPF_CNPJ = @CpfCnpj";

            if (!string.IsNullOrEmpty(idPatients))
            {
                sql += " AND ID_PATIENTS != @IdPatients";
            }

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@CpfCnpj", CpfCnpj);

            if (!string.IsNullOrEmpty(idPatients))
            {
                command.Parameters.AddWithValue("@IdPatients", idPatients);
            }

            int count = Convert.ToInt32(objDAL.RetDataTable(command).Rows[0][0]);
            return count > 0;
        }

        public void Gravar()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(IdPatients))
            {
                // Verifica duplicidade excluindo o registro atual
                if (CPF_CNPJExiste(CpfCnpj, IdPatients))
                {
                    throw new Exception("CPF ou CNPJ já existe cadastrado no sistema.");
                }
                // Se for uma atualização, preenche o campo USER_UPDATE
                sql = "UPDATE TB_CLI_PATIENTS SET CPF_CNPJ = @CpfCnpj, " +
                    "RG_RNE = @RgRne, " +
                    "NAME_PATIENT = @NamePatient, " +
                    "BORN_DATE = @BornDate, " +
                    "ZIP_CODE = @ZipCode, " +
                    "ADDRESS_PATIENT = @AddressPatient, " +
                    "NUMBER = @Number, " +
                    "COMPLEMENT = @Complement, " +
                    "DISTRICT = @District, " +
                    "CITY = @City, " +
                    "UF_ID = @IdUf, " +
                    "PHONE_NUMBER_1 = @PhoneNumber1, " +
                    "PHONE_NUMBER_2 = @PhoneNumber2, " +
                    "EMAIL = @Email, " +
                    "ID_PLAN = @IdPlan, " +
                    "CARD_NUMBER_PLAN = @CardNumberPlan,  " +
                    "LINK_PHOTO = @LinkPhoto, " +
                    "USER_UPDATE = @userUpdate, " +
                    "DATE_UPDATE = @dateUpdate " +
                    "WHERE ID_PATIENTS = @IdPatients";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@CpfCnpj", CpfCnpj);
                command.Parameters.AddWithValue("@RgRne", RgRne);
                command.Parameters.AddWithValue("@NamePatient", NamePatient);
                command.Parameters.AddWithValue("@BornDate", BornDate.ToDateTime(TimeOnly.MinValue));
                command.Parameters.AddWithValue("@ZipCode", ZipCode);
                command.Parameters.AddWithValue("@AddressPatient", AddressPatient);
                command.Parameters.AddWithValue("@Number", Number);
                command.Parameters.AddWithValue("@Complement", Complement);
                command.Parameters.AddWithValue("@District", District);
                command.Parameters.AddWithValue("@City", City);
                command.Parameters.AddWithValue("@IdUf", IdUf);
                command.Parameters.AddWithValue("@PhoneNumber1", PhoneNumber1);
                command.Parameters.AddWithValue("@PhoneNumber2", PhoneNumber2);
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@IdPlan", IdPlan);
                command.Parameters.AddWithValue("@CardNumberPlan", CardNumberPlan);
                command.Parameters.AddWithValue("@LinkPhoto", LinkPhoto);
                command.Parameters.AddWithValue("@userUpdate", userId);
                command.Parameters.AddWithValue("@dateUpdate", currentDateTime);
                command.Parameters.AddWithValue("@IdPatients", IdPatients);

                objDAL.ExecutarComandoSQL(command);
            }
            else
            {
                if (CPF_CNPJExiste(CpfCnpj))
                {
                    throw new Exception("CPF ou CNPJ já existe cadastrado no sistema.");
                }

                // Se for uma inserção, preenche o campo USER_INSERT
                sql = "INSERT INTO TB_CLI_PATIENTS (CPF_CNPJ, RG_RNE, NAME_PATIENT, BORN_DATE, ZIP_CODE, ADDRESS_PATIENT, NUMBER, COMPLEMENT, DISTRICT, CITY, UF_ID, PHONE_NUMBER_1, PHONE_NUMBER_2, EMAIL, ID_PLAN, CARD_NUMBER_PLAN, LINK_PHOTO, USER_INSERT, DATE_INSERT) " +
                                     "VALUES (@CpfCnpj, @RgRne, @NamePatient, @BornDate, @ZipCode, @AddressPatient, @Number, @Complement, @District, @City, @IdUf, @PhoneNumber1, @PhoneNumber2, @Email, @IdPlan, @CardNumberPlan, @LinkPhoto, @userInsert, @dateInsert)";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@CpfCnpj", CpfCnpj);
                command.Parameters.AddWithValue("@RgRne", RgRne);
                command.Parameters.AddWithValue("@NamePatient", NamePatient);
                command.Parameters.AddWithValue("@BornDate", BornDate.ToDateTime(TimeOnly.MinValue));
                command.Parameters.AddWithValue("@ZipCode", ZipCode);
                command.Parameters.AddWithValue("@AddressPatient", AddressPatient);
                command.Parameters.AddWithValue("@Number", Number);
                command.Parameters.AddWithValue("@Complement", Complement);
                command.Parameters.AddWithValue("@District", District);
                command.Parameters.AddWithValue("@City", City);
                command.Parameters.AddWithValue("@IdUf", IdUf);
                command.Parameters.AddWithValue("@PhoneNumber1", PhoneNumber1);
                command.Parameters.AddWithValue("@PhoneNumber2", PhoneNumber2);
                command.Parameters.AddWithValue("@Email", Email);
                command.Parameters.AddWithValue("@IdPlan", IdPlan);
                command.Parameters.AddWithValue("@CardNumberPlan", CardNumberPlan);
                command.Parameters.AddWithValue("@LinkPhoto", LinkPhoto);
                command.Parameters.AddWithValue("@userInsert", userId);
                command.Parameters.AddWithValue("@dateInsert", currentDateTime);
                command.Parameters.AddWithValue("@IdPatients", IdPatients);

                objDAL.ExecutarComandoSQL(command);
            }
        }

        public void Excluir(int id)
        {
            DAL objDAL = new DAL();
            string sql = "DELETE FROM TB_CLI_PATIENTS WHERE ID_PATIENTS = @IdPatients";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdPatients", id);

            objDAL.ExecutarComandoSQL(command);
        }
    }
}
