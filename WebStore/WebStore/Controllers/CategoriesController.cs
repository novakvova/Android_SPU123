using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.Models.Category;

namespace WebStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AppEFContext _context;
        public CategoriesController(IMapper mapper, AppEFContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("list")]
        public async Task<IActionResult> Index()
        {
            var model = await _context.Categories
               .Where(x => x.IsDeleted == false)
               .Select(x => _mapper.Map<CategoryItemViewModel>(x))
               .ToListAsync();

            return Ok(model);
        }
    }
}
