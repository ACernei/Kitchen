using Kitchen.Models;

namespace Kitchen.Services;

public interface IOrderService
{
    public Task PostAsync(Order order);
}
