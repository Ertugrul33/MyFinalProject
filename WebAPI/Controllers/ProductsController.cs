using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController] //Attribute
    public class ProductsController : ControllerBase
    {
        //Naming Convention
        //IoC Container --> Inversion of Control: Referansları depolayan bir kutu gibi düşünülebilir. Yani bizim yerimize new'ler.
        IProductService _productService;

        public ProductsController(IProductService productService) //Loosely Coupled --> Gevşek Bağlılık: Soyuta bağlı olduğu için gevşek bağlılık deniyor.
        {
            _productService = productService;
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            //Swagger: Dökümantasyon
            //IProductService productService = new ProductManager(new EfProductDal()); --> Dependency Chain --> Bağımlılık Zinciri

            Thread.Sleep(millisecondsTimeout: 1000);

            var result = _productService.GetAll();
            if (result.Success)
            {
                //Status durumu önemlidir. Çünkü Data boş mu yoksa sistemle mi alakalı bu sayede anlaşılabilir.
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getbycategory")]
        public IActionResult GetByCategory(int categoryId)
        {
            var result = _productService.GetAllByCategoryId(categoryId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getproductdetails")]
        public IActionResult GetProductDetails(int categoryId)
        {
            var result = _productService.GetProductDetails();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("add")]
        public IActionResult Add(Product product)
        {
            var result = _productService.Add(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        //Güncelleme ve silme için de HttpPost kullanılabilir.
        //HttpPut ve HttpDelete de kullanılabilir fakat HttpPost tercih edilir.
    }
}
