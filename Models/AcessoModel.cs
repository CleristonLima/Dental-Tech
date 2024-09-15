using DentalPlus.Uteis;
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

        public string Id { get; set; }

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
                    Id = row["ID_LOGIN"].ToString(),
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

    }
}
