namespace Portfolios.Data;

public class StubPortfolioRepo : IPortfolioRepo
{
    private const string StubId = "179faf1e-7700-4149-944b-091c239d153a";
    private static readonly List<Portfolio> Data = new()
    {
        new Portfolio
        {
            Id = new Guid(StubId),
            Stocks = new List<Stock>
            {
                new ("MSFT", "Microsoft Corporation", 1000),
                new ("KO", "Coca-Cola Co", 500),
                new ("HD", "Home Depo Inc", 750),
            }
        }
    };
    
    public Task<Portfolio?> GetAsync(Guid id) => Task.FromResult(Data.FirstOrDefault(p => p.Id == id));
}