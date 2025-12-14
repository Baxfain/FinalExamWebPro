using System;

namespace EastOutHotel.Models
{
    public class User
    {
        public string AccountID { get; set; } = string.Empty;
        public string? CustomerID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string AccountType { get; set; } = "Customer";
        public string? MembershipID { get; set; }

        // Default constructor
        public User() { }

        // Full constructor
        public User(string? customerID, string accountID, string username, string fullName,
                    string phoneNumber, string gender, DateTime birthDate,
                    string accountType, string? membershipID)
        {
            CustomerID = customerID;
            AccountID = accountID;
            Username = username;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Gender = gender;
            BirthDate = birthDate;
            AccountType = accountType;
            MembershipID = membershipID;
        }

        // Simplified constructor
        public User(string username, string fullName, string phoneNumber, string gender, DateTime birthDate, string accountType)
        {
            Username = username;
            FullName = fullName;
            PhoneNumber = phoneNumber;
            Gender = gender;
            BirthDate = birthDate;
            AccountType = accountType;
        }
    }
}
