﻿using AlkoStoreServer.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlkoStoreServer.Models
{
    public class CategoryAttributeCategory : Model
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int AttributeId { get; set; }

        public CategoryAttribute Attribute { get; set; }

        public string Value { get; set; }
    }
}
