

using Microsoft.AspNetCore.Mvc;
using TwitterServer.Repositories;
using TwitterServer.Utilities;
using TwitterService.DTOs;
using TwitterService.Models;

namespace TwitterServer.Controllers;
[ApiController]
[Route("api/post")]
public class PostController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private IConfiguration _configuration;
    private IPostRepository _post;

    public PostController(ILogger<UserController> logger, IPostRepository post, IConfiguration configuration)
    {
        _logger = logger;
        _post = post;
        _configuration = configuration;
    }
    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost([FromBody] PostCreateDTO data)
    {
        var currentUserId = GetCurrentUserId();
        var postCount = (await _post.GetPostByUserId(currentUserId)).Count;
        if (postCount >= 5)
            return BadRequest("You can only create 5 posts");
        var toCreatePost = new Post
        {
            Title = data.Title,
            UserId = currentUserId,


        };
        var createdPost = await _post.Create(toCreatePost);
        return StatusCode(StatusCodes.Status201Created, "Post Created");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdatePost(long id, [FromBody] PostUpdateDTO post)
    {
        var existing = await _post.GetPostById(id);
        var currentUserId = GetCurrentUserId();
        if (currentUserId != existing.UserId)
            return Unauthorized("Your not authorized to update.");
        if (existing == null)
            return NotFound("Post not found");
        var toUpdatePost = existing with
        {
            Title = post.Title ?? existing.Title,
        };
        var didUpdate = await _post.Update(toUpdatePost);
        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update Post");
        return Ok("Updated");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost(long id)
    {
        var post = await _post.GetPostById(id);
        var currentUserId = GetCurrentUserId();
        if (currentUserId != post.UserId)
            return Unauthorized("Your not authorized to delete.");

        if (post == null)
            return NotFound("Todo not found");

        var didDelete = await _post.Delete(id);

        if (!didDelete)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not delete post");
        return Ok("Deleted");
    }

    [HttpGet]
    public async Task<ActionResult<List<Post>>> GetAllPosts()
    {
        var allPost = await _post.GetAllPosts();
        return Ok(allPost);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetPostById(long id)
    {
        var post = await _post.GetPostById(id);
        return Ok(post);
    }
    private long GetCurrentUserId()
    {
        var userClaims = User.Claims;
        return Int64.Parse(userClaims.FirstOrDefault(c => c.Type == UserConstants.Id)?.Value);
    }


}