using NTTShopAdmin.Entities;
using NTTShopAdmin.Models.Entities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NTTShopAdmin.Models.ViewModel
{
    public class OrdersViewModel
    {
        public OrdersViewModel() { 
            order = new Order();
            orderDetail = new OrderDetail();
            orderStatus = new OrderStatus();
            user = new User();
        }
        public IPagedList<Order> orderPaged { get; set; }
        public IPagedList<OrderDetail> orderDetailPaged { get; set; }
        public List<Order> allOrders { get; set; } = new List<Order>();
        public List<OrderDetail> allOrdersDetail { get; set; } = new List<OrderDetail>();
        public List<OrderStatus> allOrderStatus { get; set; } = new List<OrderStatus>();
        public List<OrderStatus> searchOrderStatus { get; set; } = new List<OrderStatus> { };

        public Order order  { get; set; }
        public OrderDetail orderDetail { get; set; }
        public OrderStatus orderStatus{ get; set;}
        public User user { get; set; }
    }
       
}