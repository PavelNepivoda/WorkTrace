using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WorkTrace.Data;
using WorkTrace.Models;

namespace WorkTrace.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SystemSettingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SystemSettingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Edit()
        {
            var setting = await _context.SystemSettings.FindAsync("WorkingDayHours");
            if (setting == null)
            {
                setting = new SystemSetting { Key = "WorkingDayHours", Value = "8" };
            }
            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SystemSetting setting)
        {
            if (setting.Key != "WorkingDayHours")
                return NotFound();

            if (ModelState.IsValid)
            {
                var existing = await _context.SystemSettings.FindAsync("WorkingDayHours");
                if (existing != null)
                {
                    existing.Value = setting.Value;
                    _context.SystemSettings.Update(existing);
                }
                else
                {
                    _context.SystemSettings.Add(setting);
                }
                await _context.SaveChangesAsync();
                TempData["Success"] = "Nastavení bylo uloženo.";
                return RedirectToAction(nameof(Edit));
            }
            return View(setting);
        }
    }
}