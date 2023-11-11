using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.Data;
using WebStore.Data.Entitties;
using WebStore.Data.Entitties.Identity;
using WebStore.Models.Category;

namespace WebStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AppEFContext _context;
        private readonly UserManager<UserEntity> _userManager;
        public CategoriesController(IMapper mapper, AppEFContext context,
            UserManager<UserEntity> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("list")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Claims.First().Value;
            var user = await _userManager.FindByEmailAsync(userName);
            var model = await _context.Categories
               .Where(x => x.IsDeleted == false)
               .Where(x => x.UserId == user.Id)
               .Select(x => _mapper.Map<CategoryItemViewModel>(x))
               .ToListAsync();

            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            string userName = User.Claims.First().Value;
            var user = await _userManager.FindByEmailAsync(userName);
            var cat = await _context.Categories
               .Where(x => x.IsDeleted == false)
               .Where(x => x.UserId == user.Id)
               //.Select(x => _mapper.Map<CategoryItemViewModel>(x))
               .SingleOrDefaultAsync(x=>x.Id==id);

            if (cat == null)
                return NotFound();

            var model = _mapper.Map<CategoryItemViewModel>(cat);
            return Ok(model);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CategoryCreateViewModel model)
        {
            var requst = this.Request;
            try
            {
                string userName = User.Claims.First().Value;
                var user = await _userManager.FindByEmailAsync(userName);
                var cat = _mapper.Map<CategoryEntity>(model);
                cat.UserId = user.Id;
                await _context.Categories.AddAsync(cat);
                string imageName = String.Empty;
                if (model.Image != null)
                {
                    string exp = Path.GetExtension(model.Image.FileName);
                    imageName = Path.GetRandomFileName() + exp;
                    string dirSaveImage = Path.Combine(Directory.GetCurrentDirectory(), "images", imageName);
                    using (var stream = System.IO.File.Create(dirSaveImage))
                    {
                        await model.Image.CopyToAsync(stream);
                    }
                }
                cat.Image = imageName;
                await _context.SaveChangesAsync();
                return Ok(_mapper.Map<CategoryItemViewModel>(cat));
            } 
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Put([FromBody] CategoryUpdateViewModel dto)
        {
            string userName = User.Claims.First().Value;
            var user = await _userManager.FindByEmailAsync(userName);
            var cat = await _context.Categories
                .Where(x => x.IsDeleted == false)
                .Where(x => x.UserId == user.Id)
                .SingleOrDefaultAsync(x => x.Id == dto.Id);
            if (cat == null)
                return NotFound();
            cat.Name= dto.Name;
            cat.Description= dto.Description;
            cat.Image = dto.Image;
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<CategoryItemViewModel>(cat));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Dekete(int id)
        {
            string userName = User.Claims.First().Value;
            var user = await _userManager.FindByEmailAsync(userName);
            var cat = await _context.Categories
                .Where(x => x.IsDeleted == false)
                .Where(x => x.UserId == user.Id)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (cat == null)
                return NotFound();
            cat.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
