namespace excel_to_analytics.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly Context _context;
        public CustomerController(Context context)
        {
            _context = context;
        }
        // GET: api/v1/Products
        [HttpGet("Products")]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return _context.Products.Include(p => p.Sales).ToList();
        }

        // POST: api/v1/Product
        [HttpPost("Product")]
        public ActionResult<Product> CreateProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest("Invalid product data.");
            }

            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetProducts), new { id = product.ProductId }, product);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new product record.");
            }
        }

        // POST: api/v1/Sale
        [HttpPost("Sale")]
        public ActionResult<Sale> CreateSale(Sale sale)
        {
            if (sale == null)
            {
                return BadRequest("Invalid sale data.");
            }

            try
            {
                _context.Sales.Add(sale);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetProducts), new { id = sale.SaleId }, sale);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new sale record.");
            }
        }
    }
}