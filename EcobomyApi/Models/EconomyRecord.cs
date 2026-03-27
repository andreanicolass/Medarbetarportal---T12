namespace EcobomyApi.Models
{
    public class EconomyRecord
    {
        public int Id { get; set; }

        public string ItemName { get; set; }
        public string EmployeeName { get; set; }

        public decimal Cost { get; set; }
        public DateTime LoanDate { get; set; }
    }
}
