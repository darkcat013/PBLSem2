using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Construx.App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Construx.App.Constants;
using Construx.App.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Construx.App.Interfaces;

namespace Construx.App.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IGenericRepository<City> _cityRepository;
        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger, IGenericRepository<City> cityRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _cityRepository = cityRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Company representative")]
            public bool Representative { get; set; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "Birth date")]
            public DateTime BirthDate { get; set; }

            [Required]
            [Display(Name = "City/Region")]
            public int CityId { get; set; }

            [Display(Name = "City/Region")]
            public City City { get; set; }

            [Required]
            [Phone]
            [Display(Name = "Phone")]
            public string PhoneNumber { get; set; }



        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ViewData["CityId"] = new SelectList(await _cityRepository.GetAll(), "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new User 
                { 
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    BirthDate = Input.BirthDate,
                    CityId = Input.CityId,
                    UserName = Input.UserName, 
                    Email = Input.Email,
                    PhoneNumber = Input.PhoneNumber,
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    if(Input.Representative)
                    {
                        await _userManager.AddToRoleAsync(user, UserRoles.Representative);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect("~/Representatives/Create");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, UserRoles.User);
                    } 
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ViewData["CityId"] = new SelectList(await _cityRepository.GetAll(), "Id", "Name");
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
