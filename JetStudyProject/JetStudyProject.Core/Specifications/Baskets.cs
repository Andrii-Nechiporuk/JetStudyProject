using Ardalis.Specification;
using JetStudyProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Core.Specifications
{
    public static class Baskets
    {
        public class ByBasketIdWithBasketItems : Specification<Basket>
        {
            public ByBasketIdWithBasketItems(int? basketId)
            {
                Query
                    .Where(x => x.Id == basketId)
                    .Include(x => x.BasketItems)
                    .ThenInclude(x => x.Event);
            }
        }
        public class ByUserId : Specification<Basket>
        {
            public ByUserId(string? userId)
            {
                Query
                    .Where(x => x.UserId == userId)
                    .Include(x => x.BasketItems)
                    .ThenInclude(x => x.Event);
            }
        }
    }
}
