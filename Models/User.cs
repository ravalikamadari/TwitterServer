
using TwitterServer.DTOs;

namespace TwitterServer.Models
{
    public record User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        //    public UserDTO asDto => new UserDTO
        //     {
        //        Id = Id,
        //        Name = Name,
        //        Email = Email,

        // };

    }


}