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

    // Obter todos os produtos
    [HttpGet()]
    public async Task<IActionResult> Index()
    {
        var produtos = await _context.Products.ToListAsync();
        return View(produtos);
    }

    // Ver pagina de produto
    [HttpGet("{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var produto = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (produto == null)
            return NotFound();
        return View(produto);
    }

    // Calculo de Estatisticas dos Produtos
    [HttpGet("stats")]
    public async Task<IActionResult> Stats()
    {
        var products = await _context.Products.ToListAsync();

        var totalProducts = products.Count();
        var averagePrice = products.Any() ? products.Average(p => p.Price) : 0;
        var inStockCount = products.Count(p => p.InStock);
        var outOfStockCount = products.Count(p => !p.InStock);

        var stats = new
        {
            TotalProducts = totalProducts,
            AveragePrice = averagePrice,
            InStockCount = inStockCount,
            OutOfStockCount = outOfStockCount
        };

        return View(stats);
    }




    // APIS
    // Todos os Produtos
    [HttpGet("api/v1")]
    public async Task<IActionResult> TodosProdutos()
    {
        var produtos = await _context.Products.ToListAsync();
        return Json(produtos);
    }

    // Produtos por Status
    [HttpGet("api/v1/em-stock/{status}")]
    public async Task<IActionResult> ProdutosPorStock(bool status)
    {
        var filteredProducts = await _context.Products.Where(p => p.InStock == status).ToListAsync();
        return Json(filteredProducts);
    }

    // Produtos Acima de um Valor
    [HttpGet("api/v1/acima-preco/{minPrice}")]
    public async Task<IActionResult> ProdutosAcimaPreco(double minPrice)
    {
        var filteredProducts = await _context.Products.Where(p => p.Price >= minPrice).ToListAsync();
        return Json(filteredProducts);
    }

    // Obter produto por ID
    [HttpGet("api/v1/{id}")]
    public async Task<IActionResult> ProdutoIndividual(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
            return NotFound();
        return Json(product);
    }

    // Adicionar Produto
    [HttpPost("api/v1")]
    public async Task<IActionResult> AdicionarProduto([FromBody] Product produto)
    {
        if (produto == null)
            return BadRequest();
        
        _context.Products.Add(produto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(ProdutoIndividual), new { id = produto.Id }, produto);
    }

    // Alterar Produto
    [HttpPut("api/v1/{id}")]
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
    [HttpDelete("api/v1/{id}")]
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
