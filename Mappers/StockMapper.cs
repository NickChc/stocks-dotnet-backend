using Api.Dtos.Stock;
using Api.Models;

namespace Api.Mappers;

public static class StockMappers
{
    public static StockDto ToStockDto(this Stock stock)
    {
        return new StockDto
        {
            Id = stock.Id,
            Symbol = stock.Symbol,
            CompanyName = stock.CompanyName,
            Industry = stock.Industry,
            LastDiv = stock.LastDiv,
            MarketCap = stock.MarketCap,
            Purchase = stock.Purchase,
            Comments = [.. stock.Comments.Select((c) => c.ToDto())],
        };
    }

    public static Stock ToStock(this CreateStockDto stockDto)
    {
        return new()
        {
            Symbol = stockDto.Symbol,
            CompanyName = stockDto.CompanyName,
            Purchase = stockDto.Purchase,
            LastDiv = stockDto.LastDiv,
            Industry = stockDto.Industry,
            MarketCap = stockDto.MarketCap,
        };
    }

    public static Stock ToStock(this UpdateStockDto stock, int id)
    {
        return new()
        {
            Id = id,
            Symbol = stock.Symbol,
            CompanyName = stock.CompanyName,
            Purchase = stock.Purchase,
            LastDiv = stock.LastDiv,
            Industry = stock.Industry,
            MarketCap = stock.MarketCap,
        };
    }
}
