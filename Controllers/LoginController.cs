using Microsoft.AspNetCore.Mvc;
using OEMS.Server.DatabaseContext;
using OEMS.Server.Models;

namespace OEMS.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly DbContext _dbContext;

        public LoginController(ILogger<LoginController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _dbContext = httpContextAccessor.HttpContext?.RequestServices.GetService(typeof(DbContext)) as DbContext
                     ?? throw new InvalidOperationException("DbContext not available.");
        }

        [HttpPost]
        public ActionResult Login([FromBody] LoginModel model)
        {
            string email = _dbContext.IsValidUser(model);

            if(email == "")
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
