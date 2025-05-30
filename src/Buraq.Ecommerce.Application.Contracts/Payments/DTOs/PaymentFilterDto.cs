using Buraq.Ecommerce.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Buraq.Ecommerce.Payments.DTOs
{
    public class PaymentFilterDto : PagedAndSortedResultRequestDto
    {
        public int? OrderId { get; set; }
        public PaymentStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
