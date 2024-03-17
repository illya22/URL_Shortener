using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using URL_Shortener.BLL.Abstract;
using URL_Shortener.Models.Entities;

namespace URL_Shortener.Controllers
{
    [Authorize]
    public class LinkController : Controller
    {
        private readonly ILinkService _linkService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LinkController(ILinkService linkService, UserManager<ApplicationUser> userManager)
        {
            _linkService = linkService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
                return BadRequest("Original URL cannot be empty");

            var userId = _userManager.GetUserId(User);
            var result = await _linkService.AddLinkAsync(originalUrl, userId);
            if (result.StatusCode == 1)
                return RedirectToAction(nameof(Index));
            else
                return BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _linkService.DeleteLinkAsync(id);
            if (result.StatusCode == 1)
                return RedirectToAction(nameof(Index));
            else
                return BadRequest(result.Message);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var link = await _linkService.GetLinkAsync(id);
            if (link == null)
                return NotFound();

            return View(link);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var link = await _linkService.GetLinkAsync(id);
            if (link == null)
                return NotFound();

            return View(link);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl))
                return BadRequest("Original URL cannot be empty");

            var result = await _linkService.UpdateLinkAsync(id, originalUrl);
            if (result.StatusCode == 1)
                return RedirectToAction(nameof(Index));
            else
                return BadRequest(result.Message);
        }
    }
}
