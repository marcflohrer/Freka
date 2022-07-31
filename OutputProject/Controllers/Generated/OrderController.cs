using Microsoft.AspNetCore.Mvc;
using OutputProject.Controllers.Models;

namespace OutputProject.Controllers.Generated;

[ApiController]
[Route("[controller]")]
public partial class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;

    public OrderController(ILogger<OrderController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetOrder")]
    public IEnumerable<Order> Get()
    {
        return GetOrder();
    }
}
