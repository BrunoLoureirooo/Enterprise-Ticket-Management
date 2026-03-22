using Microsoft.AspNetCore.Mvc;

namespace teams.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected Guid GetUserId()
        {
            var header = Request.Headers["X-User-Id"].FirstOrDefault();
            return Guid.TryParse(header, out var id) ? id : Guid.Empty;
        }
    }
}
