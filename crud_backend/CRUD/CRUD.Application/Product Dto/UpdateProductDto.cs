using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Application.Product_Dto
{
    public class UpdateProductDto
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
