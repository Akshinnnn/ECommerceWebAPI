using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Models.DTO.OrderDTO
{
    public class RefundProductDTO
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}
