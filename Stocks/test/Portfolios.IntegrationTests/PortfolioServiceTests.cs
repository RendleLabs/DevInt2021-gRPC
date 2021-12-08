using System.Threading.Tasks;
using Portfolios.Protos;
using Xunit;

namespace Portfolios.IntegrationTests;

public class PortfolioServiceTests : IClassFixture<PortfoliosApplicationFactory>
{
    private const string StubId = "179faf1e-7700-4149-944b-091c239d153a";
    private readonly PortfoliosApplicationFactory _applicationFactory;

    public PortfolioServiceTests(PortfoliosApplicationFactory applicationFactory)
    {
        _applicationFactory = applicationFactory;
    }

    [Fact]
    public async Task GetsPortfolio()
    {
        var client = _applicationFactory.CreateGrpcClient();
        var request = new GetPortfolioRequest
        {
            Id = StubId
        };

        var response = await client.GetPortfolioAsync(request);

        Assert.Equal(StubId, response.Portfolio.Id);
        Assert.Collection(response.Portfolio.Stocks,
            s => Assert.Equal("MSFT", s.Code),
            s => Assert.Equal("KO", s.Code),
            s => Assert.Equal("HD", s.Code)
            );
    }
}