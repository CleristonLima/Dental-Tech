using DentalPlus.Uteis;
using MySql.Data.MySqlClient;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Web;

namespace DentalPlus.Models
{
    public class PerfilModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PerfilModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public PerfilModel()
        {

        }

        public string IdProfile { get; set; }

        [Required(ErrorMessage = "Informe a descricao do perfil")]
        public string NameProfile { get; set; }

        [Required(ErrorMessage = "Informe a sigla do perfil")]
        public string CodeProfile { get; set; }

        public string userId { get; set; }

        public string NomeComSigla
        {
            get
            {
                return $"{NameProfile} - ({CodeProfile})";
            }
        }

        public List<PerfilModel> ListarTodosPerfis()
        {
            List<PerfilModel> lista = new List<PerfilModel>();
            PerfilModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT ID_PROFILE, NAME_PROFILE, CODE_PROFILE FROM TB_AD_PROFILE ORDER BY NAME_PROFILE ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new PerfilModel(_httpContextAccessor)
                {
                    IdProfile = dt.Rows[i]["ID_PROFILE"].ToString(),
                    NameProfile = dt.Rows[i]["NAME_PROFILE"].ToString(),
                    CodeProfile = dt.Rows[i]["CODE_PROFILE"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }

        public PerfilModel RetornarPerfil(int? id)
        {

            PerfilModel item;
            DAL objDAL = new DAL();
            string sql = $"SELECT ID_PROFILE, NAME_PROFILE, CODE_PROFILE FROM TB_AD_PROFILE WHERE ID_PROFILE = '{id}' ORDER BY NAME_PROFILE ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            item = new PerfilModel(_httpContextAccessor)
            {
                IdProfile = dt.Rows[0]["ID_PROFILE"].ToString(),
                NameProfile = dt.Rows[0]["NAME_PROFILE"].ToString(),
                CodeProfile = dt.Rows[0]["CODE_PROFILE"].ToString()
            };

            return item;
        }
        // Insert ou Update

        public void Gravar()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(IdProfile))
            {
                // Se for uma atualização, preenche o campo USER_UPDATE
                sql = "UPDATE TB_AD_PROFILE SET NAME_PROFILE = @nameProfile, CODE_PROFILE = @codeProfile, USER_UPDATE = @userUpdate, DATE_UPDATE = @dateUpdate WHERE ID_PROFILE = @idProfile";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@nameProfile", NameProfile);
                command.Parameters.AddWithValue("@codeProfile", CodeProfile);
                command.Parameters.AddWithValue("@userUpdate", userId);
                command.Parameters.AddWithValue("@dateUpdate", currentDateTime);
                command.Parameters.AddWithValue("@idProfile", IdProfile);

                objDAL.ExecutarComandoSQL(command);
            }
            else
            {
                // Se for uma inserção, preenche o campo USER_INSERT
                sql = "INSERT INTO TB_AD_PROFILE (NAME_PROFILE, CODE_PROFILE, USER_INSERT, DATE_INSERT) VALUES (@nameProfile, @codeProfile, @userInsert, @dateInsert)";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@nameProfile", NameProfile);
                command.Parameters.AddWithValue("@codeProfile", CodeProfile);
                command.Parameters.AddWithValue("@userInsert", userId);
                command.Parameters.AddWithValue("@dateInsert", currentDateTime);

                objDAL.ExecutarComandoSQL(command);
            }
        }

        // Método para excluir um perfil do banco de dados
        public void Excluir(int id)
        {
            DAL objDAL = new DAL();
            string sql = "DELETE FROM TB_AD_PROFILE WHERE ID_PROFILE = @idProfile";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@idProfile", id);

            objDAL.ExecutarComandoSQL(command);
        }
    }
}