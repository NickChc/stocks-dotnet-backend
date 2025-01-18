using Api.Data;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class PortfolioRepo(ApplicationDbContext dbContext) : IPortfolioRepo
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<Stock>> GetUserPortfolio(AppUser user)
    {
        return await _dbContext
            .Portfolios.Where((u) => u.AppUserId == user.Id)
            .Select(
                (stock) =>
                    new Stock()
                    {
                        Id = stock.StockId,
                        Symbol = stock.Stock!.Symbol,
                        CompanyName = stock.Stock.CompanyName,
                        Purchase = stock.Stock.Purchase,
                        LastDiv = stock.Stock.LastDiv,
                        Industry = stock.Stock.Industry,
                        MarketCap = stock.Stock.MarketCap,
                    }
            )
            .ToListAsync();
    }

    public async Task<Portfolio> CreateAsync(Portfolio portfolio)
    {
        await _dbContext.Portfolios.AddAsync(portfolio);
        await _dbContext.SaveChangesAsync();

        return portfolio;
    }

    public async Task<Portfolio?> DeleteAsync(AppUser appUser, string symbol)
    {
        Portfolio? portfolio = await _dbContext
            .Portfolios.Include((p) => p.Stock)
            .FirstOrDefaultAsync(
                (p) => p.AppUserId == appUser.Id && p.Stock!.Symbol.ToLower() == symbol.ToLower()
            );

        if (portfolio is null)
        {
            return null;
        }

        _dbContext.Portfolios.Remove(portfolio);
        await _dbContext.SaveChangesAsync();

        return portfolio;
    }
}
