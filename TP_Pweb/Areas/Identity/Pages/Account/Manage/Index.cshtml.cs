// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TP_Pweb.Models;

namespace TP_Pweb.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Utilizador> _userManager;
        private readonly SignInManager<Utilizador> _signInManager;

        public IndexModel(
            UserManager<Utilizador> userManager,
            SignInManager<Utilizador> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
           
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "O meu Avatar")]
            public byte[]? Avatar { get; set; }

            public IFormFile AvatarFile { get; set; }

            [Display(Name = "Primeiro Nome")]
            public string PrimeiroNome { get; set; }

            [Display(Name = "UltimoNome")]
            public string UltimoNome { get; set; }

            [Display(Name = "DataNascimento")]
            public DateTime DataNascimento { get; set; }

            [Display(Name = "NIF")]
            public int NIF { get; set; }

            [Display(Name = "Ativo")]
            public Boolean ativo { get; set; }

        }

        private async Task LoadAsync(Utilizador user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Avatar = user.Avatar,
                PhoneNumber = phoneNumber,
                PrimeiroNome = user.PrimeiroNome,
                UltimoNome = user.UltimoNome,
                DataNascimento = user.DataNascimento,
                NIF = user.NIF,
                ativo = user.ativo
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            //TODO VERIFICAÇÔES
            if (Input.AvatarFile != null)
            {
                if (Input.AvatarFile.Length > (200 * 1024))
                {
                    StatusMessage = "Error: Ficheiro demasiado grande";
                    return RedirectToPage();
                }
                // método a implementar – verifica se a extensão é .png,.jpg,.jpeg
                if (!isValidFileType(Input.AvatarFile.FileName))
                {
                    StatusMessage = "Error: Ficheiro não suportado";
                    return RedirectToPage();
                }
                using (var dataStream = new MemoryStream())
                {
                    await Input.AvatarFile.CopyToAsync(dataStream);
                    user.Avatar = dataStream.ToArray();
                }
                await _userManager.UpdateAsync(user);
            }

            user.PrimeiroNome = Input.PrimeiroNome;
            user.UltimoNome = Input.UltimoNome;
            user.DataNascimento = Input.DataNascimento;
            user.ativo = Input.ativo;
            user.NIF = Input.NIF;

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
        bool isValidFileType(string fileName)
        {
            if (fileName.EndsWith(".png") || fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg"))
            {
                return true;
            }
            else
                return false;
        }
    }
}
