using ExamTask.DAL;
using ExamTask.Helpers;
using ExamTask.Models;
using ExamTask.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public TeamController(AppDbContext db,IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index(int page=1)
        {
            int take = 3;
            decimal count=await _db.Teams.CountAsync(); 
            List<Team> teams = await _db.Teams.Skip((page-1)*take).Take(take).ToListAsync();
            PaginateVM<Team> paginateVM = new PaginateVM<Team>
            {
                TotalPage=Math.Ceiling(count/take),
                CurrentPage=page,
                Take=take,
                Items=teams
            };
            return View(paginateVM);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Team team)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(team.ImageFile is null)
            {
                ModelState.AddModelError("ImageFile", "Image can not be null");
                return View();

            }
            if(!team.ImageFile.CheckFileType("image/"))
            {
                ModelState.AddModelError("ImageFile", "Image must be in image type");
                return View();
            }
            if(team.ImageFile.CheckFileLength(300))
            {
                ModelState.AddModelError("ImageFile", "Image must be more than" + 300 + "kb");
                return View();
            }
            team.Image = team.ImageFile.CreateFile(_env.WebRootPath, "uploads/team");
            await _db.Teams.AddAsync(team);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if(id==null) return NotFound();
            Team dbTeam=await _db.Teams.FirstOrDefaultAsync(t=>t.Id==id);
            if(dbTeam==null) return BadRequest();
            return View(dbTeam);
        }
        [HttpPost]
        public async Task<IActionResult>Update(Team team,int? id)
        {
            if (id == null) return NotFound();
            Team dbTeam = await _db.Teams.FirstOrDefaultAsync(t => t.Id == id);
            if (dbTeam == null) return NotFound();
            if(!ModelState.IsValid)
            {
                return View();
            }
            if(team.ImageFile is not null)
            {
                if (!team.ImageFile.CheckFileType("image/"))
                {
                    ModelState.AddModelError("ImageFile", "Image must be in image type");
                    return View();
                }
                if (team.ImageFile.CheckFileLength(300))
                {
                    ModelState.AddModelError("ImageFile", "Image must be more than" + 300 + "kb");
                    return View();
                }
                dbTeam.Image.DeleteFile(_env.WebRootPath, "uploads/team");
                dbTeam.Image = team.ImageFile.CreateFile(_env.WebRootPath, "uploads/team");
            }
            dbTeam.FullName = team.FullName;
            dbTeam.Description = team.Description;
            dbTeam.Work=team.Work;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
            

        }
        public async Task<IActionResult>Delete(int id)
        {
            Team team = await _db.Teams.FirstOrDefaultAsync(x=>x.Id == id);
            if(team == null) return NotFound();
            team.Image.DeleteFile(_env.WebRootPath, "uploads/team");
            _db.Teams.Remove(team);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
