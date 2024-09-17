using DentalPlus.Uteis;
using MySql.Data.MySqlClient;
using Mysqlx.Expr;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DentalPlus.Models
{
    public class TipoConsultaModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TipoConsultaModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public TipoConsultaModel()
        {

        }

        public string IdMedicalConsultation { get; set; }

        [Required(ErrorMessage = "Informe o tipo da consulta!")]
        public string descMedicalConsultation { get; set; }

        [Required(ErrorMessage = "Informe o valor da consulta!")]
        public decimal valueConsultation { get; set; }

        public string userId { get; set; }

        public List<TipoConsultaModel> ListarTodosTiposConsultas()
        {
            List<TipoConsultaModel> lista = new List<TipoConsultaModel>();
            TipoConsultaModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT ID_MEDICAL_CONSULTATION, DESC_MEDICAL_CONSULTATION, VALUE_CONSULTATION FROM TB_CLI_MEDICAL_CONSULTATION " +
                         "ORDER BY DESC_MEDICAL_CONSULTATION ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                item = new TipoConsultaModel(_httpContextAccessor)
                {
                    IdMedicalConsultation = row["ID_MEDICAL_CONSULTATION"].ToString(),
                    descMedicalConsultation = row["DESC_MEDICAL_CONSULTATION"].ToString(),
                    valueConsultation = Convert.ToDecimal(row["VALUE_CONSULTATION"].ToString())
                };

                lista.Add(item);
            }

            return lista;
        }

        public TipoConsultaModel RetornarTipoConsulta(int? id)
        {

            TipoConsultaModel item = null;
            DAL objDAL = new DAL();
            string sql = $"SELECT ID_MEDICAL_CONSULTATION, DESC_MEDICAL_CONSULTATION, VALUE_CONSULTATION FROM TB_CLI_MEDICAL_CONSULTATION " +
                        $"WHERE ID_MEDICAL_CONSULTATION = '{id}' ORDER BY DESC_MEDICAL_CONSULTATION ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                item = new TipoConsultaModel(_httpContextAccessor)
                {
                    IdMedicalConsultation = dt.Rows[0]["ID_MEDICAL_CONSULTATION"].ToString(),
                    descMedicalConsultation = dt.Rows[0]["DESC_MEDICAL_CONSULTATION"].ToString(),
                    valueConsultation = Convert.ToDecimal(dt.Rows[0]["VALUE_CONSULTATION"].ToString())
                };
            }

            return item;
        }

        public void Gravar()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(IdMedicalConsultation))
            {
                // Se for uma atualização, preenche o campo USER_UPDATE
                sql = "UPDATE TB_CLI_MEDICAL_CONSULTATION SET DESC_MEDICAL_CONSULTATION = @descMedicalConsultation, " +
                    "VALUE_CONSULTATION = @valueConsultation, " +
                    "USER_UPDATE = @userUpdate, " +
                    "DATE_UPDATE = @dateUpdate " +
                    "WHERE ID_MEDICAL_CONSULTATION = @idMedicalConsultation";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@descMedicalConsultation", descMedicalConsultation);
                command.Parameters.AddWithValue("@valueConsultation", valueConsultation);
                command.Parameters.AddWithValue("@userUpdate", userId);
                command.Parameters.AddWithValue("@dateUpdate", currentDateTime);
                command.Parameters.AddWithValue("@idMedicalConsultation", IdMedicalConsultation);

                objDAL.ExecutarComandoSQL(command);
            }
            else
            {

                // Se for uma inserção, preenche o campo USER_INSERT
                sql = "INSERT INTO TB_CLI_MEDICAL_CONSULTATION (DESC_MEDICAL_CONSULTATION, VALUE_CONSULTATION, USER_INSERT, DATE_INSERT) " +
                                     "VALUES (@descMedicalConsultation, @valueConsultation, @userInsert, @dateInsert)";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@descMedicalConsultation", descMedicalConsultation);
                command.Parameters.AddWithValue("@valueConsultation", valueConsultation);
                command.Parameters.AddWithValue("@userInsert", userId);
                command.Parameters.AddWithValue("@dateInsert", currentDateTime);

                objDAL.ExecutarComandoSQL(command);
            }
        }

        public void Excluir(int id)
        {
            DAL objDAL = new DAL();
            string sql = "DELETE FROM TB_CLI_MEDICAL_CONSULTATION WHERE ID_MEDICAL_CONSULTATION = @idMedicalConsultation";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@idMedicalConsultation", id);

            objDAL.ExecutarComandoSQL(command);
        }
    }
}
