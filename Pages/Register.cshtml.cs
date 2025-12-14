using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EastOutHotel.Data;
using EastOutHotel.Models;

namespace EastOutHotel.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly UserDAO _userDAO;

        public RegisterModel(UserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        [BindProperty, Required]
        public string Username { get; set; } = string.Empty;

        [BindProperty, Required]
        public string FullName { get; set; } = string.Empty;

        [BindProperty, Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [BindProperty, Required]
        public string Gender { get; set; } = string.Empty;

        [BindProperty, Required, DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [BindProperty, Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [BindProperty, Required, DataType(DataType.Password)]
        public string RePassword { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Semua field wajib diisi!";
                return Page();
            }

            if (Password != RePassword)
            {
                ErrorMessage = "Password dan Konfirmasi Password tidak sama!";
                return Page();
            }

            if (_userDAO.IsUsernameExists(Username))
            {
                ErrorMessage = "Username sudah digunakan!";
                return Page();
            }

            var newUser = new User
            {
                Username = Username,
                FullName = FullName,
                PhoneNumber = PhoneNumber,
                Gender = Gender,
                BirthDate = BirthDate
            };

            if (_userDAO.RegisterUser(newUser, Password))
                return RedirectToPage("/Login", new { success = "registered" });

            ErrorMessage = "Registrasi gagal, silakan coba lagi.";
            return Page();
        }
    }
}
