using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkTrace.Data;
using WorkTrace.Models;

namespace WorkTrace.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.AbsenceTypes = new SelectList(await _context.AbsenceTypes.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StartWork(string pin)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.PINCode == pin);
            if (employee == null)
            {
                TempData["Error"] = "Neplatný PIN kód.";
                return RedirectToAction(nameof(Index));
            }

            var today = DateTime.Today;
            var record = await _context.AttendanceRecords
                .FirstOrDefaultAsync(ar => ar.EmployeeId == employee.Id && ar.Date == today);

            if (record != null && record.StartTime != null)
            {
                TempData["Error"] = "Pracovní den již byl zahájen.";
            }
            else if (record != null && record.StartTime == null)
            {
                record.StartTime = DateTime.Now;
                _context.Update(record);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Pracovní den zahájen v {record.StartTime:HH:mm}.";
            }
            else
            {
                record = new AttendanceRecord
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = employee.Id,
                    Date = today,
                    StartTime = DateTime.Now
                };
                _context.AttendanceRecords.Add(record);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Pracovní den zahájen v {record.StartTime:HH:mm}.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> StartBreak(string pin)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.PINCode == pin);
            if (employee == null)
            {
                TempData["Error"] = "Neplatný PIN kód.";
                return RedirectToAction(nameof(Index));
            }

            var today = DateTime.Today;
            var record = await _context.AttendanceRecords
                .FirstOrDefaultAsync(ar => ar.EmployeeId == employee.Id && ar.Date == today);

            if (record == null || record.StartTime == null)
            {
                TempData["Error"] = "Nejprve zahajte pracovní den.";
            }
            else if (record.BreakStart != null && record.BreakEnd == null)
            {
                TempData["Error"] = "Již máte zapoèatou pøestávku.";
            }
            else
            {
                record.BreakStart = DateTime.Now;
                record.BreakEnd = null;
                _context.Update(record);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Pøestávka zaèala v {record.BreakStart:HH:mm}.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EndBreak(string pin)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.PINCode == pin);
            if (employee == null)
            {
                TempData["Error"] = "Neplatný PIN kód.";
                return RedirectToAction(nameof(Index));
            }

            var today = DateTime.Today;
            var record = await _context.AttendanceRecords
                .FirstOrDefaultAsync(ar => ar.EmployeeId == employee.Id && ar.Date == today);

            if (record == null || record.StartTime == null)
            {
                TempData["Error"] = "Nejprve zahajte pracovní den.";
            }
            else if (record.BreakStart == null || record.BreakEnd != null)
            {
                TempData["Error"] = "Pøestávka není aktivní.";
            }
            else
            {
                record.BreakEnd = DateTime.Now;
                _context.Update(record);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Pøestávka ukonèena v {record.BreakEnd:HH:mm}.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EndWork(string pin)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.PINCode == pin);
            if (employee == null)
            {
                TempData["Error"] = "Neplatný PIN kód.";
                return RedirectToAction(nameof(Index));
            }

            var today = DateTime.Today;
            var record = await _context.AttendanceRecords
                .FirstOrDefaultAsync(ar => ar.EmployeeId == employee.Id && ar.Date == today);

            if (record == null || record.StartTime == null)
            {
                TempData["Error"] = "Nejprve zahajte pracovní den.";
            }
            else if (record.EndTime != null)
            {
                TempData["Error"] = "Pracovní den již byl ukonèen.";
            }
            else
            {
                record.EndTime = DateTime.Now;
                if (record.BreakStart != null && record.BreakEnd == null)
                {
                    record.BreakEnd = DateTime.Now;
                }
                _context.Update(record);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Pracovní den ukonèen v {record.EndTime:HH:mm}.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RequestAbsence(string pin, Guid absenceTypeId, DateTime startDate, DateTime endDate, string? reason)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.PINCode == pin);
            if (employee == null)
            {
                TempData["Error"] = "Neplatný PIN kód.";
                return RedirectToAction(nameof(Index));
            }

            if (startDate > endDate)
            {
                TempData["Error"] = "Datum zaèátku musí být døíve než datum konce.";
                return RedirectToAction(nameof(Index));
            }

            var absence = new Absence
            {
                Id = Guid.NewGuid(),
                EmployeeId = employee.Id,
                AbsenceTypeId = absenceTypeId,
                StartDate = startDate,
                EndDate = endDate,
                Reason = reason,
                RequestedDate = DateTime.Now,
                Status = AbsenceStatus.Pending
            };
            _context.Absences.Add(absence);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Žádost o absenci byla odeslána ke schválení.";
            return RedirectToAction(nameof(Index));
        }
    }
}