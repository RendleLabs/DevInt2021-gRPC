namespace ClientApp.Models;

public class Portfolio
{
    public static Portfolio Empty = new();
    
    public Guid Id { get; init; }
    public ICollection<Stock> Stocks { get; init; } = Array.Empty<Stock>();
}