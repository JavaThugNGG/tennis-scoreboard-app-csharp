using Microsoft.AspNetCore.Mvc;

namespace TennisScoreboard
{
    public class NewMatchController : Controller
    {
        [HttpGet("/new-match")]
        public IActionResult ShowForm()
        {
            return View("NewMatch");
        }
    }
}
