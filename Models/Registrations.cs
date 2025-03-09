namespace KSPRAS.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Registrations
    {
            [Key]
            public int Id { get; set; } // "Yes" or "No"
            public string Attend { get; set; } // "Yes" or "No"
            public string FName { get; set; }
            public string SName { get; set; }
            public string Cadre { get; set; }
            public string TelephoneNumber { get; set; }
            public string EmailAddress { get; set; }
            public string Institution { get; set; }
            public string PaymentConfirmation { get; set; } // "Yes" or "No"
        public Boolean isPaid { get; set; } = false;// Corresponds to <textarea name="conclusion">
        public float ammount { get; set; } = 0;// Corresponds to <textarea name="conclusion">
        public string reffCode { get; set; } // Corresponds to <textarea name="conclusion">
        public string PaymentCategory { get; set; } // Corresponds to <textarea name="conclusion">
        public string Currency { get; set; } // Corresponds to <textarea name="conclusion">
    }
    
}
