﻿namespace TASK.DTO
{
    public class CategoryProductDTO
    {
        public int Id { get; set; }
        public string? ArabicName { get; set; }
        public string? EnglishName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string? State { get; set; }
        public int CreationUserId { get; set; }
        public DateTime CreationDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }


      // public List<CategoryProductDTO> Products { get; set; }
        public List<string> ProductNames { get; set; }
    }
}
