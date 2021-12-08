namespace Portfolios.Data;

public interface IPortfolioRepo
{
    Task<Portfolio?> GetAsync(Guid id);
}