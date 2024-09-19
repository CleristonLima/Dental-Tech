﻿using DentalPlus.Uteis;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
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
        
        public string CpfCnpj { get; set; }

        public string NamePatient { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Range(typeof(DateOnly), "1900-01-01", "3100-12-31", ErrorMessage = "A data de nascimento deve estar entre 01/01/1900 e 31/12/3100.")]
        public DateOnly BornDate { get; set; }

        public string ZipCode { get; set; }

        public string AddressPatient { get; set; }

        public string Number { get; set; }

        public string Complement { get; set; }

        public string District { get; set; }

        public string City { get; set; }

        public string IdUf { get; set; }

        public string CodeUf {  get; set; }

        public string PhoneNumber1 { get; set; }

        public string PhoneNumber2 { get; set; }

        public string Email { get; set; }

        public string IdPlan { get; set; }

        public string NamePlan { get; set; }

        public string Coverage { get; set; }

        public string CardNumberPlan { get; set; }

        public string userId { get; set; }

        public List<PacienteModel> ListarTodosPacientes()
        {
            List<PacienteModel> lista = new List<PacienteModel>();
            PacienteModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT CP.ID_PATIENTS, " +
                        "CP.CPF_CNPJ, " +
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
                        "CP.CARD_NUMBER_PLAN " +
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
                    CardNumberPlan = row["CARD_NUMBER_PLAN"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }

        public PacienteModel RetornarPaciente(int? id)
        {

            PacienteModel item = null;
            DAL objDAL = new DAL();
            string sql = $"SELECT CP.ID_PATIENTS, " +
                        $"CP.CPF_CNPJ, " +
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
                        $"CP.CARD_NUMBER_PLAN " +
                        $"FROM TB_CLI_PATIENTS CP " +
                        $"INNER JOIN TB_UF U ON U.ID_UF = CP.UF_ID " +
                        $"INNER JOIN TB_CLI_DENTAL_PLAN CDP ON CDP.ID_PLAN = CP.ID_PLAN " +
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
                        CardNumberPlan = dt.Rows[0]["CARD_NUMBER_PLAN"].ToString()
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
        public bool CPF_CNPJExiste(string crm_cro)
        {
            DAL objDAL = new DAL();
            string sql = $"SELECT COUNT(*) FROM TB_CLI_PATIENTS WHERE CPF_CNPJ = '{CpfCnpj}'";
            int count = Convert.ToInt32(objDAL.RetornarValor(sql));
            return count > 0;
        }

        public void Gravar()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (CPF_CNPJExiste(CpfCnpj))
            {
                throw new Exception("CPF ou CNPJ já existe cadastrado no sistema.");
            }

            if (!string.IsNullOrEmpty(IdPatients))
            {
                // Se for uma atualização, preenche o campo USER_UPDATE
                sql = "UPDATE TB_CLI_PATIENTS SET CPF_CNPJ = @CpfCnpj, " +
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
                    "EMAIL = @Email,  " +
                    "ID_PLAN = @IdPlan,  " +
                    "CARD_NUMBER_PLAN = @CardNumberPlan,  " +
                    "USER_UPDATE = @userUpdate, " +
                    "DATE_UPDATE = @dateUpdate " +
                    "WHERE ID_PATIENTS = @IdPatients";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@CpfCnpj", CpfCnpj);
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
                sql = "INSERT INTO TB_CLI_PATIENTS (CPF_CNPJ, NAME_PATIENT, BORN_DATE, ZIP_CODE, ADDRESS_PATIENT, NUMBER, COMPLEMENT, DISTRICT, CITY, UF_ID, PHONE_NUMBER_1, PHONE_NUMBER_2, EMAIL, ID_PLAN, CARD_NUMBER_PLAN, USER_INSERT, DATE_INSERT) " +
                                     "VALUES (@CpfCnpj, @NamePatient, @BornDate, @ZipCode, @AddressPatient, @Number, @Complement, @District, @City, @IdUf, @PhoneNumber1, @PhoneNumber2, @Email, @IdPlan, @CardNumberPlan, @userInsert, @dateInsert)";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@CpfCnpj", CpfCnpj);
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
                command.Parameters.AddWithValue("@userInsert", userId);
                command.Parameters.AddWithValue("@dateInsert", currentDateTime);
                command.Parameters.AddWithValue("@IdPatients", IdPatients);

                objDAL.ExecutarComandoSQL(command);
            }
        }
    }
}