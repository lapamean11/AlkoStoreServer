﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AlkoStoreServer.Base;
using AlkoStoreServer.CustomAttributes;

namespace AlkoStoreServer.Models
{
    public class Product : Model
    {
        [NoRender]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string ImgUrl { get; set; }

        [NoRender]
        public int IsPopular { get; set; }

        [NotMapped]
        public static readonly int IS_POPULAR = 1;

        /*[Reference(typeof(Category))]
        public List<Category> Categories { get; set; } = new List<Category>();*/

        [Reference(typeof(Category))]
        public List<ProductCategory> Categories { get; set; }

        [Reference(typeof(Store))]
        public List<ProductStore> ProductStore { get; set; }

        public List<Review> Reviews { get; set; }

        public List<ProductAttributeProduct> ProductAttributes { get; set; } = new List<ProductAttributeProduct>();
    }
}
