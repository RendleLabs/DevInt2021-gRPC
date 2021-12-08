using Grpc.Net.Client;
using Stocks.Protos;

namespace Client;

public class MainViewModel
{
    private readonly PortfolioService.PortfolioServiceClient _portfolioServiceClient;
    
    public MainViewModel()
    {
        var channel = GrpcChannel.ForAddress("https://localhost:5001");
        _portfolioServiceClient = new PortfolioService.PortfolioServiceClient(channel);
    }
}