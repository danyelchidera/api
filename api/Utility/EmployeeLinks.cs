using Contracts;
using Entities.LinkModels;
using Entities.Models;
using Microsoft.Net.Http.Headers;
using Shared.DTOs;


namespace api.Utility
{
    public class EmployeeLinks: IEmployeeLinks
    {
        private readonly IDataShaper<EmployeeDto> _dataShaper;
        private readonly LinkGenerator _linkGenerator;

        public EmployeeLinks(LinkGenerator linkGenerator, IDataShaper<EmployeeDto> dataShaper)
        {
            _dataShaper = dataShaper;
            _linkGenerator = linkGenerator;
        }

        public LinkResponse TryGenerateLinks(IEnumerable<EmployeeDto> employeesDto,
        string fields, Guid companyId, HttpContext httpContext)
        {
            var shapedEmployees = ShapeData(employeesDto, fields);

            if (shouldGenerateLinks(httpContext))
                return ReturnLinkedEmployees(employeesDto, fields, companyId, httpContext, shapedEmployees);

            return ReturnShapedEmployees(shapedEmployees);
        }

        private LinkResponse ReturnShapedEmployees(List<Entity> shapedEmployees) =>
            new LinkResponse { ShapedEntities = shapedEmployees };

        private bool shouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }

        private LinkResponse ReturnLinkedEmployees(IEnumerable<EmployeeDto> employeesDto, 
            string fields, Guid companyId, HttpContext httpContext, List<Entity> shapedEmployees)
        {
            var employeesDtoList = employeesDto.ToList();

            for(int index = 0; index < employeesDtoList.Count; index++)
            {
                var employeeLinks = CreateLinksForEmployee(httpContext, 
                    companyId, employeesDtoList[index].Id, fields);

                shapedEmployees[index].Add("Links", employeeLinks);
            }

            var employeeCollection = new LinkCollectionWrapper<Entity>(shapedEmployees);
            var linkeEmployees = CreateLinksForEmployees(httpContext, employeeCollection);

            return new LinkResponse{ HasLinks = true, LinkedEntities = linkeEmployees };
        }

        private LinkCollectionWrapper<Entity> CreateLinksForEmployees(HttpContext httpContext, LinkCollectionWrapper<Entity> employeeWrapper)
        {
            employeeWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetEmployeesForCompany", values: new { }),
                "self", "GET"));

            return employeeWrapper;
        }

        private List<Link> CreateLinksForEmployee(HttpContext httpContext, Guid companyId, Guid id, string fields)
        {
            var links = new List<Link>
            {
                new Link(_linkGenerator.GetUriByAction(httpContext, "GetEmployeeForCompany",
                values: new{ companyId, id, fields }), "self", "GET"),
                
                new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteEmployeeForCompany",
                 values: new{ companyId, id}),
                 "delete_employee", "DELETE"),

                new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateEmployee",
                 values: new{ companyId, id}), "update_employee", "UPDATE"),

                new Link(_linkGenerator.GetUriByAction(httpContext,"PartiallyUpdateEmployeeForCompany", 
                values: new { companyId, id }), "partially_update_employee", "PATCH")
            };

            return links;
        }

        private List<Entity> ShapeData(IEnumerable<EmployeeDto> employeesDto, string fields) =>
            _dataShaper.ShapeData(employeesDto, fields)
            .Select(e => e.Entity)
            .ToList();
    }
}
