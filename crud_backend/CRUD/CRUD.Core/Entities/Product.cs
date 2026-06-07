using System;

namespace CRUD.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Size { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime MFG { get; set; }
        public DateTime ExpireDate { get; set; }
        public int CategoryId { get; set; }
    }
}
