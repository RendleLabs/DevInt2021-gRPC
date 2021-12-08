using System.Collections.Concurrent;
using Grpc.Core;
using Market.Protos;

namespace Market.Services;

public class MarketServiceImpl : MarketService.MarketServiceBase
{
    private readonly ILogger<MarketServiceImpl> _logger;

    public MarketServiceImpl(ILogger<MarketServiceImpl> logger)
    {
        _logger = logger;
    }

    public override async Task StockWatch(IAsyncStreamReader<AddStockWatch> requestStream, IServerStreamWriter<StockPriceUpdate> responseStream, ServerCallContext context)
    {
        var ticker = new StockTicker();
        var adds = AddAsync(requestStream, responseStream, ticker, context.CancellationToken);
        var ticks = TickAsync(responseStream, ticker, context.CancellationToken);
        await Task.WhenAll(adds, ticks);
    }

    private async Task AddAsync(IAsyncStreamReader<AddStockWatch> streamReader, IServerStreamWriter<StockPriceUpdate> responseStream, StockTicker ticker, CancellationToken token)
    {
        try
        {
            await foreach (var item in streamReader.ReadAllAsync(cancellationToken: token))
            {
                var update = ticker.Add(item.Code);
                await responseStream.WriteAsync(update);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Watch cancelled.");
        }
    }

    private async Task TickAsync(IServerStreamWriter<StockPriceUpdate> streamWriter, StockTicker ticker, CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(1, 5)), token);
                var update = ticker.GetRandom();
                await streamWriter.WriteAsync(update);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Watch cancelled.");
        }
    }
}