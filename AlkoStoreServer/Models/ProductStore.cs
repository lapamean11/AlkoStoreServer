﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlkoStoreServer.Models
{
    public class ProductStore
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public int ProductId { get; set; }

        public int StoreId { get; set; }

        public decimal Price { get; set; }

        public string Barcode { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public Product Product { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public Store Store { get; set; }
    }
}