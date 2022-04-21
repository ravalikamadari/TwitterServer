

namespace TwitterService.Models;
public record Post
{
    public long Id { get; set; }
    public string Title { get; set; }
    public long UserId { get; set; }
    public string CreatedAt { get; set; }
    public string UpdatedAt { get; set; }
    // public ICollection<Comment> Comments { get; set; }
}