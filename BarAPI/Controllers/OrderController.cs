using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Data;
using DataAccess.Repository;

namespace BarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: api/<OrderController>/id
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Order))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id)
        {
            Order order = await _orderRepository.Get(id);
            if(order == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(order);
            }
        }
        
        // POST: api/<OrderController>/
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Order))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Upsert([FromBody] Order item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            int affacted = await _orderRepository.Upsert(item);
            if(affacted == -201)
            {
                return CreatedAtAction(nameof(Upsert), item);
            }
            if(affacted == -204)
            {
                return new NoContentResult();
            }
            return BadRequest();
        }
    }
}
