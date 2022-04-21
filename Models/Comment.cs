

namespace TwitterServer.Models;

public record Comment{

    public long Id { get; set; }
    public string CommentText { get; set; }
    public long PostId { get; set; }
    public long UserId { get; set; }
    public DateTime CreatedAt { get; set; }

}