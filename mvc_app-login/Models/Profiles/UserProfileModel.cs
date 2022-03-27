namespace mvc_app_login.Models.Profiles
{
    public class UserProfileModel
    {
        public UserProfileModel()
        {
        }

        public UserProfileModel(string firstName, string lastName, string email, string streetName, string postalCode, string city)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            StreetName = streetName;
            PostalCode = postalCode;
            City = city;
        }

        public UserProfileModel(int id, string firstName, string lastName, string email, string streetName, string postalCode, string city)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            StreetName = streetName;
            PostalCode = postalCode;
            City = city;
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string StreetName { get; set; } = "";
        public string PostalCode { get; set; } = "";
        public string City { get; set; } = "";
    }
}
