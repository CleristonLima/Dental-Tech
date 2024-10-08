﻿using DentalPlus.Uteis;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;

namespace DentalPlus.Models
{
    public class FinanceiroModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FinanceiroModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public FinanceiroModel()
        {

        }
        public string IdMbSale {  get; set; }

        public string IdDoctor { get; set; }

        [Required(ErrorMessage = "Informe o médico!")]
        public string NameDoctor { get; set; }

        public string IdPatients { get; set; }

        [Required(ErrorMessage = "Informe o paciente!")]
        public string NamePatient { get; set; }

        public string IdPayment { get; set; }

        [Required(ErrorMessage = "Informe a forma de pagamento!")]
        public string DescPayment { get; set; }

        [Required(ErrorMessage = "Informe o tipo de consulta!")]
        public string IdMedicalConsultation { get; set; }

        public string DescMedicalConsultation { get; set; }

        [Required(ErrorMessage = "Informe a quantidade!")]
        public double Qty {  get; set; }

        public decimal ValueConsultation { get; set; }

        public decimal TotalValue { get; set; }

        public string userId { get; set; }

        public decimal GetConsultaPreco(string idConsulta)
        {
            DAL objDAL = new DAL();

            string query = "SELECT VALUE_CONSULTATION FROM TB_CLI_MEDICAL_CONSULTATION WHERE ID_MEDICAL_CONSULTATION = @Id";
            var parameters = new Dictionary<string, object>
            {
                    { "@Id", idConsulta }
            };
            DataTable dt = objDAL.RetDataTableObj(query, parameters);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToDecimal(dt.Rows[0]["VALUE_CONSULTATION"]);
            }
            else
            {
                throw new Exception("Consulta não encontrada.");
            }
        }

        public void Gravar()
        {
            DAL objDAL = new DAL();
            string sql = string.Empty;

            string currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Garantir que o valor total esteja no formato correto
            decimal totalValueFormatted = Math.Round(TotalValue, 2, MidpointRounding.AwayFromZero);

            // Inserir na tabela TB_MB_SALE
            sql = "INSERT INTO TB_MB_SALE (ID_DOCTOR, ID_PAYMENT, TOTAL_VALUE, ID_PATIENTS, USER_INSERT, DATE_INSERT) " +
                                "VALUES (@IdDoctor, @IdPayment, @TotalValue, @IdPatients, @userInsert, @dateInsert) ";

            MySqlCommand command = new MySqlCommand(sql);
            command.Parameters.AddWithValue("@IdDoctor", IdDoctor);
            command.Parameters.AddWithValue("@IdPayment", IdPayment);
            command.Parameters.AddWithValue("@TotalValue", totalValueFormatted);
            command.Parameters.AddWithValue("@IdPatients", IdPatients);
            command.Parameters.AddWithValue("@userInsert", userId);
            command.Parameters.AddWithValue("@dateInsert", currentDateTime);

            objDAL.ExecutarComandoSQL(command);

            /*sql = "SELECT LAST_INSERT_ID();";

            MySqlCommand commandGetId = new MySqlCommand(sql);

            DataTable dt = objDAL.RetDataTable(commandGetId);

            if (dt.Rows.Count > 0)
            {
                int lastInsertedId = Convert.ToInt32(dt.Rows[0]["LAST_INSERT_ID()"]);

                decimal ValueConsultationFormatted = Math.Round(ValueConsultation, 2, MidpointRounding.AwayFromZero);

                foreach (var idPatient in IdPatients)
                {

                    string sqlPatientId = "SELECT ID_PATIENTS FROM TB_CLI_PATIENTS WHERE ID_PATIENTS = @IdPatients "; // Ajustar a condição conforme necessário
                    MySqlCommand commandGetPatientId = new MySqlCommand(sqlPatientId);

                    commandGetPatientId.Parameters.AddWithValue("@IdPatients", IdPatients);

                    DataTable dtPatients = objDAL.RetDataTable(commandGetPatientId);

                    if (dtPatients.Rows.Count > 0)
                    {

                        int idPatients = Convert.ToInt32(dtPatients.Rows[0]["ID_PATIENTS"]);

                        string sqlItens = "INSERT INTO TB_MB_SALE_ITEM (ID_MB_SALE, ID_MEDICAL_CONSULTATION, QTY, VALUE_CONSULTATION, ID_PATIENTS, USER_INSERT, DATE_INSERT) " +
                                                           "VALUES (@IdMbSale, @IdMedicalConsultation, @Qty, @ValueConsultation, @IdPatients, @userInsert, @dateInsert)";

                        MySqlCommand commandItens = new MySqlCommand(sqlItens);
                        commandItens.Parameters.AddWithValue("@IdMbSale", lastInsertedId);
                        commandItens.Parameters.AddWithValue("@IdMedicalConsultation", IdMedicalConsultation);
                        commandItens.Parameters.AddWithValue("@Qty", Qty);
                        commandItens.Parameters.AddWithValue("@ValueConsultation", ValueConsultationFormatted);
                        commandItens.Parameters.AddWithValue("@IdPatients", idPatients);
                        commandItens.Parameters.AddWithValue("@userInsert", userId);
                        commandItens.Parameters.AddWithValue("@dateInsert", currentDateTime);

                        objDAL.ExecutarComandoSQL(commandItens);
                    }
                    else
                    {
                        throw new Exception("Erro ao obter o ID do paciente.");
                    }
                }

            }
            else
            {
                throw new Exception("Erro ao inserir a venda e recuperar o último ID.");
            }*/
        }
    }

}
