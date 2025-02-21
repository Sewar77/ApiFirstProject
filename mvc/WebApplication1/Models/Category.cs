using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models;

namespace WebApplication1.Models;

public partial class Category
{
    public decimal Id { get; set; }

    public string? CategoryName { get; set; }

    public string? ImagePath { get; set; }

    //2.Add an Image file Property in the category class to add a new category. ​
    [NotMapped]
    public virtual IFormFile ImageFile { get; set; }
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
