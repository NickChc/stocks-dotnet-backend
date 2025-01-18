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
}
