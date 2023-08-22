using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models.DTO.OrderDTO
{
    public class GetOrderDTO
    {
        public Guid OrderNumber { get; set; }
        public int ProductQuantity { get; set; }

        public DateTime CreatedDate { get; set; }
        public ICollection<GetOrderProductDTO> OrderProducts { get; set; }
    }
}
