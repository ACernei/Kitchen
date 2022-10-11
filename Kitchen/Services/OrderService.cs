using Kitchen.Models;

namespace Kitchen.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient httpClient;
    private readonly ILogger<OrderService> logger;
    
    public OrderService(HttpClient httpClient, ILogger<OrderService> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async Task PostAsync(Order order)
    {
        using var response = await httpClient.PostAsJsonAsync("distribution", order);
        // this.logger.LogInformation(response.ToString());
    }
}
