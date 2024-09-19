using DentalPlus.Uteis;
using System.Data;

namespace DentalPlus.Models
{
    public class EstadoModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EstadoModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public EstadoModel()
        {

        }

        public string IdUf { get; set; }

        public string codeUf { get; set; }

        public string descUf { get; set; }

        public List<EstadoModel> ListarTodosEstados()
        {
            List<EstadoModel> lista = new List<EstadoModel>();
            EstadoModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT ID_UF, CODE_UF, DESC_UF FROM TB_UF ";
            DataTable dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new EstadoModel(_httpContextAccessor)
                {
                    IdUf = dt.Rows[i]["ID_UF"].ToString(),
                    codeUf = dt.Rows[i]["CODE_UF"].ToString(),
                    descUf = dt.Rows[i]["DESC_UF"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}
