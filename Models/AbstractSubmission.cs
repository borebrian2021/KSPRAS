using System.ComponentModel.DataAnnotations;

public class AbstractSubmissionModel
{
    // Personal Information
    [Key]
    public string ID { get; set; } // Corresponds to <input name="fname">
    public string FirstName { get; set; } // Corresponds to <input name="fname">
    public string LastName { get; set; } // Corresponds to <input name="lname">
    public string Email { get; set; } // Corresponds to <input name="email">
    public string PhoneNumber { get; set; } // Corresponds to <input id="Pnumber">
    public string Institution { get; set; } // Corresponds to <input id="institution">
    public string PresentingAuthor { get; set; } // Corresponds to <input id="author">
    public string CoAuthors { get; set; } // Corresponds to <textarea id="co_authors">

    // Abstract Details
    public string Title { get; set; } // Corresponds to <input name="title">
    public string PresentationType { get; set; } // Corresponds to <select name="presentation_type">
    public string ConferenceType { get; set; } // Corresponds to <select name="conference_type">

    // Abstract Content
    public string Introduction { get; set; } // Corresponds to <textarea name="introduction">
    public string Methods { get; set; } // Corresponds to <textarea name="methods">
    public string Results { get; set; } // Corresponds to <textarea name="results">
    public string Conclusion { get; set; } // Corresponds to <textarea name="conclusion">
    public Boolean isPaid { get; set; } = false;// Corresponds to <textarea name="conclusion">
    public float ammount { get; set; } = 0;// Corresponds to <textarea name="conclusion">
    public string reffCode { get; set; } // Corresponds to <textarea name="conclusion">
    public string PaymentCategory { get; set; } // Corresponds to <textarea name="conclusion">
    public string Currency { get; set; } // Corresponds to <textarea name="conclusion">
}