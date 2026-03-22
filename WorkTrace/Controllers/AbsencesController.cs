using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkTrace.Data;
using WorkTrace.Models;

namespace WorkTrace.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AbsencesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AbsencesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, Guid? employeeId, AbsenceStatus? status)
        {
            var query = _context.Absences
                .Include(a => a.Employee)
                .Include(a => a.AbsenceType)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(a => a.StartDate >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(a => a.EndDate <= endDate.Value);
            if (employeeId.HasValue)
                query = query.Where(a => a.EmployeeId == employeeId.Value);
            if (status.HasValue)
                query = query.Where(a => a.Status == status.Value);

            var absences = await query.ToListAsync();

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName");
            ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(AbsenceStatus)).Cast<AbsenceStatus>());

            return View(absences);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            var absence = await _context.Absences
                .Include(a => a.Employee)
                .Include(a => a.AbsenceType)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (absence == null) return NotFound();
            return View(absence);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName");
            ViewBag.AbsenceTypes = new SelectList(await _context.AbsenceTypes.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Absence absence)
        {
            if (ModelState.IsValid)
            {
                absence.Id = Guid.NewGuid();
                absence.RequestedDate = DateTime.Now;
                absence.Status = AbsenceStatus.Pending;
                _context.Add(absence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", absence.EmployeeId);
            ViewBag.AbsenceTypes = new SelectList(await _context.AbsenceTypes.ToListAsync(), "Id", "Name", absence.AbsenceTypeId);
            return View(absence);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var absence = await _context.Absences.FindAsync(id);
            if (absence == null) return NotFound();
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", absence.EmployeeId);
            ViewBag.AbsenceTypes = new SelectList(await _context.AbsenceTypes.ToListAsync(), "Id", "Name", absence.AbsenceTypeId);
            return View(absence);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Absence absence)
        {
            if (id != absence.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(absence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AbsenceExists(absence.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName", absence.EmployeeId);
            ViewBag.AbsenceTypes = new SelectList(await _context.AbsenceTypes.ToListAsync(), "Id", "Name", absence.AbsenceTypeId);
            return View(absence);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var absence = await _context.Absences
                .Include(a => a.Employee)
                .Include(a => a.AbsenceType)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (absence == null) return NotFound();
            return View(absence);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var absence = await _context.Absences.FindAsync(id);
            if (absence != null) _context.Absences.Remove(absence);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AbsenceExists(Guid id)
        {
            return _context.Absences.Any(e => e.Id == id);
        }
    }
}