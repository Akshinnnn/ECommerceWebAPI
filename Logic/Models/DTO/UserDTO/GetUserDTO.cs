﻿using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models.DTO.UserDTO
{
    public class GetUserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<Basket> Baskets { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
