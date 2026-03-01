using backend.Application.Services.Contracts;
using System.Security.Claims;

namespace backend.Application.Services
{
    /// <summary>
    /// No-op enricher registered by default. Replace with a real implementation
    /// once an HR / Profile service exists that owns department and org data.
    ///
    /// Example real implementation (in a future HR service client):
    ///
    ///   public class HrServiceClaimEnricher : IClaimEnricher
    ///   {
    ///       public async Task&lt;IEnumerable&lt;Claim&gt;&gt; EnrichAsync(Guid userId)
    ///       {
    ///           var profile = await _hrClient.GetProfileAsync(userId);
    ///           return new[]
    ///           {
    ///               new Claim("dept",   profile.DepartmentId),
    ///               new Claim("org_id", profile.OrganisationId),
    ///           };
    ///       }
    ///   }
    /// </summary>
    public class NullClaimEnricher : IClaimEnricher
    {
        public Task<IEnumerable<Claim>> EnrichAsync(Guid userId)
            => Task.FromResult(Enumerable.Empty<Claim>());
    }
}
