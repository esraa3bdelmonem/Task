using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TASK.DTO;
using TASK.Models;
using ZendeskApi_v2.Models.HelpCenter.Topics;

namespace TASK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly TaskDbContext _context;

        public ProductController(TaskDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> p = _context.Products.Include(c => c.Category).ToList();
            if (p.Count == 0)
                return BadRequest("There are no Products");

            List<ProductCategoryDTO> proDTO = new List<ProductCategoryDTO>();
            foreach (Product pro in p)
            {
                proDTO.Add(new ProductCategoryDTO()
                {
                    Id = pro.Id,
                    EnglishName = pro.EnglishName,
                    ArabicName = pro.ArabicName,
                    Price = pro.Price,
                    Description = pro.Description,
                    Manufacturer = pro.Manufacturer,
                    State = pro.State,
                    CreationUserId = pro.CreationUserId,
                    CreationDate = pro.CreationDate,
                    UpdateUserId = pro.UpdateUserId,
                    UpdateDate = pro.UpdateDate,
                    CategoryName = pro.Category.EnglishName
                });
            }

            return Ok(proDTO);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            Product pro = _context.Products.Include(c => c.Category).FirstOrDefault(c => c.Id == id);
            if (pro == null)
                return BadRequest("Id is invalid");
            ProductCategoryDTO proDTO = new ProductCategoryDTO()
            {
                Id = pro.Id,
                EnglishName = pro.EnglishName,
                ArabicName = pro.ArabicName,
                Price = pro.Price,
                Description = pro.Description,
                Manufacturer = pro.Manufacturer,
                State = pro.State,
                CreationUserId = pro.CreationUserId,
                CreationDate = pro.CreationDate,
                UpdateUserId = pro.UpdateUserId,
                UpdateDate = pro.UpdateDate,
                CategoryName = pro.Category.EnglishName
            };

            return Ok(proDTO);
        }
        [HttpGet("{name:alpha}")]
        public IActionResult GetByName(string name)
        {
            Product pro = _context.Products.Include(c => c.Category).FirstOrDefault(c => c.EnglishName == name);
            if (pro == null)
                return BadRequest("Id is invalid");
            ProductCategoryDTO crsDTO = new ProductCategoryDTO()
            {
                Id = pro.Id,
                EnglishName = pro.EnglishName,
                ArabicName = pro.ArabicName,
                Price = pro.Price,
                Description = pro.Description,
                Manufacturer = pro.Manufacturer,
                State = pro.State,
                CreationUserId = pro.CreationUserId,
                CreationDate = pro.CreationDate,
                UpdateUserId = pro.UpdateUserId,
                UpdateDate = pro.UpdateDate,
                CategoryName = pro.Category.EnglishName
            };

            return Ok(crsDTO);
        }



        [HttpPost]
        public IActionResult Add(ProductCategoryDTO pro)
        {
            if (ModelState.IsValid)
            {
                Product ProductExist = _context.Products.Include(c => c.Category)
                                                    .FirstOrDefault(c => c.EnglishName == pro.EnglishName);
                if (ProductExist != null)
                    return Conflict("Product is already exist");

                Category Category = _context.Categories.FirstOrDefault(t => t.EnglishName == pro.EnglishName);

                if (Category == null)
                    return BadRequest("Category is not exist");

                Product newProduct = new Product()
                {
                    Id = pro.Id,
                    EnglishName = pro.EnglishName,
                    ArabicName = pro.ArabicName,
                    Price = pro.Price,
                    Description = pro.Description,
                    Manufacturer = pro.Manufacturer,
                    State = pro.State,
                    CreationUserId = pro.CreationUserId,
                    CreationDate = pro.CreationDate,
                    UpdateUserId = pro.UpdateUserId,
                    UpdateDate = pro.UpdateDate,
                    Category = Category
                };

                _context.Products.Add(newProduct);
                _context.SaveChanges();
                ProductCategoryDTO catDTO = new ProductCategoryDTO()
                {
                    Id = newProduct.Id,
                    EnglishName = newProduct.EnglishName,
                    ArabicName = newProduct.ArabicName,
                    Price = newProduct.Price,
                    Description = newProduct.Description,
                    Manufacturer = newProduct.Manufacturer,
                    State = newProduct.State,
                    CreationUserId = newProduct.CreationUserId,
                    CreationDate = newProduct.CreationDate,
                    UpdateUserId = newProduct.UpdateUserId,
                    UpdateDate = newProduct.UpdateDate,

                    CategoryName = newProduct.Category.EnglishName
                };

                return Created("Product Created Successfully", catDTO);

            }
            return BadRequest(ModelState);
        }
        //        [HttpPost]
        //        public IActionResult AddProduct(ProductCategoryDTO productCategoryDTO)
        //        {
        //            var category = _context.Categories.Include(x => x.Products).FirstOrDefault(x => x.Id == productCategoryDTO.CategoryId);

        //            if (category == null)
        //            {
        //                return NotFound("Category Not Found");
        //            }

        //            var product = new Product
        //            {
        //                ArabicName = productCategoryDTO.ArabicName,
        //                EnglishName = productCategoryDTO.EnglishName,
        //                Price = productCategoryDTO.Price,
        //                Description = productCategoryDTO.Description,
        //                Manufacturer = productCategoryDTO.Manufacturer,
        //                State = productCategoryDTO.State,
        //                CategoryId = productCategoryDTO.CategoryId
        //            };

        //            _context.Products.Add(product);
        //            _context.SaveChanges();

        //            return Ok("Product Added Successfully");
        //        }

        //        [HttpPut]
        //        public IActionResult UpdateProduct(ProductCategoryDTO productCategoryDTO)
        //        {
        //            var category = _context.Categories.Include(x => x.Products).FirstOrDefault(x => x.Id == productCategoryDTO.CategoryId);

        //            if (category == null)
        //            {
        //                return NotFound("Category Not Found");
        //            }

        //            var product = _context.Products.FirstOrDefault(x => x.Id == productCategoryDTO.Id);

        //            if (product == null)
        //            {
        //                return NotFound("Product Not Found");
        //            }

        //            product.ArabicName = productCategoryDTO.ArabicName;
        //            product.EnglishName = productCategoryDTO.EnglishName;
        //            product.Price = productCategoryDTO.Price;
        //            product.Description = productCategoryDTO.Description;
        //            product.Manufacturer = productCategoryDTO.Manufacturer;
        //            product.State = productCategoryDTO.State;
        //            product.CategoryId = productCategoryDTO.CategoryId;

        //            _context.SaveChanges();

        //            return Ok("Product Updated Successfully ");
        //        }

        //        [HttpDelete("{id}")]
        //        public IActionResult DeleteProduct(int id)
        //        {
        //            var product = _context.Products.FirstOrDefault(x => x.Id == id);

        //            if (product == null)
        //            {
        //                return NotFound("Product Not Found ");
        //            }

        //            _context.Products.Remove(product);
        //            _context.SaveChanges();

        //            return Ok("Product Deleted Successfully ");
        //        }

        //        [HttpGet("search")]
        //        public IActionResult SearchProducts(int categoryId = 0, string name = null)
        //        {
        //            IQueryable<Product> query = _context.Products;

        //            if (categoryId != 0)
        //            {
        //                query = query.Where(p => p.CategoryId == categoryId);
        //            }

        //            if (!string.IsNullOrEmpty(name))
        //            {
        //                query = query.Where(p => p.EnglishName.Contains(name) || p.ArabicName.Contains(name));
        //            }

        //            var products = query.ToList();

        //            return Ok(products);
        //        }

    }

}
