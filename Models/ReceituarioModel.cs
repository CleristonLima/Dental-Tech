namespace DentalPlus.Models
{
    public class ReceituarioModel
    {
        public string IdMrRecipe { get; set; }

        public string IdPatients { get; set; }

        public string Symptoms { get; set; }

        public DateOnly dateIssue { get; set; }

        public string IdMedicinePrescription { get; set; }

        public string IdProductRevenue { get; set; }

        public double Qty { get; set; }

        public string userId { get; set; }
    }
}
