using Api.Models;

namespace Api.Interfaces;

public interface IPortfolioRepo
{
    Task<List<Stock>> GetUserPortfolio(AppUser user);

    Task<Portfolio> CreateAsync(Portfolio portfolio);

    Task<Portfolio?> DeleteAsync(AppUser appUser, string symbol);
}
