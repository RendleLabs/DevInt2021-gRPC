using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Portfolios.Data;
using Portfolios.Protos;

namespace Portfolios.IntegrationTests;

public class PortfoliosApplicationFactory : WebApplicationFactory<IPortfolioRepo>
{
    public PortfolioService.PortfolioServiceClient CreateGrpcClient()
    {
        var httpClient = CreateClient();
        var channel = GrpcChannel.ForAddress(httpClient.BaseAddress, new GrpcChannelOptions
        {
            HttpClient = httpClient,
            DisposeHttpClient = true
        });
        return new PortfolioService.PortfolioServiceClient(channel);
    }
}