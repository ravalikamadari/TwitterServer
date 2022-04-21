

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TwitterServer.DTOs
{
    public class UserDTO
    {
        [JsonPropertyName("id")]

        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }

    public class UserLoginDTO
    {
        [JsonPropertyName("email")]
        [Required]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        [Required]
        public string Password { get; set; }
    }
    public class UserLoginResponseDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }

    }


    public class UserCreateDTO
    {
        [JsonPropertyName("name")]
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
    }


    public class UserUpdateDTO
    {
        [JsonPropertyName("name")]
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }


}