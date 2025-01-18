using Api.Extensions;
using Api.Interfaces;
using Api.Mappers;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/portfolio")]
[ApiController]
public class PortfilioController(
    UserManager<AppUser> userManager,
    IStockRepo stockRepo,
    IPortfolioRepo portfolioRepo
) : ControllerBase
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IStockRepo _stockRepo = stockRepo;
    private readonly IPortfolioRepo _portfolioRepo = portfolioRepo;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolio()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var username = User.GetUsername();
        AppUser? appUser = await _userManager.FindByNameAsync(username);

        if (appUser is null)
        {
            return StatusCode(500);
        }

        List<Stock> portfolios = await _portfolioRepo.GetUserPortfolio(appUser);

        return Ok(portfolios.Select((p) => p.ToStockDto()));
    }
}
