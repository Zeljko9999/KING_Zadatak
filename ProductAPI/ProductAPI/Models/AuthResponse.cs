namespace ProductAPI.Models
{
    public class AuthResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Image { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
