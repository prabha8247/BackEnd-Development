using System.Linq;
using Employee.DBContext;
using Employee.Models;
using Microsoft.AspNetCore.Mvc;

namespace Employee.TodoControllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    private TodoContext _context;
    
    public TodoController(TodoContext context)
    {
        _context = context;
    }

    [HttpGet("GetTodoList")]
    public IActionResult GetTodoList()
    {
        return Ok(_context.Todos.ToList());
    }
    
    

    [HttpPost("CreateTodo")]
    public IActionResult CreateTodo([FromBody] Todo pTodo)
    {
        // var todo =  _context.Todos.Find(Id);
        //
        // if (todo != null) return Ok();
        
        var todoData = Todo.Create(pTodo.Name, pTodo.IsComplete);
        _context.Todos.Add(todoData);
         _context.SaveChanges();
        return Ok();

    }
    
    [HttpPost("UpdateTodo")]
    public IActionResult UpdateTod([FromBody] Todo pTodo)
    {
        var todo =  _context.Todos.FirstOrDefault(x => x.Id == pTodo.Id);
        //
        if (todo == null) return Ok(); // throw business error
        
        todo.UpdateName(pTodo.Name);
        todo.UpdateIsComplete(pTodo.IsComplete);
        _context.Todos.Update(todo);
        _context.SaveChanges();
        return Ok();

    }

    [HttpPost("DeleteTodo")]
    public IActionResult DeleteTodo([FromBody] int id)
    {
        var todo =  _context.Todos.FirstOrDefault(x => x.Id == id);
        //
        if (todo == null) return Ok(); // throw business error
        
        _context.Todos.Remove(todo);
        _context.SaveChanges();
        return Ok();
    }
}