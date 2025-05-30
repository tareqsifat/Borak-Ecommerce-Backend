using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buraq.Ecommerce.Enums
{
    public enum PaymentStatus
    {
        Pending = 1,
        Completed = 2,
        Failed = 3,
        Refunded = 4,
        PartiallyRefunded = 5
    }

    public enum ShipmentStatus
    {
        Pending = 1,
        Processing = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5
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
