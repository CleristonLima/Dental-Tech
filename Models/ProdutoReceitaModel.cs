using DentalPlus.Uteis;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DentalPlus.Models
{
    public class ProdutoReceitaModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProdutoReceitaModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ProdutoReceitaModel()
        {

        }

        public string IdProductRevenue { get; set; }

        [Required(ErrorMessage = "Por favor informe o medicamento!")]
        public string NameProduct { get; set; }

        public string LinkPhotoRevenue { get; set; }

        public string userId { get; set; }

        public List<ProdutoReceitaModel> ListarTodosRemedios()
        {
            List<ProdutoReceitaModel> lista = new List<ProdutoReceitaModel>();
            ProdutoReceitaModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT ID_PRODUCT_REVENUE, NAME_PRODUCT, LINK_PHOTO FROM TB_PR_PRODUCT_REVENUE " +
                         "ORDER BY NAME_PRODUCT ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                item = new ProdutoReceitaModel(_httpContextAccessor)
                {
                    IdProductRevenue = row["ID_PRODUCT_REVENUE"].ToString(),
                    NameProduct = row["NAME_PRODUCT"].ToString(),
                    LinkPhotoRevenue = row["LINK_PHOTO"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }

        public ProdutoReceitaModel RetornarRemedios(int? id)
        {

            ProdutoReceitaModel item = null;
            DAL objDAL = new DAL();
            string sql = $"SELECT ID_PRODUCT_REVENUE, NAME_PRODUCT, LINK_PHOTO FROM TB_PR_PRODUCT_REVENUE " +
                        $"WHERE ID_PRODUCT_REVENUE = '{id}' ORDER BY NAME_PRODUCT ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                item = new ProdutoReceitaModel(_httpContextAccessor)
                {
                    IdProductRevenue = dt.Rows[0]["ID_PRODUCT_REVENUE"].ToString(),
                    NameProduct = dt.Rows[0]["NAME_PRODUCT"].ToString(),
                    LinkPhotoRevenue = dt.Rows[0]["LINK_PHOTO"].ToString()
                };
            }

            return item;
        }

        public void Gravar()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(IdProductRevenue))
            {
                // Se for uma atualização, preenche o campo USER_UPDATE
                sql = "UPDATE TB_PR_PRODUCT_REVENUE SET NAME_PRODUCT = @NameProduct, " +
                    "LINK_PHOTO = @LinkPhotoRevenue, " +
                    "USER_UPDATE = @userUpdate, " +
                    "DATE_UPDATE = @dateUpdate " +
                    "WHERE ID_PRODUCT_REVENUE = @IdProductRevenue";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@NameProduct", NameProduct);
                command.Parameters.AddWithValue("@LinkPhotoRevenue", LinkPhotoRevenue);
                command.Parameters.AddWithValue("@userUpdate", userId);
                command.Parameters.AddWithValue("@dateUpdate", currentDateTime);
                command.Parameters.AddWithValue("@IdProductRevenue", IdProductRevenue);

                objDAL.ExecutarComandoSQL(command);
            }
            else
            {

                // Se for uma inserção, preenche o campo USER_INSERT
                sql = "INSERT INTO TB_PR_PRODUCT_REVENUE (NAME_PRODUCT, LINK_PHOTO, USER_INSERT, DATE_INSERT) " +
                                     "VALUES (@NameProduct, @LinkPhotoRevenue, @userInsert, @dateInsert)";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@NameProduct", NameProduct);
                command.Parameters.AddWithValue("@LinkPhotoRevenue", LinkPhotoRevenue);
                command.Parameters.AddWithValue("@userInsert", userId);
                command.Parameters.AddWithValue("@dateInsert", currentDateTime);

                objDAL.ExecutarComandoSQL(command);
            }
        }

        public void ExcluirRemedios(int id)
        {
            DAL objDAL = new DAL();
            string sql = "DELETE FROM TB_PR_PRODUCT_REVENUE WHERE ID_PRODUCT_REVENUE = @IdProductRevenue";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdProductRevenue", id);

            objDAL.ExecutarComandoSQL(command);
        }
    }
}
