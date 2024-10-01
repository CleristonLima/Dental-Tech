using DentalPlus.Uteis;
using System.Data;

namespace DentalPlus.Models
{
    public class PagamentoModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PagamentoModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public PagamentoModel()
        {

        }

        public string IdPayment { get; set; }

        public string DescPayment { get; set; }

        public List<PagamentoModel> ListarTodosPagamentos()
        {
            List<PagamentoModel> lista = new List<PagamentoModel>();
            PagamentoModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT ID_PAYMENT, DESC_PAYMENT FROM TB_MB_PAYMENTS ";
            DataTable dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new PagamentoModel(_httpContextAccessor)
                {
                    IdPayment = dt.Rows[i]["ID_PAYMENT"].ToString(),
                    DescPayment = dt.Rows[i]["DESC_PAYMENT"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }
    }
}
