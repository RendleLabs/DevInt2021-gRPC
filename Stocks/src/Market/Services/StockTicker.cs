using Market.Protos;

namespace Market.Services;

public class StockTicker
{
    private readonly List<StockPriceUpdate> _updates = new();
    private readonly object _mutex = new();

    public StockPriceUpdate Add(string code)
    {
        lock (_mutex)
        {
            var update = _updates.FirstOrDefault(c => c.Code == code);
            if (update is null)
            {
                update = new StockPriceUpdate
                {
                    Code = code,
                    Price = Random.Shared.NextSingle() * 100f + 300f
                };
                _updates.Add(update);
            }

            return update;
        }
    }

    public StockPriceUpdate GetRandom()
    {
        lock (_mutex)
        {
            int random = Random.Shared.Next(_updates.Count);
            var update = _updates[random];
            var delta = Random.Shared.NextSingle() * 10f - 5f;
            
            update.Price = Math.Max(update.Price + delta, 0f);
            return update;
        }
    }
}