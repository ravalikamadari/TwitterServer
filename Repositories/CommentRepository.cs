using Dapper;
using TwitterServer.Models;
using TwitterServer.Utilities;

namespace TwitterServer.Repositories;


public interface ICommentRepository
{
    Task<Comment> Create(Comment Data);
    Task<List<Comment>> GetAllComments(long post_id);
    Task<Comment> GetCommentById(long id);
    Task<bool> Delete(long Id);
}
public class CommentRepository : BaseRepository, ICommentRepository
{
    public CommentRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<Comment> Create(Comment Data)
    {
        var query = $@"INSERT INTO ""{TableNames.comment}"" (comment_text, post_id, user_id)
        VALUES(@CommentText, @PostId, @UserId) RETURNING *";
        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Comment>(query, Data);
    }

    public async Task<bool> Delete(long Id)
    {
        var query = $@"DELETE FROM {TableNames.comment} WHERE id = @Id";

        using (var con = NewConnection)
            return (await con.ExecuteAsync(query, new { Id })) > 0;

    }

    public async Task<List<Comment>> GetAllComments(long post_id)
    {
        var query = $@"SELECT * FROM ""{TableNames.comment}"" WHERE post_id = @PostId ";

        using (var con = NewConnection)
            return (await con.QueryAsync<Comment>(query, new { PostId = post_id })).AsList();
    }

    public async Task<Comment> GetCommentById(long id)
    {
        var query = $@"SELECT * FROM {TableNames.comment} WHERE id = @Id";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Comment>(query, new { Id = id });
    }
}

