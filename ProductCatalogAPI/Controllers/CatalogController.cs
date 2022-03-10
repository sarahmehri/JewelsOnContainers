using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductCatalogAPI.Data;
using ProductCatalogAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext _Context;
        private readonly IConfiguration _config;
        public CatalogController(CatalogContext context, IConfiguration config)
        {
            _Context = context;
            _config = config;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> CatalogBrand()
        {
            var brands = await _Context.CatalogBrands.ToListAsync();
            return Ok(brands);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> CatalogTypes()
        {
            var types = await _Context.CatalogTypes.ToListAsync();
            return Ok(types);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> Items([FromQuery]int pageIndex = 0, [FromQuery]int pageSize = 6)
        {
           var items = await _Context.Catalog
                .OrderBy(c => c.Name)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
           items =  ChangePicutureUrl(items);
            return Ok(items);
        }

        private List<CatalogItem> ChangePicutureUrl(List<CatalogItem> items)
        {
            items.ForEach(item => item.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced",
                _config["Externalbaseurl"]));
            return items;
        }
    }
}
