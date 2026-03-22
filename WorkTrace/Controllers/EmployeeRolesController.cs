using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkTrace.Interfaces;
using WorkTrace.Models;

namespace WorkTrace.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeRolesController : Controller
    {
        private readonly ICRUDRepository<EmployeeRole> _roleRepo;

        public EmployeeRolesController(ICRUDRepository<EmployeeRole> roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleRepo.GetAllAsync();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeRole role)
        {
            if (ModelState.IsValid)
            {
                role.Id = Guid.NewGuid();
                await _roleRepo.CreateAsync(role);
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var role = await _roleRepo.GetByIdAsync(id.Value);
            if (role == null) return NotFound();
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EmployeeRole role)
        {
            if (id != role.Id) return NotFound();
            if (ModelState.IsValid)
            {
                await _roleRepo.UpdateAsync(role);
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var role = await _roleRepo.GetByIdAsync(id.Value);
            if (role == null) return NotFound();
            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var role = await _roleRepo.GetByIdAsync(id);
            if (role != null) await _roleRepo.DeleteAsync(role);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            var role = await _roleRepo.GetByIdAsync(id.Value);
            if (role == null) return NotFound();
            return View(role);
        }
    }
}