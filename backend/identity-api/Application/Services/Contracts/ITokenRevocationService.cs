
namespace backend.Application.Services.Contracts
{
    /// <summary>
    /// Manages the Redis-backed token revocation state used by both Identity and Gateway.
    ///
    /// Two-mechanism revocation:
    ///   1. perm_hash  — one key per user (userhash:{userId}). Written on role permission change.
    ///                   Gateway compares token's perm_hash claim against this. O(1) check.
    ///   2. JTI blocklist — one key per active token (jti:{userId}:{jti}). Written at login.
    ///                      Moved to blocklist:{jti} on explicit logout or permission change.
    ///                      Gateway checks GET blocklist:{jti}. O(1) check.
    /// </summary>
    public interface ITokenRevocationService
    {
        /// <summary>Records an issued JTI so it can be revoked later. TTL matches token lifetime.</summary>
        Task TrackJtiAsync(Guid userId, string jti, TimeSpan tokenLifetime);

        /// <summary>Immediately revokes a single token by moving its JTI to the blocklist.</summary>
        Task RevokeJtiAsync(string jti, TimeSpan remainingLifetime);

        /// <summary>
        /// Revokes all active tokens for a user by scanning their JTI keys and blocklisting them.
        /// Called when a role's permissions change.
        /// </summary>
        Task RevokeAllUserTokensAsync(Guid userId, TimeSpan remainingLifetime);

        /// <summary>
        /// Writes (or updates) the canonical permissions hash for a user.
        /// Gateway compares the token's perm_hash claim against this on every request.
        /// </summary>
        Task SetPermissionsHashAsync(Guid userId, string permissionsHash, TimeSpan ttl);

        /// <summary>Returns the stored permissions hash for a user, or null if not set.</summary>
        Task<string?> GetPermissionsHashAsync(Guid userId);

        /// <summary>Returns true if the JTI is on the blocklist (token is revoked).</summary>
        Task<bool> IsJtiRevokedAsync(string jti);
    }
}
