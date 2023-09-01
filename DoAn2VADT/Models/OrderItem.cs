using DoAn2VADT.Database.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace DoAn2VADT.Models
{
    public class OrderItem
    {
        public int quantity { set; get; }
        public Book book { set; get; }
    }
}