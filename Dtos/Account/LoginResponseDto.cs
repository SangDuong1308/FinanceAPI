namespace api.Dtos.Account
{
    public class LoginResponseDto
    {
        public required string Jwt {  get; set; }
        public DateTime Expiration { get; set; }
    }
}
