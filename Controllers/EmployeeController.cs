using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutosMVC.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProdutosMVC.Controllers;

[Route("employee")]
public class EmployeeController : Controller
{
    private readonly ApplicationDbContext _context;

    public EmployeeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Obter todos os funcionários <- TODO
    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await _context.Employees.ToListAsync();
        return Json(employees);
    }

    // Obter funcionário por ID <- TODO
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null)
            return NotFound();
        return Json(employee);
    }

    // Adicionar funcionário <- TODO
    [HttpPost]
    public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
    {
        if (employee == null)
            return BadRequest();

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
    }

    // Atualizar Funcionário <- TODO
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee updatedEmployee)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null)
            return NotFound();
        if (updatedEmployee == null)
            return BadRequest();

        employee.FirstName = updatedEmployee.FirstName;
        employee.LastName = updatedEmployee.LastName;
        employee.Position = updatedEmployee.Position;
        employee.Salary = updatedEmployee.Salary;
        employee.HireDate = updatedEmployee.HireDate;

        await _context.SaveChangesAsync();

        return Ok(employee);
    }

    // Eliminar Funcionário <- TODO
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null)
            return NotFound();

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return Ok("Funcionário eliminado!");
    }
}
