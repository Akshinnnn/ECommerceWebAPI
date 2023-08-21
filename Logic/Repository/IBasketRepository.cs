using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Repository
{
    public interface IBasketRepository
    {
        void ClearBasket(Expression<Func<Basket, bool>> expression);
    }
}
