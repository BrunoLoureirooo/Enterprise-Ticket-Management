using ticket.Entities.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ticket.API.Controllers
{
    public class PermissionController : BaseApiController
    {
        [HttpGet]
        public IActionResult GetAll([FromServices] EndpointDataSource endpointDataSource)
        {
            var permissions = endpointDataSource.Endpoints
                .OfType<RouteEndpoint>()
                .SelectMany(e =>
                {
                    var methods = e.Metadata.GetMetadata<HttpMethodMetadata>()?.HttpMethods ?? [];
                    var route = e.RoutePattern.RawText ?? "";
                    return methods.Select(m => PermissionResolver.Resolve(route, m));
                })
                .Where(p => p != null && p != "anonymous"
                         && !p.StartsWith("permission.")
                         && !p.StartsWith("auth.")
                         && !p.StartsWith("health."))
                .Distinct()
                .OrderBy(p => p)
                .ToList();

            return Ok(permissions);
        }
    }
}
