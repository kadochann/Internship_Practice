using Microsoft.AspNetCore.Mvc;

namespace BookStoresWebAPI.Controllers
{
    public class PopupController : Controller
{
    public IActionResult AccessDenied()
    {
        return PartialView();
    }
}
}
