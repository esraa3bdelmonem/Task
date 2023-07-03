using System.ComponentModel.DataAnnotations.Schema;
using TASK.Models;
using ZendeskApi_v2.Models.HelpCenter.Topics;

namespace TASK.DTO
{
    public class ProductCategoryDTO
    {
        public int Id { get; set; }
        public string? ArabicName { get; set; }
        public string? EnglishName { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string? Manufacturer { get; set; }
     
        public string? State { get; set; }
        public int CreationUserId { get; set; }
        public DateTime CreationDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }


        public int CategoryId { get; set; }

        public  string CategoryName { get; set; }
    }
}
