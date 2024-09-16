using DentalPlus.Uteis;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DentalPlus.Models
{
    public class AcessoModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AcessoModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public AcessoModel()
        {

        }

        public string IdLogin { get; set; }

        [Required(ErrorMessage = "Informe o nome completo do usuário!")]
        public string NamePeople { get; set; }

        [Required(ErrorMessage = "Informe o login do usuário!")]
        public string LoginUser { get; set; }

        [Required(ErrorMessage = "Informe a senha do usuário!")]
        public string PassUser { get; set; }

        [Required(ErrorMessage = "Informe o email do usuário!")]
        public string Email { get; set; }

        public bool BlockUser { get; set; }

        [Required(ErrorMessage = "Informe o perfil do usuário!")]
        public string IdProfile { get; set; }

        public string NameProfile { get; set; }

        public string userId { get; set; }

        public List<AcessoModel> ListarTodosAcessos()
        {
            List<AcessoModel> lista = new List<AcessoModel>();
            AcessoModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT DISTINCT AL.ID_LOGIN, AL.NAME_PEOPLE, AL.EMAIL, AL.LOGIN_NAME, AL.BLOCK_USER, AP.NAME_PROFILE FROM TB_AD_LOGIN AL " +
                            "INNER JOIN TB_AD_PROFILE AP " +
                            "ON AP.ID_PROFILE = AL.ID_PROFILE " +
                         "ORDER BY NAME_PROFILE ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                item = new AcessoModel(_httpContextAccessor)
                {
                    IdLogin = row["ID_LOGIN"].ToString(),
                    NamePeople = row["NAME_PEOPLE"].ToString(),
                    Email = row["EMAIL"].ToString(),
                    LoginUser = row["LOGIN_NAME"].ToString(),
                    BlockUser = Convert.ToBoolean(row["BLOCK_USER"]),
                    NameProfile = row["NAME_PROFILE"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }

        public AcessoModel RetornarAcesso(int? id)
        {

            AcessoModel item = null;
            DAL objDAL = new DAL();
            string sql = $"SELECT AL.ID_LOGIN, AL.NAME_PEOPLE, AL.EMAIL, AL.LOGIN_NAME, AL.PASSWORD_LOGIN, AL.BLOCK_USER, AP.ID_PROFILE FROM TB_AD_LOGIN AL " +
                            "INNER JOIN TB_AD_PROFILE AP " +
                            "ON AP.ID_PROFILE = AL.ID_PROFILE " +
                        $"WHERE ID_LOGIN = '{id}' ORDER BY NAME_PEOPLE ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                item = new AcessoModel(_httpContextAccessor)
                {
                    IdLogin = dt.Rows[0]["ID_LOGIN"].ToString(),
                    NamePeople = dt.Rows[0]["NAME_PEOPLE"].ToString(),
                    LoginUser = dt.Rows[0]["LOGIN_NAME"].ToString(),
                    PassUser = dt.Rows[0]["PASSWORD_LOGIN"].ToString(),
                    Email = dt.Rows[0]["EMAIL"].ToString(),
                    BlockUser = Convert.ToBoolean(dt.Rows[0]["BLOCK_USER"]),
                    IdProfile = dt.Rows[0]["ID_PROFILE"].ToString()
                };
            }

            return item;
        }

        public void Gravar()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(IdLogin))
            {
                // Se for uma atualização, preenche o campo USER_UPDATE
                sql = "UPDATE TB_AD_LOGIN SET NAME_PEOPLE = @namePeople, " +
                    "LOGIN_NAME = @loginName, " +
                    "PASSWORD_LOGIN = @passLogin, " +
                    "EMAIL = @email, " +
                    "BLOCK_USER = @blockUser,  " +
                    "ID_PROFILE = @idProfile,  " +
                    "USER_UPDATE = @userUpdate, " +
                    "DATE_UPDATE = @dateUpdate " +
                    "WHERE ID_LOGIN = @idLogin";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@namePeople", NamePeople);
                command.Parameters.AddWithValue("@loginName", LoginUser);
                command.Parameters.AddWithValue("@passLogin", PassUser);
                command.Parameters.AddWithValue("@email", Email);
                command.Parameters.AddWithValue("@blockUser", BlockUser);
                command.Parameters.AddWithValue("@idProfile", IdProfile);
                command.Parameters.AddWithValue("@userUpdate", userId);
                command.Parameters.AddWithValue("@dateUpdate", currentDateTime);
                command.Parameters.AddWithValue("@idLogin", IdLogin);

                objDAL.ExecutarComandoSQL(command);
            }
            else
            {
                // Se for uma inserção, preenche o campo USER_INSERT
                sql = "INSERT INTO TB_AD_LOGIN (NAME_PEOPLE, LOGIN_NAME, PASSWORD_LOGIN, EMAIL, BLOCK_USER, ID_PROFILE, USER_INSERT, DATE_INSERT) " +
                                     "VALUES (@namePeople, @loginName, @passLogin, @email, @blockUser, @idProfile, @userInsert, @dateInsert)";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@namePeople", NamePeople);
                command.Parameters.AddWithValue("@loginName", LoginUser);
                command.Parameters.AddWithValue("@passLogin", PassUser);
                command.Parameters.AddWithValue("@email", Email);
                command.Parameters.AddWithValue("@blockUser", BlockUser);
                command.Parameters.AddWithValue("@idProfile", IdProfile);
                command.Parameters.AddWithValue("@userInsert", userId);
                command.Parameters.AddWithValue("@dateInsert", currentDateTime);

                objDAL.ExecutarComandoSQL(command);
            }
        }

    }
}
