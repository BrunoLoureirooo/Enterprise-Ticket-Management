namespace backend.Entities.Constants
{
    public static class PermissionResolver
    {
        public static string? Resolve(string path, string method)
        {
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 1) return null;

            // Swagger UI doc fetches are always public (dev tooling)
            if (parts[0].ToLower() == "swagger")
                return "anonymous";

            if (parts.Length < 2) return null;

            var resource = parts[1].ToLower();

            if (resource == "auth")
            {
                var action = parts.Length > 2 ? parts[2].ToLower() : "";
                if (action == "login" || action == "register")
                {
                    return "anonymous";
                }
            }
            
            return method switch
            {
                "GET" => $"{resource}.read",
                "POST" => $"{resource}.create",
                "PUT" or "PATCH" => $"{resource}.update",
                "DELETE" => $"{resource}.delete",
                _ => null
            };
        }
    }
}
