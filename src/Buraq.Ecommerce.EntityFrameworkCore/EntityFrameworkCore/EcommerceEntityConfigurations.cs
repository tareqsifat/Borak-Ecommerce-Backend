using Buraq.Ecommerce.Addresses;
using Buraq.Ecommerce.CartItems;
using Buraq.Ecommerce.Carts;
using Buraq.Ecommerce.Categories;
using Buraq.Ecommerce.Customers;
using Buraq.Ecommerce.OrderItems;
using Buraq.Ecommerce.Orders;
using Buraq.Ecommerce.Payments;
using Buraq.Ecommerce.Products;
using Buraq.Ecommerce.Shipments;
using Buraq.Ecommerce.Suppliers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Buraq.Ecommerce.EntityFrameworkCore
{
    public static class EcommerceEntityConfigurations
    {
        public static void ConfigureEntities(this ModelBuilder builder)
        {
            builder.Entity<Address>(b =>
            {
                b.ToTable(EcommerceConsts.DbTablePrefix + "Addresses", EcommerceConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(p => p.Id)
                   .ValueGeneratedOnAdd()
                   .UseIdentityColumn();
            });

            builder.Entity<Cart>(b =>
            {
                b.ToTable(EcommerceConsts.DbTablePrefix + "Carts", EcommerceConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(p => p.Id)
                   .ValueGeneratedOnAdd()
                   .UseIdentityColumn();
            });

            builder.Entity<CartItem>(b =>
            {
                b.ToTable(EcommerceConsts.DbTablePrefix + "CartItems", EcommerceConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(p => p.Id)
                   .ValueGeneratedOnAdd()
                   .UseIdentityColumn();
            });

            builder.Entity<Category>(b =>
            {
                b.ToTable(EcommerceConsts.DbTablePrefix + "Categories", EcommerceConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(p => p.Id)
                   .ValueGeneratedOnAdd()
                   .UseIdentityColumn();
            });

            builder.Entity<Customer>(b =>
            {
                b.ToTable(EcommerceConsts.DbTablePrefix + "Customers", EcommerceConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(p => p.Id)
                   .ValueGeneratedOnAdd()
                   .UseIdentityColumn();
            });

            builder.Entity<Order>(b =>
            {
                b.ToTable(EcommerceConsts.DbTablePrefix + "Orders", EcommerceConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(p => p.Id)
                   .ValueGeneratedOnAdd()
                   .UseIdentityColumn();
            });

            builder.Entity<OrderItem>(b =>
            {
                b.ToTable(EcommerceConsts.DbTablePrefix + "OrderItems", EcommerceConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(p => p.Id)
                   .ValueGeneratedOnAdd()
                   .UseIdentityColumn();
            });

            builder.Entity<Payment>(b =>
            {
                b.ToTable(EcommerceConsts.DbTablePrefix + "Payments", EcommerceConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(p => p.Id)
                   .ValueGeneratedOnAdd()
                   .UseIdentityColumn();
            });

            builder.Entity<Product>(b =>
            {
                b.ToTable(EcommerceConsts.DbTablePrefix + "Products", EcommerceConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(p => p.Id)
                   .ValueGeneratedOnAdd()
                   .UseIdentityColumn();
            });

            builder.Entity<Shipment>(b =>
            {
                b.ToTable(EcommerceConsts.DbTablePrefix + "Shipments", EcommerceConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(p => p.Id)
                   .ValueGeneratedOnAdd()
                   .UseIdentityColumn();
            });

            builder.Entity<Supplier>(b =>
            {
                b.ToTable(EcommerceConsts.DbTablePrefix + "Suppliers", EcommerceConsts.DbSchema);
                b.ConfigureByConvention();

                b.Property(p => p.Id)
                   .ValueGeneratedOnAdd()
                   .UseIdentityColumn();
            });
        }
    }
}
