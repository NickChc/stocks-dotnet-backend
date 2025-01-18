using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

[Table("Portfolios")]
public class Portfolio
{
    public required string AppUserId { get; set; }
    public required int StockId { get; set; }
    public AppUser? AppUser { get; set; }
    public Stock? Stock { get; set; }
}
