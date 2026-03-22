using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkTrace.Data;
using WorkTrace.Models;

namespace WorkTrace.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AttendanceRecordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceRecordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? from, DateTime? to, Guid? employeeId)
        {
            var query = _context.AttendanceRecords
                .Include(r => r.Employee)
                .AsQueryable();

            if (from.HasValue)
                query = query.Where(r => r.Date >= from.Value);
            if (to.HasValue)
                query = query.Where(r => r.Date <= to.Value);
            if (employeeId.HasValue)
                query = query.Where(r => r.EmployeeId == employeeId.Value);

            var records = await query.ToListAsync();

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName");
            return View(records);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            var record = await _context.AttendanceRecords
                .Include(r => r.Employee)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (record == null) return NotFound();
            return View(record);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AttendanceRecord record)
        {
            if (ModelState.IsValid)
            {
                record.Id = Guid.NewGuid();
                _context.Add(record);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", record.EmployeeId);
            return View(record);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var record = await _context.AttendanceRecords.FindAsync(id);
            if (record == null) return NotFound();
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", record.EmployeeId);
            return View(record);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AttendanceRecord record)
        {
            if (id != record.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(record);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceRecordExists(record.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", record.EmployeeId);
            return View(record);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var record = await _context.AttendanceRecords
                .Include(r => r.Employee)
                .FirstOrDefaultAsync(r => r.Id == id);
            if (record == null) return NotFound();
            return View(record);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var record = await _context.AttendanceRecords.FindAsync(id);
            if (record != null) _context.AttendanceRecords.Remove(record);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceRecordExists(Guid id)
        {
            return _context.AttendanceRecords.Any(e => e.Id == id);
        }
    }
}