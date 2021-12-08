using Grpc.Core;
using Market.Protos;

namespace ClientApp.Services;

public class StockTicker : IAsyncDisposable
{
    private readonly MarketService.MarketServiceClient _marketService;
    private readonly object _mutex = new();
    private AsyncDuplexStreamingCall<AddStockWatch,StockPriceUpdate> _call;
    private Task _tickTask;

    public StockTicker(MarketService.MarketServiceClient marketService)
    {
        _marketService = marketService;
    }

    ~StockTicker()
    {
        _call.Dispose();
    }

    public event Func<StockTick, ValueTask> Tick;

    public async Task AddAsync(string code)
    {
        EnsureCall();
        await _call.RequestStream.WriteAsync(new AddStockWatch { Code = code });
    }

    private async Task WatchAsync(IAsyncStreamReader<StockPriceUpdate> responseStream)
    {
        await foreach (var update in responseStream.ReadAllAsync())
        {
            var tick = Tick;
            if (tick is not null)
            {
                await tick(new StockTick(update.Code, update.Price));
            }
        }
    }

    private void EnsureCall()
    {
        lock (_mutex)
        {
            if (_call is not null) return;
            
            _call = _marketService.StockWatch();
            _tickTask = Task.Run(() => WatchAsync(_call.ResponseStream));
        }
    }

    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        await _call.RequestStream.CompleteAsync();
        _call.Dispose();
    }
}

public record struct StockTick(string Code, float Price);