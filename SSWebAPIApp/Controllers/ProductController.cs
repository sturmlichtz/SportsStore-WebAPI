using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SSWebAPIApp.Models.Abstract;
using SSWebAPIApp.Models.Entities;

namespace SSWebAPIApp.Controllers
{
  [ApiController]
  [Produces("application/json")]
  [Route("api/product")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "RAdmin")]
  public class ProductController: ControllerBase
  {
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
      _productRepository = productRepository;
    }

    [HttpGet, Route("")] // http://localhost:5000/api/product
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get()
    {
      var products = await _productRepository.GetProductsAsync();
      if (products != null)
      {
        return Ok(products);
      }
      return NoContent();
    }

    [HttpGet, Route("id/{id}")] // http://localhost:5000/api/product/id/2
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get(int id)
    {
      var product = await _productRepository.GetProductByIdAsync(id);
      if (product != null)
      {
        return Ok(product);
      }
      return NoContent();
    }

    [HttpGet, Route("category/{category}")] // http://localhost:5000/api/product/category/chess
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Get(string category)
    {
      var products = await _productRepository.GetProductsByCategoryAsync(category);
      if (products != null)
      {
        return Ok(products);
      }
      return NoContent();
    }


    [HttpPost, Route("")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Product))]
    public async Task<IActionResult> Post([FromBody] Product product)
    {
      var newProduct = await _productRepository.AddProductAsync(product);
      if (newProduct != null)
      {
        return Ok(product);
      }
      return BadRequest($"New Product could not be created....");
    }


    [HttpPut, Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
    public async Task<IActionResult> Put([FromBody] Product product)
    {
      var updatedProduct = await _productRepository.UpdateProductAsync(product);
      if (updatedProduct != null)
      {
        return Ok(updatedProduct);
      }
      return BadRequest($"Could not update the Product....");
    }


    [HttpDelete, Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
    public async Task<IActionResult> Delete(long id)
    {
      bool flag = await _productRepository.DeleteProductAsync(id);
      if (flag)
      {
        //return RedirectToAction("Get");
        return Ok($"Product with the Id: '{id}' deleted successfully");
      }
      return NotFound($"No Product found with the Id: '{id}'");
    }
  }
}
