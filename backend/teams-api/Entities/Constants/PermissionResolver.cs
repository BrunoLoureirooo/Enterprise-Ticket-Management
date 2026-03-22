namespace teams.Entities.Constants
{
    public static class PermissionResolver
    {
        public static string? Resolve(string path, string method)
        {
            var parts = path.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 1) return null;

            if (parts[0].ToLower() == "swagger")
                return "anonymous";

            if (parts.Length < 2) return null;

            var resource = parts[1].ToLower();

            return method switch
            {
                "GET"            => $"{resource}.read",
                "POST"           => $"{resource}.create",
                "PUT" or "PATCH" => $"{resource}.update",
                "DELETE"         => $"{resource}.delete",
                _                => null
            };
        }
    }
}
