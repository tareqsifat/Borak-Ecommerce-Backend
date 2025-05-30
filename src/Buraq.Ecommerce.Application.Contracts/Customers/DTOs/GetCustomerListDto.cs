using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Buraq.Ecommerce.Customers.DTOs
{
    public class GetCustomerListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
