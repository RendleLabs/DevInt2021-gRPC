namespace Portfolios.Data;

public class Portfolio
{
    public Guid Id { get; init; }
    public ICollection<Stock> Stocks { get; init; } = Array.Empty<Stock>();
}