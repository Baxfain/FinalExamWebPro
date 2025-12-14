using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace EastOutHotel.Pages
{
    public class ProfileModel : PageModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string BirthDate { get; set; }

        public IActionResult OnGet()
        {
            // Check session
            if (HttpContext.Session.GetString("Username") == null)
            {
                return RedirectToPage("/Login");
            }

            // Load user data from session
            Username = HttpContext.Session.GetString("Username");
            FullName = HttpContext.Session.GetString("FullName");
            Email = HttpContext.Session.GetString("Email");
            PhoneNumber = HttpContext.Session.GetString("PhoneNumber");
            Gender = HttpContext.Session.GetString("Gender");
            BirthDate = HttpContext.Session.GetString("BirthDate");

            return Page();
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}
