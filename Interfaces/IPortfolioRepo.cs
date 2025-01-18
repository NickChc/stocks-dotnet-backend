using Api.Models;

namespace Api.Interfaces;

public interface IPortfolioRepo
{
    Task<List<Stock>> GetUserPortfolio(AppUser user);
}
