using System.Security.Claims;

namespace backend.Application.Services.Contracts
{
    /// <summary>
    /// Enriches a JWT with contextual attributes fetched from external services
    /// at login time (e.g. department_id from HR service, org_id from tenant service).
    ///
    /// Pattern: Identity calls each registered enricher after validating credentials.
    /// Each enricher returns additional claims — typically contextual attributes
    /// that live in domain services, not in the Identity store.
    ///
    /// To add a new attribute source, implement this interface and register it
    /// with DI. No changes to AuthService required.
    /// </summary>
    public interface IClaimEnricher
    {
        /// <summary>
        /// Returns additional claims to embed in the JWT for the given user.
        /// Called once per login / token refresh.
        /// </summary>
        Task<IEnumerable<Claim>> EnrichAsync(Guid userId);
    }
}
