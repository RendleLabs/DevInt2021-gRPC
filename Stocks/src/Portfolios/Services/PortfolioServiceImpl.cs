using Grpc.Core;
using Portfolios.Data;
using Portfolios.Protos;

namespace Portfolios.Services;

internal partial class PortfolioServiceImpl : PortfolioService.PortfolioServiceBase
{
    private readonly IPortfolioRepo _portfolioRepo;
    private readonly ILogger<PortfolioServiceImpl> _logger;

    public PortfolioServiceImpl(IPortfolioRepo portfolioRepo, ILogger<PortfolioServiceImpl> logger)
    {
        _portfolioRepo = portfolioRepo;
        _logger = logger;
    }
    
    /// <summary>
    /// Gets a Portfolio
    /// </summary>
    /// <param name="request">The Protobuf request message</param>
    /// <param name="context">Context for the call, including the underlying HttpContext</param>
    /// <returns>The Protobuf response message</returns>
    /// <exception cref="RpcException">Throws RpcException to report errors</exception>
    public override async Task<GetPortfolioResponse> GetPortfolio(GetPortfolioRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var id))
        {
            var ipAddress = context.GetHttpContext().Connection.RemoteIpAddress?.ToString() ?? "?.?.?.?";
            LogBadRequest(ipAddress);
            throw new RpcException(new Status(StatusCode.InvalidArgument, "GetPortfolioRequest ID should be a UUID"));
        }

        var portfolio = await _portfolioRepo.GetAsync(id);

        if (portfolio is null)
        {
            LogNotFound(request.Id);
            throw new RpcException(new Status(StatusCode.NotFound, "Portfolio not found"));
        }

        var response = new GetPortfolioResponse
        {
            Portfolio = new Protos.Portfolio
            {
                Id = portfolio.Id.ToString("D"),
                Stocks =
                {
                    portfolio.Stocks.Select(s => new Protos.Stock
                    {
                        Code = s.Code,
                        Name = s.Name,
                        QuantityHeld = s.QuantityHeld
                    })
                }
            }
        };

        return response;
    }

    [LoggerMessage(0, LogLevel.Warning, "Portfolio not found: {Id}")]
    partial void LogNotFound(string id);

    [LoggerMessage(1, LogLevel.Warning, "Bad request from {ClientIP}")]
    partial void LogBadRequest(string clientIp);
}