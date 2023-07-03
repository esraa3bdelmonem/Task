using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TASK.DTO;
using TASK.Models;
using ZendeskApi_v2.Models.HelpCenter.Topics;

namespace TASK.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {

        private readonly TaskDbContext _context;

        public CategoryController(TaskDbContext context)
        {

            _context = context;
        }
  
        [HttpGet]
        public IActionResult GeAll()
        {
            List<Category> Categories = _context.Categories.Include(t => t.Products).ToList();
            if (Categories.Count == 0)
                return BadRequest("No Categories Found");

            List<CategoryProductDTO> CategoriesDTO = new List<CategoryProductDTO>();
            foreach (var c in Categories)
            {
                CategoryProductDTO catDTO = new CategoryProductDTO()
                {
                    Id = c.Id,
                    ArabicName = c.ArabicName,
                    EnglishName = c.EnglishName,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    State = c.State,
                    CreationUserId = c.CreationUserId,
                    CreationDate = c.CreationDate,
                    UpdateUserId = c.UpdateUserId,
                    UpdateDate = c.UpdateDate,


                };
                foreach (var p in c.Products)
                {
                      catDTO.ProductNames?.Add(p.EnglishName);
                }
                CategoriesDTO.Add(catDTO);
            }
            return Ok(CategoriesDTO);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            Category c = _context.Categories.Include(t => t.Products).FirstOrDefault(t => t.Id == id);
            if (c == null)
                return BadRequest("Id is invalid");
            CategoryProductDTO catDTO = new CategoryProductDTO()
            {
                Id = c.Id,
                ArabicName = c.ArabicName,
                EnglishName = c.EnglishName,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                State = c.State,
                CreationUserId = c.CreationUserId,
                CreationDate = c.CreationDate,
                UpdateUserId = c.UpdateUserId,
                UpdateDate = c.UpdateDate,
            };
            foreach (var p in c.Products)
            {

             catDTO.ProductNames?.Add(p.EnglishName);
            }
            return Ok(catDTO);
        }
       

        [HttpGet("{name:alpha}")]
        public IActionResult GetByName(string name)
        {
            Category c = _context.Categories.Include(t => t.Products).FirstOrDefault(t => t.EnglishName== name);
          
            if (c == null)
                return BadRequest("Name is invalid");

            CategoryProductDTO catDTO = new CategoryProductDTO()
            {
                Id = c.Id,
                ArabicName = c.ArabicName,
                EnglishName = c.EnglishName,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                State = c.State,
                CreationUserId = c.CreationUserId,
                CreationDate = c.CreationDate,
                UpdateUserId = c.UpdateUserId,
                UpdateDate = c.UpdateDate,
            };
            foreach (var p in c.Products)
            {

                catDTO.ProductNames?.Add(p.EnglishName);
            }
            return Ok(catDTO);
        }
        [HttpPost]
        public IActionResult Add(CategoryProductDTO c)
        {
            if (ModelState.IsValid)
            {
                Category existingCategory = _context.Categories
                    .Include(t => t.Products)
                    .FirstOrDefault(t => t.EnglishName == c.EnglishName);

            
                if (existingCategory != null)
                {
                    return Conflict(" Category already exists");
                }

                Category newCategory = new Category()
                {
                    EnglishName = c.EnglishName,
                    ArabicName = c.ArabicName,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    State = c.State,

                    Products = new List<Product>()
                };

                foreach (var productName in c.ProductNames)
                {
                    Product existingProduct = _context.Products
                        .FirstOrDefault(c => c.EnglishName == productName);

                    if (existingProduct == null)
                    {
                        Product newProduct = new Product()
                        {
                            EnglishName = productName,
                            ArabicName = "some value", // set the ArabicName property to a non-null value
                            Description = "some value", // set the Description property to a non-null value
                            Manufacturer = "some value",
                            State = "some value"// set the Manufacturer property to a non-null value
                        };

                        _context.Products.Add(newProduct);
                        newCategory.Products.Add(newProduct);
                    }
                    else if (!newCategory.Products.Any(c => c.Id == existingProduct.Id))
                    {
                        newCategory.Products.Add(existingProduct);
                    }
                }

                _context.Categories.Add(newCategory);
                _context.SaveChanges();

                GetById(newCategory.Id);

                return StatusCode(201, "Product Created Successfylly");
            }

            return BadRequest(ModelState);
        }


        [HttpPut("{id:int}")]
        public IActionResult Update(CategoryProductDTO c, int id)
        {
            if (ModelState.IsValid)
            {
                Category oldCategory = _context.Categories.Include(c => c.Products).FirstOrDefault(t => t.Id == id);
                if (oldCategory == null)
                    return BadRequest("Id is invalid");

                foreach (var cat in oldCategory.Products)
                {
                    _context.Products.Remove(cat);
                }

                //context.SaveChanges();

                oldCategory.EnglishName = c.EnglishName;
                oldCategory.ArabicName = c.ArabicName;

                oldCategory.Products.Clear();


                foreach (var cat in c.ProductNames)
                {
                    var existProduct = _context.Products.FirstOrDefault(c => c.EnglishName == cat);
                    if (existProduct == null)
                    {
                        Product newProduct = new Product()
                        {
                            EnglishName = cat,
                         
                        };
                       _context.Products.Add(newProduct);
                        oldCategory.Products.Add(newProduct);
                    }
                    else if (!oldCategory.Products.Any(c => c.Id == existProduct.Id))
                    {
                        oldCategory.Products.Add(existProduct);
                    }
                }
                _context.SaveChanges();

                return StatusCode(200, "product Updated Successfully");
            }

            return BadRequest(ModelState);
        }


        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            Category c = _context.Categories.Include(c => c.Products).FirstOrDefault(t => t.Id == id);
            if (c == null)
                return BadRequest("Id is invalid");
            try
            {
                foreach (var cat in c?.Products)
                {
                    _context.Products.Remove(cat);
                }
                _context.Categories.Remove(c);
                _context.SaveChanges();
                return StatusCode(204, "Category Deleted Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}