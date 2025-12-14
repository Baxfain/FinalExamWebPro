using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using EastOutHotel.Data;
using EastOutHotel.Models;

namespace EastOutHotel.Pages
{
    public class LoginModel : PageModel
    {
        private readonly UserDAO _userDAO;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(UserDAO userDAO, ILogger<LoginModel> logger)
        {
            _userDAO = userDAO;
            _logger = logger;
        }

        // ==== Bind Properties ====
        [BindProperty, Required]
        public string Username { get; set; } = string.Empty;

        [BindProperty, Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;

        // ==== GET ====
        public IActionResult OnGet(string success = null, string logout = null)
        {
            if (!string.IsNullOrEmpty(success) && success == "registered")
                SuccessMessage = "Registrasi berhasil! Silakan login.";

            if (!string.IsNullOrEmpty(logout))
                SuccessMessage = "Anda telah logout.";

            return Page();
        }

        // ==== POST ====
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Username dan Password wajib diisi!";
                return Page();
            }

            var user = _userDAO.LoginUser(Username, Password);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("AccountType", user.AccountType ?? "Customer");
                return RedirectToPage("/Index");
            }

            ErrorMessage = "Username atau Password salah!";
            return Page();
        }
    }
}
