using Microsoft.AspNetCore.Mvc;
using OutputProject.Controllers.Models;

namespace OutputProject.Controllers.Generated;

public partial class OrderController : ControllerBase
{
    private static readonly string[] Summaries = new[]
{
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    protected IEnumerable<Order> GetOrder()
    {
        var result = Enumerable.Range(1, 5).Select(index => new Order
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
        _logger.LogInformation(message: "Result {ResultCount}",
                               args: result.Length);
        return result;
    }
}
