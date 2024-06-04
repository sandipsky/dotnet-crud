namespace DotnetCrud.DTOs
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string token { get; set; }
    }
}