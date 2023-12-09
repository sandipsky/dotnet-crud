using System.Text.Json.Serialization;

namespace crud.ViewModels
{
    public class RegisterViewModel
    {

        [JsonPropertyName("user_name")]    
        public string UserName { get; set; } = null!;
        [JsonPropertyName("password")]    
        public string Password { get; set; } = null!;
        [JsonPropertyName("full_name")]    
        public string Name { get; set; }
    }
}