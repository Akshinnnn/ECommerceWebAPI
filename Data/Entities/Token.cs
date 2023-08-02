using Microsoft.AspNetCore.Identity;

namespace Data.Entities
{
    public class Token : BaseEntity
    {
        public string JWTToken { get; set; }
        public DateTime CreatedDate { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
