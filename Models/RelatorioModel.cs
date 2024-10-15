using DentalPlus.Uteis;
using MySql.Data.MySqlClient;

namespace DentalPlus.Models
{
    public class RelatorioModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RelatorioModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public RelatorioModel()
        {

        }

        public DateOnly DateStart { get; set; }

        public DateOnly DateFinish { get; set; }


        public void Consultar()
        {
            DAL objDAL = new DAL();
            string sqlSale = string.Empty;
            string sqlPayable = string.Empty;

            // Se for uma atualização, preenche o campo USER_UPDATE
            sqlSale = "SELECT SUM(MS.TOTAL_VALUE) AS TOTAL " +
                  "FROM TB_MB_SALE MS " +
                  "WHERE MS.DATE_INSERT BETWEEN @startDate AND @endDate ";

            MySqlCommand command = new MySqlCommand(sqlSale);
            command.Parameters.AddWithValue("@startDate", DateStart);
            command.Parameters.AddWithValue("@endDate", DateFinish);

            objDAL.ExecutarComandoSQL(command);


        }
    }
}
