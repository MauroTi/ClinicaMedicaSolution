using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace ClinicaMedica.Web.Pages.Database
{
    public class SelectProviderModel : PageModel
    {
        public IActionResult OnPostSet(string provider)
        {
            if (!Enum.TryParse(provider, true, out DatabaseProvider parsed))
                return BadRequest();

            Response.Cookies.Append("SelectedDatabaseProvider", parsed.ToString(), new Microsoft.AspNetCore.Http.CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                HttpOnly = false,
                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax
            });

            return RedirectToPage("/Index");
        }
    }
}