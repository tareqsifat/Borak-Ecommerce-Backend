﻿using Buraq.Ecommerce.Enums;
using Buraq.Ecommerce.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Buraq.Ecommerce.Payments
{
    public class Payment : FullAuditedAggregateRoot<int>
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }

        public Payment(
            int id,
            int orderId,
            DateTime paymentDate,
            decimal amount,
            string paymentMethod,
            PaymentStatus Status
        ) : base(id)
        {
            OrderId = orderId;
            PaymentDate = paymentDate;
            Amount = amount;
            PaymentMethod = paymentMethod;
            Status = Status;
        }
    }

}
