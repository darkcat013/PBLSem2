using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Construx.App.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
    }
}
