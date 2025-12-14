using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace EastOutHotel.Pages
{
    public class BookingModel : PageModel
    {
        private readonly ILogger<BookingModel> _logger;

        // Property to hold the user information
        [BindProperty]
        public UserDetails User { get; set; }

        // Property to store room selection, dates, and payment method
        [BindProperty]
        public BookingDetails Booking { get; set; }

        // Constructor with logger dependency injection
        public BookingModel(ILogger<BookingModel> logger)
        {
            _logger = logger;
            User = new UserDetails();
            Booking = new BookingDetails();
        }

        // This runs when the page is loaded (GET request)
        public void OnGet()
        {
            // You can preload user info or logic here if needed
            // For example, get user data from session or database if authenticated

            // Example user data:
            User.FullName = "John Doe"; // Just a placeholder, retrieve this from session or DB
            User.PhoneNumber = "1234567890"; // Just a placeholder, retrieve this from session or DB
        }

        // This runs when the form is submitted (POST request)
        public IActionResult OnPost()
        {
            // Perform logic to process the booking
            // For example, validate data, calculate costs, save the booking to the database, etc.

            if (!ModelState.IsValid)
            {
                return Page(); // If there are validation errors, return to the booking page
            }

            // Process the booking data
            // You can pass this data to a service layer or save it to a database
            _logger.LogInformation($"Booking for {User.FullName} at {Booking.CheckInDate} - {Booking.CheckOutDate} with {Booking.PaymentMethod}");

            // After processing, redirect to a confirmation page
            return RedirectToPage("BookConfirm", new { bookingId = 123 }); // Example, replace with real logic
        }
    }

    // Model to hold user details
    public class UserDetails
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }

    // Model to hold booking details
    public class BookingDetails
    {
        public string RoomType { get; set; }
        public string PaymentMethod { get; set; }
        public string DateRange { get; set; } // You might want to parse and separate the dates later
        public string CheckInDate { get; set; }
        public string CheckOutDate { get; set; }
        public decimal TotalCost { get; set; }
    }
}