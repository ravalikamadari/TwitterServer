using System.ComponentModel.DataAnnotations;

namespace TwitterServer.DTOs;

public class CommentDTO
{
    public long Id { get; set; }
    public string CommentText { get; set; }
    public long PostId { get; set; }
    public long UserId { get; set; }
    public DateTime CreatedAt { get; set; }
}
public class CommentCreateDTO
{
    [Required]
    [MaxLength(60)]

    public string CommentText { get; set; }


}
