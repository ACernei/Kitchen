using Kitchen.Models;
using Kitchen.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Kitchen.Controllers;

[ApiController]
[Route("order")]
public class OrderController : ControllerBase
{
    private readonly KitchenManager kitchenManager;
    private readonly ILogger<OrderController> logger;

    public OrderController(
        KitchenManager kitchenManager,
        ILogger<OrderController> logger)
    {
        this.kitchenManager = kitchenManager;
        this.logger = logger;
    }

    [HttpPost]
    public IActionResult Post(Order order)
    {
        kitchenManager.AddOrder(order);
        return Ok();
    }
}
