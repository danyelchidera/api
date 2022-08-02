using Entities.Models;
using Repository.Extensions.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions
{
    public static class RepositoryEmployeeExtensions
    {
        public static IQueryable<Employee> FilterEmployee(this IQueryable<Employee> employee, 
            uint minAge, uint maxAge) => 
            employee.Where(x => x.Age >= minAge && x.Age <= maxAge);  

        public static IQueryable<Employee> Search(this IQueryable<Employee> employee, string searchTerm)
        {
            if(string.IsNullOrEmpty(searchTerm))
                return employee;

            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return employee.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Employee> Sort(this IQueryable<Employee> employee, string orderByQueryString)
        {
            if(string.IsNullOrEmpty(orderByQueryString))
                return employee.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);

            if(string.IsNullOrEmpty(orderQuery))
                return employee.OrderBy(e => e.Name);

            return employee.OrderBy(orderQuery);
        }
    }
}
