using DentalPlus.Uteis;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DentalPlus.Models
{
    public class PlanoSaudeModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PlanoSaudeModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public PlanoSaudeModel()
        {

        }

        public string IdPlan {  get; set; }

        [Required(ErrorMessage = "Informe o nome do plano!")]
        public string NamePlan { get; set; }

        [Required(ErrorMessage = "Informe o operador do plano!")]
        public string Operator { get; set; }

        [Required(ErrorMessage = "Informe a cobertura do plano!")]
        public string Coverage { get; set; }

        [Required(ErrorMessage = "Informe o telefone do plano!")]
        public string Phone_number_1 { get; set; }

        public string Phone_number_2 { get; set; }

        [Required(ErrorMessage = "Informe o email referente ao plano!")]
        public string Email { get; set; }

        public string PlanoComCobertura
        {
            get
            {
                return $"{Operator} - ({Coverage})";
            }
        }

        public string userId { get; set; }

        public List<PlanoSaudeModel> ListarTodosPlanos()
        {
            List<PlanoSaudeModel> lista = new List<PlanoSaudeModel>();
            PlanoSaudeModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT ID_PLAN, NAME_PLAN, OPERATOR, COVERAGE, PHONE_NUMBER_1, PHONE_NUMBER_2, EMAIL FROM TB_CLI_DENTAL_PLAN " +
                         "ORDER BY NAME_PLAN ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                item = new PlanoSaudeModel(_httpContextAccessor)
                {
                    IdPlan = row["ID_PLAN"].ToString(),
                    NamePlan = row["NAME_PLAN"].ToString(),
                    Operator = row["OPERATOR"].ToString(),
                    Coverage = row["COVERAGE"].ToString(),
                    Phone_number_1 = row["PHONE_NUMBER_1"].ToString(),
                    Phone_number_2 = row["PHONE_NUMBER_2"].ToString(),
                    Email = row["EMAIL"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }

        public PlanoSaudeModel RetornarPlano(int? id)
        {

            PlanoSaudeModel item = null;
            DAL objDAL = new DAL();
            string sql = $"SELECT ID_PLAN, NAME_PLAN, OPERATOR, COVERAGE, PHONE_NUMBER_1, PHONE_NUMBER_2, EMAIL FROM TB_CLI_DENTAL_PLAN " +
                        $"WHERE ID_PLAN = '{id}' ORDER BY NAME_PLAN ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                item = new PlanoSaudeModel(_httpContextAccessor)
                {
                    IdPlan = dt.Rows[0]["ID_PLAN"].ToString(),
                    NamePlan = dt.Rows[0]["NAME_PLAN"].ToString(),
                    Operator = dt.Rows[0]["OPERATOR"].ToString(),
                    Coverage = dt.Rows[0]["COVERAGE"].ToString(),
                    Phone_number_1 = dt.Rows[0]["PHONE_NUMBER_1"].ToString(),
                    Phone_number_2 = dt.Rows[0]["PHONE_NUMBER_2"].ToString(),
                    Email = dt.Rows[0]["EMAIL"].ToString()
                };
            }

            return item;
        }

        public void Gravar()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(IdPlan))
            {
                // Se for uma atualização, preenche o campo USER_UPDATE
                sql = "UPDATE TB_CLI_DENTAL_PLAN SET NAME_PLAN = @namePlan, " +
                    "OPERATOR = @operator, " +
                    "COVERAGE = @coverage, " +
                    "PHONE_NUMBER_1 = @phoneNumber1, " +
                    "PHONE_NUMBER_2 = @phoneNumber2,  " +
                    "EMAIL = @email,  " +
                    "USER_UPDATE = @userUpdate, " +
                    "DATE_UPDATE = @dateUpdate " +
                    "WHERE ID_PLAN = @idPlan";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@namePlan", NamePlan);
                command.Parameters.AddWithValue("@operator", Operator);
                command.Parameters.AddWithValue("@coverage", Coverage);
                command.Parameters.AddWithValue("@phoneNumber1", Phone_number_1);
                command.Parameters.AddWithValue("@phoneNumber2", Phone_number_2);
                command.Parameters.AddWithValue("@email", Email);
                command.Parameters.AddWithValue("@userUpdate", userId);
                command.Parameters.AddWithValue("@dateUpdate", currentDateTime);
                command.Parameters.AddWithValue("@idPlan", IdPlan);

                objDAL.ExecutarComandoSQL(command);
            }
            else
            {

                // Se for uma inserção, preenche o campo USER_INSERT
                sql = "INSERT INTO TB_CLI_DENTAL_PLAN (NAME_PLAN, OPERATOR, COVERAGE, PHONE_NUMBER_1, PHONE_NUMBER_2, EMAIL, USER_INSERT, DATE_INSERT) " +
                                     "VALUES (@namePlan, @operator, @coverage, @phoneNumber1, @phoneNumber2, @email, @userInsert, @dateInsert)";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@namePlan", NamePlan);
                command.Parameters.AddWithValue("@operator", Operator);
                command.Parameters.AddWithValue("@coverage", Coverage);
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
            string sql = "DELETE FROM TB_CLI_DENTAL_PLAN WHERE ID_PLAN = @idPlan";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@idPlan", id);

            objDAL.ExecutarComandoSQL(command);
        }

    }
}
