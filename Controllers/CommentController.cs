

using Microsoft.AspNetCore.Mvc;
using TwitterServer.DTOs;
using TwitterServer.Models;
using TwitterServer.Repositories;
using TwitterServer.Utilities;

namespace TwitterServer.Controllers;
[ApiController]
[Route("api/comment")]
public class CommentController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly ICommentRepository _comment;

    public CommentController(ILogger<UserController> logger, ICommentRepository comment)
    {
        _logger = logger;
        _comment = comment;
    }


    [HttpPost("{post_id}")]
    public async Task<ActionResult<Comment>> CreateComment([FromRoute] long post_id, [FromBody] CommentCreateDTO data)
    {
        var currentUserId = GetCurrentUserId();
        var toCreateComment = new Comment
        {
            UserId = currentUserId,
            PostId = post_id,
            CommentText = data.CommentText,
        };
        var createdComment = await _comment.Create(toCreateComment);
        return StatusCode(StatusCodes.Status201Created, "Comment Created");
    }


    [HttpGet("{post_id}")]
    public async Task<ActionResult<List<Comment>>> GetAllComments([FromRoute] long post_id)
    {
        var comments = await _comment.GetAllComments(post_id);
        return Ok(comments);
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteComment(long id)
    {
        var existing = await _comment.GetCommentById(id);
        var currentUserId = GetCurrentUserId();
        if (currentUserId != existing.UserId)
            return Unauthorized("Your not authorized to delete.");
        if (existing == null)
            return NotFound("Comment not found");
        var didDelete = await _comment.Delete(id);
        if (!didDelete)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not delete Comment");
        return Ok("Deleted");
        }

    private long GetCurrentUserId()
    {
        var userClaims = User.Claims;
        return Int64.Parse(userClaims.FirstOrDefault(c => c.Type == UserConstants.Id)?.Value);
    }
}
