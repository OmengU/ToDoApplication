using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.Contracts;
using System.Formats.Asn1;
using ToDoAPI.Models;

namespace ToDoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoRepository _repository;

        public ToDoController(IToDoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<ToDo>> GetAllToDos()
        {
            try
            {
                var result = await _repository.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDo>> GetToDosById(Guid id)
        {
            try
            {
                var result = await _repository.GetByID(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<ToDo>> CreateToDo([FromBody]ToDoDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                ToDo toto = await _repository.CreateToDo(dto);
                return Ok(toto);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ToDo>> Delete(Guid id)
        {
            try
            {
                await _repository.DeleteToDo(id);
                return NoContent();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult<ToDo>> SetCompleted (Guid id)
        {
            try
            {
                var todo = await _repository.SetCompleted(id);
                if(todo == null)
                {
                    return NotFound();
                }
                await _repository.SetCompleted(id);
                return Ok(todo);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}