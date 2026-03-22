using Microsoft.AspNetCore.Mvc;
using WorkTrace.Interfaces;
using WorkTrace.Models;

namespace WorkTrace.Controllers
{
    public class AbsenceTypesController : Controller
    {
        private readonly ICRUDRepository<AbsenceType> _typeRepository;

        public AbsenceTypesController(ICRUDRepository<AbsenceType> typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _typeRepository.GetAllAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            var type = await _typeRepository.GetByIdAsync(id.Value);
            if (type == null) return NotFound();
            return View(type);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AbsenceType type)
        {
            if (ModelState.IsValid)
            {
                type.Id = Guid.NewGuid();
                await _typeRepository.CreateAsync(type);
                return RedirectToAction(nameof(Index));
            }
            return View(type);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var type = await _typeRepository.GetByIdAsync(id.Value);
            if (type == null) return NotFound();
            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AbsenceType type)
        {
            if (id != type.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _typeRepository.UpdateAsync(type);
                return RedirectToAction(nameof(Index));
            }
            return View(type);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var type = await _typeRepository.GetByIdAsync(id.Value);
            if (type == null) return NotFound();
            return View(type);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var type = await _typeRepository.GetByIdAsync(id);
            if (type != null)
                await _typeRepository.DeleteAsync(type);
            return RedirectToAction(nameof(Index));
        }
    }
}