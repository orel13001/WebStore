using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Orders;
using WebStore.Domain.ViewModels;

namespace WebStore.Domain.DTO
{
    public class CreateOrderDTO
    {
        public OrderViewModel Order { get; set; }

        public IEnumerable<OrderItemDTO> Items { get; set; }
    }


    public class OrderDTO
    {
        public int Id { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset Date { get; set; }

        public IEnumerable<OrderItemDTO> Items { get; set; }
    }


    public class OrderItemDTO
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }



    public static class OrderDTOMapper
    {
        public static OrderItemDTO? ToDTO(this OrderItem? item) => item is null
            ? null
            : new OrderItemDTO
            {
                Id = item.Id,
                Price = item.Price,
                ProductId = item.Product.Id,
                Quantity = item.Quantity,
            };

        public static OrderItem? FromDTO(this OrderItemDTO? item) => item is null
            ? null
            : new OrderItem
            {
                Id = item.Id,
                Price = item.Price,
                Product = new Product { Id = item.Id},
                Quantity = item.Quantity,
            };

        public static OrderDTO? ToDTO(this Order? order) => order is null
            ? null
            : new()
            {
                Id = order.Id,
                Address = order.Address,
                Date = order.Date,
                Description = order.Description,
                Items = order.Items.Select(ToDTO)!,
                Phone = order.Phone,
            };

        public static Order? FromDTO(this OrderDTO? order) => order is null
            ? null
            : new()
            {
                Id = order.Id,
                Phone = order.Phone,
                Address = order.Address,
                Date = order.Date,
                Description = order.Description,
                Items = order.Items.Select(FromDTO).ToList()!,
            };


        public static IEnumerable<OrderDTO?> ToDTO(this IEnumerable<Order?> orders) => orders.Select(ToDTO);
        public static IEnumerable<Order?> FromDTO(this IEnumerable<OrderDTO?> orders) => orders.Select(FromDTO);


        public static IEnumerable<OrderItemDTO> ToDTO (this CartViewModel cart) => 
            cart.Items.Select(p => new OrderItemDTO
            {
                ProductId = p.Product.Id,
                Price = p.Product.Price,
                Quantity = p.Quantity,
            });

        public static CartViewModel ToCartView(this IEnumerable<OrderItemDTO> items) => new()
        {
            Items = items.Select(p => (new ProductViewModel { Id = p.ProductId }, p.Quantity))
        };
    }
}
