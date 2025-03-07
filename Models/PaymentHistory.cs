namespace KSPRAS.Models
{
    using System;

    public class PaymentRequest
    {
        public string Id { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string CallbackUrl { get; set; }
        public string NotificationId { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string ZipCode { get; set; }
    }
}
