using DentalPlus.Uteis;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DentalPlus.Models
{
    public class LoginModel
    {
        public string Id { get; set; }

        public string NamePeople { get; set; }

        [Required(ErrorMessage = "Informe o login do usuário!")]
        public string LoginUser {  get; set; }

        [Required(ErrorMessage = "Informe a senha do usuário!")]
        public string PassUser { get; set; }

        public string ErrorMessage { get; set; }

        public bool ValidarLogin()
        {
            string sql = "SELECT ID_LOGIN, NAME_PEOPLE, BLOCK_USER FROM TB_AD_LOGIN WHERE LOGIN_NAME = @loginUser AND PASSWORD_LOGIN = @senhaUser";

            MySqlCommand Command = new MySqlCommand(sql);
            Command.Parameters.AddWithValue("@loginUser", LoginUser);
            Command.Parameters.AddWithValue("@senhaUser", PassUser);

            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(Command);

            if (dt.Rows.Count == 1)
            {
                // Verifica se o usuário está bloqueado
                if (Convert.ToInt32(dt.Rows[0]["BLOCK_USER"]) == 1)
                {
                    ErrorMessage = "Usuário bloqueado. Entre em contato com o administrador.";
                    return false;
                }

                // Ira gravar o login e senha do usuário
                Id = dt.Rows[0]["ID_LOGIN"].ToString();
                NamePeople = dt.Rows[0]["NAME_PEOPLE"].ToString();

                return true;

            }
            else
            {
                ErrorMessage = "Login ou senha inválidos!";

                return false;
            }
        }
    }
}
