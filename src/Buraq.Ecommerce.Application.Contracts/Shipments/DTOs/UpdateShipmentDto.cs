﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buraq.Ecommerce.Shipments.DTOs
{
    public class UpdateShipmentDto
    {
        public string Carrier { get; set; }
        public string TrackingNumber { get; set; }
    }
}
