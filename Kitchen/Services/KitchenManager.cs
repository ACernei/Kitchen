using System.Collections.Concurrent;
using Kitchen.Models;
using Microsoft.Extensions.Options;

namespace Kitchen.Services;

public class KitchenManager : BackgroundService
{
    private readonly IOrderService orderService;
    private readonly KitchenOptions config;
    private readonly ILogger<KitchenManager> logger;
    private readonly ConcurrentQueue<Order> orderQueue;

    public KitchenManager(
        IOrderService orderService,
        IOptions<KitchenOptions> options,
        ILogger<KitchenManager> logger)
    {
        this.orderService = orderService;
        this.config = options.Value;
        this.logger = logger;
        
        this.orderQueue = new ConcurrentQueue<Order>();
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var cook in this.config.Cooks)
        {
            Task.Run(() => ProcessQueue(cook), stoppingToken);
        }
        return Task.CompletedTask;
    }

    public void AddOrder(Order order)
    {
        this.orderQueue.Enqueue(order);
        this.logger.LogInformation($"RECEIVED ORDER {order.Id}");
    }

    private async Task ProcessQueue(Cook cook)
    {
        while (true)
        {
            if (!this.orderQueue.TryDequeue(out var order))
                continue;
            var orderPreparationTime = order.Items
                .Sum(x => this.config.Menu.FirstOrDefault(m => m.Id == x)?.PreparationTime ?? 0);
            
            this.logger.LogInformation($"{cook.Name} IS PREPARING ORDER {order.Id} ({orderPreparationTime} TIME)");

            // cook prepares food
            await Task.Delay(orderPreparationTime * this.config.TimeUnit);
            await this.orderService.PostAsync(order);
        }
    }
}
