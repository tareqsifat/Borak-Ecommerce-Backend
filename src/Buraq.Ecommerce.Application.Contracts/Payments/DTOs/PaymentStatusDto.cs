using Buraq.Ecommerce.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buraq.Ecommerce.Payments.DTOs
{
    public class PaymentStatusDto
    {
        public int PaymentId { get; set; }
        public PaymentStatus Status { get; set; }
        public decimal AmountPaid { get; set; }
        public bool CanRefund { get; set; }
    }
}
