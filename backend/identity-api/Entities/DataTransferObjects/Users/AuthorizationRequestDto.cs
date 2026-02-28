namespace backend.Entities.DataTransferObjects.Users
{
    public class AuthorizationRequestDto
    {
        public string Token { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Method { get; set; } = string.Empty;
    }
}
