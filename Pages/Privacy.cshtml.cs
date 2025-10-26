using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Diagnostics;

namespace LibraryManagement.Pages;
    [Authorize]
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

public PrivacyModel(ILogger<PrivacyModel> logger)
{
  _logger = logger;
}
public void OnGet()
    {
    var statusCodePagesFeature =
        HttpContext.Features.Get<IStatusCodePagesFeature>();

    if (statusCodePagesFeature is not null)
    {
        statusCodePagesFeature.Enabled = false;
    }
}
}
