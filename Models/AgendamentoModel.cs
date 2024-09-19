namespace DentalPlus.Models
{
    public class AgendamentoModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AgendamentoModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public AgendamentoModel()
        {

        }
    }
}
