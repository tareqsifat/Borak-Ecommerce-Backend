using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buraq.Ecommerce.Enums
{
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public enum ShipmentStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    public enum OrderStatus
    {
        Draft = 0,
        PendingPayment = 1,
        Processing = 2,
        PartiallyShipped = 3,
        Shipped = 4,
        Delivered = 5,
        Completed = 6,
        Cancelled = 7,
        ReturnRequested = 8,
        ReturnInProgress = 9,
        Returned = 10,
        Refunded = 11,
        OnHold = 12,
        Failed = 13
    }
}
