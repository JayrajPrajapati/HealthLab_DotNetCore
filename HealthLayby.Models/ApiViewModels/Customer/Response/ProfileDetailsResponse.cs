namespace HealthLayby.Models.ApiViewModels.Customer.Response
{
    public class ProfileDetailsResponse
    {
        public long CustomerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? ProfilePic { get; set; }
        public string? ProfilePicPath { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? CustomerAddress { get; set; }
        //public string? EmergencyContactFirstName { get; set; }
        //public string? EmergencyContactLastName { get; set; }
        //public string? EmergencyContactNumber { get; set; }
        //public string? EmergencyContactEmail { get; set; }
        public List<FamilyMemberResponse>? FamilyMembers { get; set; }
        //public List<EmergencyContactResponse>? EmergencyContacts { get; set; }
    }
}
