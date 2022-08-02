using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions.Utility
{
    public static class OrderQueryBuilder
    {
         public static string CreateOrderQuery<T>(string orderByQueryString)
        {
            var orderParam = orderByQueryString.Split(',');

            var propertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParam)
            {
                if (string.IsNullOrEmpty(param))
                    continue;

                var propertyFromQueryName = param.Split(' ')[0];
                var objectProperty = propertyInfo.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName,
                    StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var direction = param.EndsWith("desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {direction}, ");

            }
            return orderQueryBuilder.ToString().TrimEnd(',', ' ');
        }
    }

}
