using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Exceptions
{
    public sealed class CompanyCollectionBadRequest : BadRequestException
    {
        public CompanyCollectionBadRequest()
        : base("Company CollectionForCreationDto is null.")
        {
        }
    }
}
