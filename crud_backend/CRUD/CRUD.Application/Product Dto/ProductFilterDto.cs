using System;


namespace CRUD.Application.Product_Dto
{
    public class ProductFilterDto
    {
        public string? Name { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? CategoryId { get; set; }
    }
}