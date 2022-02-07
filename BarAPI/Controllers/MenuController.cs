using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DataAccess.Data;
using DataAccess.Repository;
using System.Collections.Generic;


namespace BarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuRepository _menuRepository;

        public MenuController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        // GET: api/<MenuController>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<MenuItem>))]
        public async Task<List<MenuItem>> GetAll()
        {
            return await _menuRepository.GetAllItems();
        }

        // GET api/<MenuController>/id
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(MenuItem))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id)
        {
            MenuItem menuItem = await _menuRepository.GetItem(id);
            if(menuItem == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(menuItem);
            }
        }

        // POST api/<MenuController>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MenuItem))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] MenuItem item)
        {
            if(item == null)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            MenuItem exist = await _menuRepository.GetItem(item.Id);
            if (exist != null)
            {
                return BadRequest($"Item with id= {item.Id} already exists");
            }
            MenuItem menuItem = await _menuRepository.Add(item);
            return CreatedAtAction(nameof(Post), menuItem);
        }

        // PUT api/<MenuController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Update(int id, [FromBody] MenuItem item)
        {
            if(item.Id != id)
            {
                return BadRequest();
            }
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _menuRepository.Update(item);
            return new NoContentResult();
        }

        // DELETE api/<MenuController>/id
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var exist = await _menuRepository.GetItem(id);
            if(exist == null)
            {
                return NotFound();
            }
            bool deleted = await _menuRepository.Remove(id);
            if (deleted)
            {
                return new NoContentResult();
            }
            else
            {
                return BadRequest($"Item with id={id} was found but faild to delete");
            }
        }
    }
}
