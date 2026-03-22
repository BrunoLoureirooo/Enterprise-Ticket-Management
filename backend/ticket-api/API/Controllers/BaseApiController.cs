using Microsoft.AspNetCore.Mvc;

namespace ticket.API.Controllers
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

        protected bool IsAdmin() =>
            Request.Headers["X-User-Is-Admin"].FirstOrDefault() == "true";
    }
}
