using System.Text.Json.Serialization;

namespace crud.ViewModels
{
    public class LoginViewModel
    {

        [JsonPropertyName("user_name")]    
        public string UserName { get; set; } = null!;
        [JsonPropertyName("password")]    
        public string Password { get; set; } = null!;
    }
}