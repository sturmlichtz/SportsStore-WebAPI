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
  [Route("api/orderdetail")]
  public class OrderDetailController : ControllerBase
  {
    private readonly IOrderDetailRepository _orderDetailRepository;

    public OrderDetailController(IOrderDetailRepository orderDetailRepository) => _orderDetailRepository = orderDetailRepository;

    [HttpGet, Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDetail>))]
    public async Task<IActionResult> Get()
    {
      var orderDetails = await _orderDetailRepository.GetOrderDetailsAsync();
      return Ok(orderDetails);
    }

    [HttpGet, Route("id/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDetail>))]
    public async Task<IActionResult> Get(int id)
    {
      var orderDetail = await _orderDetailRepository.GetOrderDetailAsync(id);
      if (orderDetail != null)
      {
        return Ok(orderDetail);
      }
      return NotFound($"No OrderDetail Data found for the id: '{id}'");
    }

    [HttpPost, Route("")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderDetail))]
    public async Task<IActionResult> Post([FromBody] OrderDetail orderDetail)
    {
      var newOrderDetail = await _orderDetailRepository.AddOrderDetailAsync(orderDetail);
      if (newOrderDetail != null)
      {
        return Ok(newOrderDetail);
      }
      return BadRequest($"Could not create new OrderDetail");
    }


    [HttpPut, Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDetail))]
    public async Task<IActionResult> Put([FromBody] OrderDetail orderDetail)
    {
      var updatedOrderDetail = await _orderDetailRepository.UpdateOrderDetailAsync(orderDetail);
      if (updatedOrderDetail != null)
      {
        return Ok(updatedOrderDetail);
      }
      return BadRequest($"Could not update the OrderDetail with the Id: {orderDetail.OrderId}");
    }

    [HttpDelete, Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    public async Task<IActionResult> Delete(int id)
    {
      var flag = await _orderDetailRepository.RemoveOrderDetailAsync(id);
      if (flag)
      {
        return Ok($"OrderDetail with the id: '{id}', has been deleted successfully");
      }
      return NotFound($"OrderDetail with the id: '{id}', could not be found");
    }
  }
}