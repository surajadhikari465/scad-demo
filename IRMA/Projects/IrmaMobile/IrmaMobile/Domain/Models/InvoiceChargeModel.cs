namespace IrmaMobile.Domain.Models
{
    public class InvoiceChargeModel
    {
        public int OrderId { get; set; }
        public int SacType { get; set; }
        public string Description { get; set; }
        public int SubteamGlAccount { get; set; }
        public bool Allowance { get; set; }
        public decimal ChargeValue { get; set; }
    }
}