using App.Core;
using App.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {        

        private readonly ILogger<TestController> _logger;
        private readonly IRepository<BaseEntity> _repo;

        public TestController(ILogger<TestController> logger, IRepository<BaseEntity> repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        [Produces("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IBaseEntity))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var all = await _repo.GetAllAsync();
            if(all == null)
            {
                _logger.LogError("Database empty");
                return BadRequest("Database empty");
            }
            _logger.LogInformation("All items");
            return Ok(all);
        }

        [HttpGet("{id}")]
        [Produces("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IBaseEntity))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var item = await _repo.GetAsync(id);
            if (item == null)
            {
                _logger.LogError("Wrong id of entity");
                return BadRequest("Wrong id of entity");
            }
            _logger.LogInformation(string.Format("Returned {0}", typeof(IBaseEntity)), item);
            return Ok(item);
        }

        /// <summary>
        /// Creates new item
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Ok if success, 404 if fail</returns>
        [HttpPost]
        [Produces("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IBaseEntity))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] BaseEntity item)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Model");
                return BadRequest("Invalid Model");
            }

            var returned =  _repo.Add(item);
            if (await _repo.SaveAsync())
            {
                _logger.LogInformation("Item created successfuly");
                return CreatedAtAction("GetById", new { id = returned.Id }, returned);
            }

            _logger.LogError("Something went wrong during add item");
            throw new Exception("Adding item failed");
        }

        [HttpPut("{id}")]
        [Produces("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IBaseEntity))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] IBaseEntity item)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Model");
                return BadRequest("Invalid Model");
            }

            if (id != item.Id)
                return BadRequest();

            if (await _repo.SaveAsync())
            {
                _logger.LogInformation("Item created successfuly");
                return Ok();
            }

            _logger.LogError("Something went wrong during update item");
            throw new Exception("Update item failed");
        }

        [HttpDelete("{id}")]
        [Produces("application/json", "application/xml")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IBaseEntity))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Model");
                return BadRequest("Invalid Model");
            }

            var itemToDelete = await _repo.GetAsync(id);

            if (await _repo.SaveAsync())
            {
                _logger.LogError("Entity to delete doesn't exist");
                return BadRequest("Entity to delete doesn't exist");
            }

            _repo.Delete(itemToDelete);
            return NoContent();
        }
    }
}
