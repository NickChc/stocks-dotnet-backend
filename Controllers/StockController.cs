using Api.Dtos.Stock;
using Api.Helpers;
using Api.Interfaces;
using Api.Mappers;
using Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController(IStockRepo stockRepo) : ControllerBase
{
    private readonly IStockRepo _stockRepo = stockRepo;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
    {
        var (total, stocks) = await _stockRepo.GetAllAsync(query);

        var response = new
        {
            TotalCount = total,
            Data = stocks.Select((stock) => stock.ToStockDto()),
        };

        return Ok(response);
    }

    [HttpGet("{id:int}", Name = "GetById")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Stock? stock = await _stockRepo.GetByIdAsync(id);

        if (stock is null)
        {
            return NotFound();
        }

        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockDto createStockDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Stock stock = createStockDto.ToStock();
        await _stockRepo.CreateAsync(stock);

        return CreatedAtRoute(nameof(GetById), new { id = stock.Id }, stock.ToStockDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        [FromRoute] int id,
        [FromBody] UpdateStockDto updatedStock
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Stock? existingStock = await _stockRepo.UpdateAsync(id, updatedStock);

        if (existingStock is null)
        {
            return NotFound();
        }

        Stock stock = updatedStock.ToStock(id);

        return Ok(stock.ToStockDto());
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var stock = await _stockRepo.DeleteAsync(id);

        if (stock is null)
        {
            return NotFound();
        }

        return NoContent();
    }
}
