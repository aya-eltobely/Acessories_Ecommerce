﻿namespace AccessoriesWebsite.Models
{
    public class Category
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Product> products { get; set; }         
    }
}
