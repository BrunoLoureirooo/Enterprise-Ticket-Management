using backend.Application.Services.Contracts;
using StackExchange.Redis;

namespace backend.Application.Services
{
    public class TokenRevocationService : ITokenRevocationService
    {
        private readonly IDatabase _redis;

        public TokenRevocationService(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        // --- JTI tracking (written at login) ---

        public async Task TrackJtiAsync(Guid userId, string jti, TimeSpan tokenLifetime)
        {
            // Pattern: jti:{userId}:{jti} — scannable by userId on bulk revocation
            await _redis.StringSetAsync($"jti:{userId}:{jti}", "1", tokenLifetime);
        }

        public async Task RevokeJtiAsync(string jti, TimeSpan remainingLifetime)
        {
            await _redis.StringSetAsync($"blocklist:{jti}", "1", remainingLifetime);
        }

        public async Task RevokeAllUserTokensAsync(Guid userId, TimeSpan remainingLifetime)
        {
            // Scan for all active JTI keys for this user
            var server = _redis.Multiplexer.GetServer(_redis.Multiplexer.GetEndPoints().First());
            var pattern = $"jti:{userId}:*";

            await foreach (var key in server.KeysAsync(pattern: pattern))
            {
                // Extract the JTI from the key name
                var parts = ((string)key!).Split(':');
                if (parts.Length == 3)
                {
                    var jti = parts[2];
                    await _redis.StringSetAsync($"blocklist:{jti}", "1", remainingLifetime);
                    await _redis.KeyDeleteAsync(key);
                }
            }
        }

        // --- Permissions hash (written on login + permission change) ---

        public async Task SetPermissionsHashAsync(Guid userId, string permissionsHash, TimeSpan ttl)
        {
            await _redis.StringSetAsync($"userhash:{userId}", permissionsHash, ttl);
        }

        public async Task<string?> GetPermissionsHashAsync(Guid userId)
        {
            var value = await _redis.StringGetAsync($"userhash:{userId}");
            return value.HasValue ? (string?)value : null;
        }

        // --- Blocklist check (used by gateway) ---

        public async Task<bool> IsJtiRevokedAsync(string jti)
        {
            return await _redis.KeyExistsAsync($"blocklist:{jti}");
        }
    }
}
