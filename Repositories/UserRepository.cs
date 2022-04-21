using Dapper;
using TwitterServer.Models;
using TwitterServer.Utilities;

namespace TwitterServer.Repositories;


public interface IUserRepository
{
    Task<User> Create(User Data);
    Task<User> GetByEmail(string Email);
    Task<bool> Update(User toUpdateUser);
}
public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<User> Create(User Data)
    {
        var query = $@"INSERT INTO ""{TableNames.users}"" (name, email, password)
        VALUES(@Name, @Email, @Password) RETURNING *;";
        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<User>(query, Data);
    }

    public async Task<User> GetByEmail(string Email)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}"" WHERE email = @Email;";
        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<User>(query, new { Email });
    }

    public async Task<bool> Update(User toUpdateUser)
    {
        var query = $@"UPDATE ""{TableNames.users}"" SET name = @Name WHERE id = @Id;";
        using (var con = NewConnection)
            return (await con.ExecuteAsync(query, toUpdateUser)) == 1;
    }
}