
using Dapper;
using TwitterServer.Utilities;
using TwitterService.Models;

namespace TwitterServer.Repositories;


public interface IPostRepository
{
    Task<Post> Create(Post Data);
    Task<Post> GetPostById(long Id);
    Task<bool> Update(Post toUpdatePost);
    Task<bool> Delete(long Id);
    Task<List<Post>> GetAllPosts();
    Task<List<Post>> GetPostByUserId(long currentUserId);
}
public class PostRepository : BaseRepository, IPostRepository
{
    public PostRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<Post> Create(Post Data)
    {
        var query = $@"INSERT INTO ""{TableNames.post}"" (title, user_id)
        VALUES(@Title,@UserId) RETURNING *";
        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Post>(query, Data);
    }

    public async Task<bool> Delete(long Id)
    {
        var query = $@"DELETE FROM {TableNames.post} WHERE id = @Id";

        using (var con = NewConnection)
            return (await con.ExecuteAsync(query, new { Id })) > 0;

    }

    public async Task<List<Post>> GetAllPosts()
    {
        var query = $@"SELECT * FROM {TableNames.post} ORDER BY created_at DESC";

        using (var con = NewConnection)
            return (await con.QueryAsync<Post>(query)).AsList();

    }

    public async Task<Post> GetPostById(long Id)
    {
        var query = $@"SELECT * FROM {TableNames.post} WHERE id = @Id";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Post>(query, new { Id });
    }

    public async Task<List<Post>> GetPostByUserId(long currentUserId)
    {
        var query = $@"SELECT * FROM {TableNames.post} WHERE user_id = @UserId";

        using (var con = NewConnection)
            return (await con.QueryAsync<Post>(query, new { UserId = currentUserId })).AsList();
    }

    public async Task<bool> Update(Post toUpdatePost)
    {
        var query = $@"UPDATE {TableNames.post} SET title = @Title, updated_at = NOW()
         WHERE id = @Id";

        using (var con = NewConnection)
            return (await con.ExecuteAsync(query, toUpdatePost)) > 0;
    }
}