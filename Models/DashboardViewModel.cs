namespace KSPRAS.Models
{
    public class DashboardViewModel
    {
        public int TotalRegistrations { get; set; }
        public int PaidRegistrations { get; set; }
        public List<Registrations> Registrations { get; set; }
        public List<Registrations> UnpaidRegistrations { get; set; }
        public List<PaymentResponse> CashPayments { get; set; }
    }
}