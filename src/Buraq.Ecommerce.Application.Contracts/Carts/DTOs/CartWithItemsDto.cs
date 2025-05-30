﻿using Buraq.Ecommerce.CartItems.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buraq.Ecommerce.Carts.DTOs
{
    public class CartWithItemsDto : CartDto
    {
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
    }
}
