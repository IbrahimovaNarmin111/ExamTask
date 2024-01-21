using ExamTask.DAL;
using ExamTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _db;
        public SettingController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Setting> settings=await _db.Settings.ToListAsync();
            return View(settings);
        }
        public IActionResult Create() 
        { 
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Setting setting)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            bool result=await _db.Settings.AnyAsync(x=>x.Key == setting.Key);
            if(result==true)
            {
                return View();
            }
            await _db.Settings.AddAsync(setting);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult>Update(int id)
        {
            var exist=await _db.Settings.FirstOrDefaultAsync(x=>x.Id==id);
            if(exist==null) return NotFound();
            return View(exist);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Setting setting, int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var exist = await _db.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if (exist == null) return NotFound();
            bool result = await _db.Settings.AnyAsync(x => x.Key == setting.Key && x.Id != id);
            if(result)
            {
                return View();
            }
           exist.Key = setting.Key;
            exist.Value = setting.Value;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var exist = await _db.Settings.FirstOrDefaultAsync(x => x.Id == id);
            if(exist==null) return NotFound();
            _db.Settings.Remove(exist);
            await _db.SaveChangesAsync();   
            return RedirectToAction(nameof(Index));
        }

    }
}
