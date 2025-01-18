using Api.Data;
using Api.Dtos.Stock;
using Api.Helpers;
using Api.Interfaces;
using Api.Mappers;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories;

public class StockRepo(ApplicationDbContext dbContext) : IStockRepo
{
    private readonly ApplicationDbContext _context = dbContext;

    public async Task<(int, List<Stock>)> GetAllAsync(QueryObject query)
    {
        var stocks = _context
            .Stock.Include((s) => s.Comments)
            .ThenInclude((c) => c.AppUser)
            .AsQueryable()
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.CompanyName))
        {
            stocks = stocks.Where((s) => s.CompanyName.Contains(query.CompanyName));
        }

        if (!string.IsNullOrWhiteSpace(query.Symbol))
        {
            stocks = stocks.Where((s) => s.Symbol.Contains(query.Symbol));
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.IsDescending
                    ? stocks.OrderByDescending((s) => s.Symbol)
                    : stocks.OrderBy((s) => s.Symbol);
            }
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;

        int totalStocks = await stocks.CountAsync();
        var finalStocks = await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();

        return (totalStocks, finalStocks);
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _context
            .Stock.Include((s) => s.Comments)
            .ThenInclude((c) => c.AppUser)
            .FirstOrDefaultAsync((s) => s.Id == id);
    }

    public async Task<Stock?> GetBySymbolAsync(string symbols)
    {
        return await _context
            .Stock.Include((s) => s.Comments)
            .ThenInclude((c) => c.AppUser)
            .FirstOrDefaultAsync((s) => s.Symbol == symbols);
    }

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
        await _context.Stock.AddAsync(stockModel);
        await _context.SaveChangesAsync();

        return stockModel;
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockDto updatedStock)
    {
        Stock? existingStock = await _context.Stock.FirstOrDefaultAsync((s) => s.Id == id);

        if (existingStock is null)
        {
            return null;
        }

        var stock = updatedStock.ToStock(id);

        _context.Stock.Entry(existingStock).CurrentValues.SetValues(stock);
        await _context.SaveChangesAsync();

        return stock;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        Stock? existingStock = await _context.Stock.FirstOrDefaultAsync((stock) => stock.Id == id);

        if (existingStock is null)
        {
            return null;
        }

        _context.Stock.Remove(existingStock);
        await _context.SaveChangesAsync();

        return existingStock;
    }

    public async Task<bool> StockExists(int id)
    {
        return await _context.Stock.AnyAsync((s) => s.Id == id);
    }
}
