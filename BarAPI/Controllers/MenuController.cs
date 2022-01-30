using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Domain;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public Task<MenuItem[]> Get()
        {
            return _menuRepository.Get();
        }

        // GET api/<MenuController>/name
        [HttpGet("{name}")]
        public Task<MenuItem> Get(string name)
        {
            return _menuRepository.GetItem(name);
        }

        // POST api/<MenuController>
        [HttpPost]
        public async Task Post([FromBody] MenuItem item)
        {
            await _menuRepository.Add(item);
        }

        // PUT api/<MenuController>/5
        [HttpPut("{name}")]
        public void Put(string name, [FromBody] string value)
        {
            throw new NotImplementedException("Our bar does not allow to modify a menu! (yet)");
        }

        // DELETE api/<MenuController>/name
        [HttpDelete("{name}")]
        public async Task Delete(string name)
        {
            await _menuRepository.Remove(name);
        }
    }
}
