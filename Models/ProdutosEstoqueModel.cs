using DentalPlus.Uteis;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace DentalPlus.Models
{
    public class ProdutosEstoqueModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProdutosEstoqueModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ProdutosEstoqueModel()
        {

        }

        public string IdProduct { get; set; }

        [Required(ErrorMessage = "Por favor selecione o nome do medicamento!")]
        public string DescProduct { get; set; }

        [Required(ErrorMessage = "Por favor informe a quantidade para o estoque!")]
        public decimal QtyStock { get; set; }

        [Required(ErrorMessage = "Por favor informe a quantidade minima para o estoque!")]
        public decimal QtyMinStock { get; set; }

        [Required(ErrorMessage = "Por favor informe o unitlizador!")]
        public string Unitilizer { get; set; }

        public string LinkPhoto { get; set; }

        public decimal QtySubtr { get; set; }

        public decimal QtyAdd { get; set; }

        public string userId { get; set; }

        public List<ProdutosEstoqueModel> VerificarEstoqueBaixo()
        {
            List<ProdutosEstoqueModel> lista = new List<ProdutosEstoqueModel>();
            DAL objDAL = new DAL();

            string sql = "SELECT ID_PRODUCT, DESC_PRODUCT, QTY_STOCK, QTY_MIN_STOCK FROM TB_PR_PRODUCT " +
                         "WHERE QTY_STOCK <= QTY_MIN_STOCK";
            DataTable dt = objDAL.RetDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                ProdutosEstoqueModel item = new ProdutosEstoqueModel(_httpContextAccessor)
                {
                    IdProduct = row["ID_PRODUCT"].ToString(),
                    DescProduct = row["DESC_PRODUCT"].ToString(),
                    QtyStock = Convert.ToDecimal(row["QTY_STOCK"].ToString()),
                    QtyMinStock = Convert.ToDecimal(row["QTY_MIN_STOCK"].ToString())
                };

                lista.Add(item);
            }

            return lista;
        }

        public List<ProdutosEstoqueModel> ListarTodosProdutosEstoque()
        {
            List<ProdutosEstoqueModel> lista = new List<ProdutosEstoqueModel>();
            ProdutosEstoqueModel item;
            DAL objDAL = new DAL();

            string sql = "SELECT ID_PRODUCT, DESC_PRODUCT, QTY_STOCK, QTY_MIN_STOCK, UNITILIZER, LINK_PHOTO FROM TB_PR_PRODUCT " +
                         "ORDER BY DESC_PRODUCT ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            foreach (DataRow row in dt.Rows)
            {
                item = new ProdutosEstoqueModel(_httpContextAccessor)
                {
                    IdProduct = row["ID_PRODUCT"].ToString(),
                    DescProduct = row["DESC_PRODUCT"].ToString(),
                    QtyStock = Convert.ToDecimal(row["QTY_STOCK"].ToString()),
                    QtyMinStock = Convert.ToDecimal(row["QTY_MIN_STOCK"].ToString()),
                    Unitilizer = row["UNITILIZER"].ToString(),
                    LinkPhoto = row["LINK_PHOTO"].ToString()
                };

                lista.Add(item);
            }

            return lista;
        }

        public ProdutosEstoqueModel RetornarProdutosEstoque(int? id)
        {

            ProdutosEstoqueModel item = null;
            DAL objDAL = new DAL();
            string sql = $"SELECT ID_PRODUCT, DESC_PRODUCT, QTY_STOCK, QTY_MIN_STOCK, UNITILIZER, LINK_PHOTO FROM TB_PR_PRODUCT " +
                        $"WHERE ID_PRODUCT = '{id}' ORDER BY DESC_PRODUCT ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                item = new ProdutosEstoqueModel(_httpContextAccessor)
                {
                    IdProduct = dt.Rows[0]["ID_PRODUCT"].ToString(),
                    DescProduct = dt.Rows[0]["DESC_PRODUCT"].ToString(),
                    QtyStock = Convert.ToDecimal(dt.Rows[0]["QTY_STOCK"].ToString()),
                    QtyMinStock = Convert.ToDecimal(dt.Rows[0]["QTY_MIN_STOCK"].ToString()),
                    Unitilizer = dt.Rows[0]["UNITILIZER"].ToString(),
                    LinkPhoto = dt.Rows[0]["LINK_PHOTO"].ToString()
                };
            }

            return item;
        }

        public void Gravar()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(IdProduct))
            {
                // Se for uma atualização, preenche o campo USER_UPDATE
                sql = "UPDATE TB_PR_PRODUCT SET DESC_PRODUCT = @DescProduct, " +
                    "QTY_STOCK = @QtyStock, " +
                    "QTY_MIN_STOCK = @QtyMinStock, " +
                    "UNITILIZER = @Unitilizer, " +
                    "LINK_PHOTO = @LinkPhoto, " +
                    "USER_UPDATE = @userUpdate, " +
                    "DATE_UPDATE = @dateUpdate " +
                    "WHERE ID_PRODUCT = @IdProduct";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@DescProduct", DescProduct);
                command.Parameters.AddWithValue("@QtyStock", QtyStock);
                command.Parameters.AddWithValue("@QtyMinStock", QtyMinStock);
                command.Parameters.AddWithValue("@Unitilizer", Unitilizer);
                command.Parameters.AddWithValue("@LinkPhoto", LinkPhoto);
                command.Parameters.AddWithValue("@userUpdate", userId);
                command.Parameters.AddWithValue("@dateUpdate", currentDateTime);
                command.Parameters.AddWithValue("@IdProduct", IdProduct);

                objDAL.ExecutarComandoSQL(command);
            }
            else
            {

                // Se for uma inserção, preenche o campo USER_INSERT
                sql = "INSERT INTO TB_PR_PRODUCT (DESC_PRODUCT, QTY_STOCK, QTY_MIN_STOCK, UNITILIZER, LINK_PHOTO, USER_INSERT, DATE_INSERT) " +
                                     "VALUES (@DescProduct, @QtyStock, @QtyMinStock, @Unitilizer, @LinkPhoto, @userInsert, @dateInsert)";

                MySqlCommand command = new MySqlCommand(sql);
                command.Parameters.AddWithValue("@DescProduct", DescProduct);
                command.Parameters.AddWithValue("@QtyStock", QtyStock);
                command.Parameters.AddWithValue("@QtyMinStock", QtyMinStock);
                command.Parameters.AddWithValue("@Unitilizer", Unitilizer);
                command.Parameters.AddWithValue("@LinkPhoto", LinkPhoto);
                command.Parameters.AddWithValue("@userInsert", userId);
                command.Parameters.AddWithValue("@dateInsert", currentDateTime);

                objDAL.ExecutarComandoSQL(command);
            }
        }

        public ProdutosEstoqueModel RetornarProdutosParaUsar(int? id)
        {

            ProdutosEstoqueModel item = null;
            DAL objDAL = new DAL();
            string sql = $"SELECT ID_PRODUCT, DESC_PRODUCT, QTY_STOCK FROM TB_PR_PRODUCT " +
                        $"WHERE ID_PRODUCT = '{id}' ORDER BY DESC_PRODUCT ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                item = new ProdutosEstoqueModel(_httpContextAccessor)
                {
                    IdProduct = dt.Rows[0]["ID_PRODUCT"].ToString(),
                    DescProduct = dt.Rows[0]["DESC_PRODUCT"].ToString(),
                    QtyStock = Convert.ToDecimal(dt.Rows[0]["QTY_STOCK"].ToString())
                };
            }

            return item;
        }
        public void SubtrairEstoque()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(IdProduct))
            {
                sql = "SELECT QTY_STOCK FROM TB_PR_PRODUCT WHERE ID_PRODUCT = @IdProduct";
                MySqlCommand selectCommand = new MySqlCommand(sql);
                selectCommand.Parameters.AddWithValue("@IdProduct", IdProduct);
                DataTable dt = objDAL.RetDataTable(selectCommand);

                if (dt.Rows.Count > 0)
                {
                    decimal currentStock = Convert.ToDecimal(dt.Rows[0]["QTY_STOCK"]);

                    if (currentStock >= QtySubtr)
                    {
                        // Subtrai a quantidade do estoque
                        decimal newStock = currentStock - QtySubtr;

                        // Atualiza apenas o campo QTY_STOCK e os campos de auditoria
                        sql = "UPDATE TB_PR_PRODUCT SET QTY_STOCK = @newStock, " +
                              "USER_UPDATE = @userUpdate, " +
                              "DATE_UPDATE = @dateUpdate " +
                              "WHERE ID_PRODUCT = @IdProduct";

                        MySqlCommand updateCommand = new MySqlCommand(sql);
                        updateCommand.Parameters.AddWithValue("@newStock", newStock);
                        updateCommand.Parameters.AddWithValue("@userUpdate", userId);
                        updateCommand.Parameters.AddWithValue("@dateUpdate", currentDateTime);
                        updateCommand.Parameters.AddWithValue("@IdProduct", IdProduct);

                        objDAL.ExecutarComandoSQL(updateCommand);
                    }
                    else
                    {
                        throw new Exception("Produto não tem saldo em estoque. Favor Abastecer!");
                    }
                }
            }
        }

        public ProdutosEstoqueModel RetornarProdutosParaAbastecer(int? id)
        {
            ProdutosEstoqueModel item = null;
            DAL objDAL = new DAL();
            string sql = $"SELECT ID_PRODUCT, DESC_PRODUCT, QTY_STOCK FROM TB_PR_PRODUCT " +
                        $"WHERE ID_PRODUCT = '{id}' ORDER BY DESC_PRODUCT ASC";
            DataTable dt = objDAL.RetDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                item = new ProdutosEstoqueModel(_httpContextAccessor)
                {
                    IdProduct = dt.Rows[0]["ID_PRODUCT"].ToString(),
                    DescProduct = dt.Rows[0]["DESC_PRODUCT"].ToString(),
                    QtyStock = Convert.ToDecimal(dt.Rows[0]["QTY_STOCK"].ToString())
                };
            }

            return item;
        }

        public void AbastecerEstoque()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            if (!string.IsNullOrEmpty(IdProduct))
            {
                sql = "SELECT QTY_STOCK FROM TB_PR_PRODUCT WHERE ID_PRODUCT = @IdProduct";
                MySqlCommand selectCommand = new MySqlCommand(sql);
                selectCommand.Parameters.AddWithValue("@IdProduct", IdProduct);
                DataTable dt = objDAL.RetDataTable(selectCommand);

                if (dt.Rows.Count > 0)
                {
                    decimal currentStock = Convert.ToDecimal(dt.Rows[0]["QTY_STOCK"]);

                    decimal newStock = currentStock + QtyAdd;

                    sql = "UPDATE TB_PR_PRODUCT SET QTY_STOCK = @newStock, " +
                          "USER_UPDATE = @userUpdate, " +
                          "DATE_UPDATE = @dateUpdate " +
                          "WHERE ID_PRODUCT = @IdProduct";

                    MySqlCommand updateCommand = new MySqlCommand(sql);
                    updateCommand.Parameters.AddWithValue("@newStock", newStock);
                    updateCommand.Parameters.AddWithValue("@userUpdate", userId);
                    updateCommand.Parameters.AddWithValue("@dateUpdate", currentDateTime);
                    updateCommand.Parameters.AddWithValue("@IdProduct", IdProduct);

                    objDAL.ExecutarComandoSQL(updateCommand);
                }
            }
        }

        public void ExcluirProdutosEstoque(int id)
        {
            DAL objDAL = new DAL();
            string sql = "DELETE FROM TB_PR_PRODUCT WHERE ID_PRODUCT = @IdProduct";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdProduct", id);

            objDAL.ExecutarComandoSQL(command);
        }
    }
}
