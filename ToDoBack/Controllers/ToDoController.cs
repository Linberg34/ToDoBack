using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoBack.Storage;
using ToDoBack.Models;


namespace ToDoBack.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoContext _context;

        public ToDoController(ToDoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetTodos()
        {
            return await _context.Todos.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ToDoItem>> PostToDoItem(ToDoItem todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodos), new { id = todo.Id }, todo);
        }

        [HttpPost("uploadList")]
        public async Task<IActionResult> UploadToDoList([FromBody] List<ToDoItem> todoList)
        {
            if (todoList == null || todoList.Count == 0)
            {
                return BadRequest();
            }

            _context.Todos.AddRange(todoList);
            await _context.SaveChangesAsync();

            return Ok(new {count = todoList.Count });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(int id, ToDoItem todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }
            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!_context.Todos.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpPut("{id}/done")]
        public async Task<IActionResult> MarkAsDone(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if(todo == null)
            {
                return NotFound();
            }
            todo.IsCompleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/undone")]
        public async Task<IActionResult> MarkAsUnDone(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            todo.IsCompleted = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }


}
