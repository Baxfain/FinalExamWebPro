using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EastOutHotel.Pages
{
    public class BookConfirmModel : PageModel
    {
        // Matches form field names
        [BindProperty]
        public string FullNameFromForm { get; set; }

        [BindProperty]
        public string Phone { get; set; }

        [BindProperty]
        public string TotalCost { get; set; }

        [BindProperty]
        public string DateRange { get; set; }

        [BindProperty]
        public string PaymentMethod { get; set; }

        public string? UserFullName { get; set; }

        // REQUIRED for form POST
        public void OnPost()
        {
            // Optional: simulate logged-in user
            UserFullName = null;

            // No redirect, just render page with posted values
        }
    }
}