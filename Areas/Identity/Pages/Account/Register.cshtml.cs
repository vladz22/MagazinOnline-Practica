using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MagazinOnline.DataAcces.Repository.IRepository;
using MagazinOnline.Modele;
using MagazinOnline.Utilitate;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace MagazinOnline.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unit;
        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unit
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _unit = unit;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string Nume { get; set; }
            public string Adresa { get; set; }
            public string Oras { get; set; }
            public string Tara { get; set; }
            public string PhoneNumber { get; set; }
            public string CodPostal { get; set; }

            public int? CompanieId { get; set; } //int? pentru a accepta si cazul de null

            public string Rol { get; set; }
            public IEnumerable<SelectListItem> ListaCompanie { get; set; }
            public IEnumerable<SelectListItem> ListaRol { get; set; }


        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            Input = new InputModel()
            {
                ListaCompanie = _unit.Companie.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Nume,
                    Value = i.Id.ToString()
                }),
                ListaRol = _roleManager.Roles.Where(x => x.Name != StaticDetalii.Rol_User_Indiv).Select(x => x.Name).Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x
                })

            };
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Nume = Input.Nume,
                    Adresa = Input.Adresa,
                    Oras = Input.Oras,
                    Tara = Input.Tara,
                    PhoneNumber = Input.PhoneNumber,
                    CodPostal = Input.CodPostal,
                    CompanieId = Input.CompanieId,
                    Rol = Input.Rol
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    if (!await _roleManager.RoleExistsAsync(StaticDetalii.Rol_Admin))//verificam daca exista rolu
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetalii.Rol_Admin));//il creaza daca nu exista
                    }
                    if (!await _roleManager.RoleExistsAsync(StaticDetalii.Rol_Angajat))//verificam daca exista rolu
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetalii.Rol_Angajat));//il creaza daca nu exista
                    }
                    if (!await _roleManager.RoleExistsAsync(StaticDetalii.Rol_User_Indiv))//verificam daca exista rolu
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetalii.Rol_User_Indiv));//il creaza daca nu exista
                    }
                    if (!await _roleManager.RoleExistsAsync(StaticDetalii.Rol_User_Comp))//verificam daca exista rolu
                    {
                        await _roleManager.CreateAsync(new IdentityRole(StaticDetalii.Rol_User_Comp));//il creaza daca nu exista
                    }

                    // await _userManager.AddToRoleAsync(user,StaticDetalii.Rol_Admin);dam rol de admin la oricine se inregistreaza


                    //separat
                    if (user.Rol == null)
                    {
                        await _userManager.AddToRoleAsync(user, StaticDetalii.Rol_User_Indiv);
                    }
                    else
                    {
                        if (user.CompanieId > 0)
                        {
                            await _userManager.AddToRoleAsync(user, StaticDetalii.Rol_User_Comp);
                        }
                        await _userManager.AddToRoleAsync(user, user.Rol);

                    }

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme); 

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (user.Rol == null)
                        {
                            //daca userul are rol de indiv dupa ce creaza contu
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            //daca cream un cont pentru user din admin panel
                            return RedirectToAction("Index", "User", new { Area = "Admin" });
                        }

                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            Input = new InputModel()
            {
                ListaCompanie = _unit.Companie.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Nume,
                    Value = i.Id.ToString()
                }),
                ListaRol = _roleManager.Roles.Where(x => x.Name != StaticDetalii.Rol_User_Indiv).Select(x => x.Name).Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x
                })

            };
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}