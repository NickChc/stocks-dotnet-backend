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

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        string username = User.GetUsername();
        AppUser? appUser = await _userManager.FindByNameAsync(username);
        Stock? stock = await _stockRepo.GetBySymbolAsync(symbol);

        if (appUser is null)
        {
            return Unauthorized();
        }

        if (stock is null)
        {
            return BadRequest("No stock associated with user");
        }

        List<Stock> userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

        bool hasStock = userPortfolio.Any(
            (s) => s.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase)
        );

        if (hasStock)
        {
            return BadRequest("Cannot add the same stock to portfolio");
        }

        Portfolio portfolioModel = new() { AppUserId = appUser.Id, StockId = stock.Id };

        var portfolio = await _portfolioRepo.CreateAsync(portfolioModel);

        return Created();
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        string username = User.GetUsername();
        AppUser? appUser = await _userManager.FindByNameAsync(username);

        if (appUser is null)
        {
            return StatusCode(500);
        }

        List<Stock> userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

        Stock? existingStock = userPortfolio.Find(
            (s) => s.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase)
        );

        if (existingStock is null)
        {
            return NotFound();
        }

        Portfolio? doubleCheckPortfolio = await _portfolioRepo.DeleteAsync(appUser, symbol);

        if (doubleCheckPortfolio is null)
        {
            return NotFound();
        }

        return NoContent();
    }
}
