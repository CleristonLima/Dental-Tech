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

        public string NameDoctor { get; set; }

        public string IdPatients { get; set; }

        public string NamePatient { get; set; }

        public string IdPayment { get; set; }

        public string DescPayment { get; set; }

        public string IdMedicalConsultation { get; set; }

        public double Qty {  get; set; }

        public double ValueConsultation { get; set; }

        public double TotalValue { get; set; }

        public string userId { get; set; }

        /*public List<PacienteModel> RetornarListaPacientes()
        {
            return new PacienteModel().ListarTodosPacientes();
        }

        public List<MedicoModel> RetornarListaMedicos()
        {
            return new MedicoModel().ListarTodosMedicos();
        }

        public List<TipoConsultaModel> RetornarListaMedicos()
        {
            return new TipoConsultaModel().ListarTodosTiposConsulta();
        }*/
    }
}
