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
        [HttpGet("Products")]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return _context.Products.ToList();
        }

        [HttpPost("Product")]
        public ActionResult<Product> CreateContact(Product contact)
        {
            if (contact == null)
            {
                return BadRequest("Invalid customer data.");
            }
            try
            {
                _context.Products.Add(contact);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetProducts), new { id = contact.ProductId }, contact);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to adding Product.");
            }
        }
    }
}
