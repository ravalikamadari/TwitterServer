
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TwitterService.DTOs
{
    public class PostDTO
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("user_id")]
        public long UserId { get; set; }
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; }

    }

    public class PostCreateDTO
    {
        [JsonPropertyName("title")]
        [Required]
        [MaxLength(90)]

        public string Title { get; set; }




    }
    public class PostUpdateDTO
    {
        [JsonPropertyName("title")]
        [Required]
        [MaxLength(90)]

        public string Title { get; set; }

    }
}