using Data.Entities.PropertyInterfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class User : IdentityUser, IDateProperties
    {
        public string? RefreshToken { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<Token> Tokens { get; set; }
        public ICollection<Basket> Baskets { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
