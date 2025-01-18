using Api.Dtos.Stock;
using Api.Helpers;
using Api.Models;

namespace Api.Interfaces;

public interface IStockRepo
{
    Task<(int, List<Stock>)> GetAllAsync(QueryObject query);
    Task<Stock?> GetByIdAsync(int id);

    Task<Stock?> GetBySymbolAsync(string symbol);
    Task<Stock> CreateAsync(Stock stockModel);
    Task<Stock?> UpdateAsync(int id, UpdateStockDto stockDto);

    Task<Stock?> DeleteAsync(int id);

    Task<bool> StockExists(int id);
}
