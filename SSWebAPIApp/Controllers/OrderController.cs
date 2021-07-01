using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SSWebAPIApp.Models.Abstract;
using SSWebAPIApp.Models.Entities;

namespace SSWebAPIApp.Controllers
{
  [ApiController]
  [Produces("application/json")]
  [Route("api/order")]
  public class OrderController : ControllerBase
  {
    private IOrderRepository _orderRepository;

    public OrderController(IOrderRepository orderRepository)
    {
      _orderRepository = orderRepository;
    }

    [HttpGet, Route("")] // http://localhost:5000/api/order
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Order>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get()
    {
      var orders = await _orderRepository.GetOrdersAsync();
      if (orders != null)
      {
        return Ok(orders);
      }
      return NoContent();
    }

    [HttpGet, Route("id/{id}")] // http://localhost:5000/api/order/id/1
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get(int id)
    {
      var order = await _orderRepository.GetOrderByIdAsync(id);
      if (order != null)
      {
        return Ok(order);
      }
      return NoContent();
    }

    [HttpPost, Route("")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Order))]
    public async Task<IActionResult> Post([FromBody] Order order)
    {
      var newOrder = await _orderRepository.SaveOrderAsync(order);
      if (newOrder != null)
      {
        return Ok($"New Order with Id: '{newOrder.OrderId}', created successfully");
      }
      return BadRequest($"Could not create the new order");
    }

    [HttpPut, Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
    public async Task<IActionResult> Put([FromBody] Order order)
    {
      var updatedOrder = await _orderRepository.SaveOrderAsync(order);
      if (updatedOrder != null)
      {
        return Ok($"Order with Id: '{updatedOrder.OrderId}', updated successfully");
      }
      return BadRequest($"Could not update the order with id: '{order.OrderId}'");
    }

    [HttpDelete, Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    public async Task<IActionResult> Delete(int id)
    {
      var flag = await _orderRepository.RemoveOrderAsync(id);
      if (flag)
      {
        return Ok($"Order with the id: '{id}' has been deleted successfully");
      }
      return NotFound($"Could not find the Order with the id: '{id}'");
    }
  }
}
