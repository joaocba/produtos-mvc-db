using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdutosMVC.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProdutosMVC.Controllers;

[Route("product")]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Todos os Produtos
    [HttpGet]
    public async Task<IActionResult> TodosProdutos()
    {
        var produtos = await _context.Products.ToListAsync();
        return Json(produtos);
    }

    // Produtos por Status
    [HttpGet("em-stock/{status}")]
    public async Task<IActionResult> ProdutosPorStock(bool status)
    {
        var filteredProducts = await _context.Products.Where(p => p.InStock == status).ToListAsync();
        return Json(filteredProducts);
    }

    // Produtos Acima de um Valor
    [HttpGet("acima-preco/{minPrice}")]
    public async Task<IActionResult> ProdutosAcimaPreco(double minPrice)
    {
        var filteredProducts = await _context.Products.Where(p => p.Price >= minPrice).ToListAsync();
        return Json(filteredProducts);
    }

    // Obter produto por ID
    [HttpGet("{id}")]
    public async Task<IActionResult> ProdutoIndividual(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
            return NotFound();
        return Json(product);
    }

    // Adicionar Produto
    [HttpPost]
    public async Task<IActionResult> AdicionarProduto([FromBody] Product produto)
    {
        if (produto == null)
            return BadRequest();
        
        _context.Products.Add(produto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ProdutoIndividual), new { id = produto.Id }, produto);
    }

    // Alterar Produto
    [HttpPut("{id}")]
    public async Task<IActionResult> AlterarProduto(int id, [FromBody] Product updatedProduct)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
            return NotFound();
        if (updatedProduct == null)
            return BadRequest();

        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.Description = updatedProduct.Description;
        product.InStock = updatedProduct.InStock;

        await _context.SaveChangesAsync();

        return Ok(product);
    }

    // Remover Produto
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarProduto(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
            return NotFound();

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return Ok("Produto eliminado!");
    }
}
