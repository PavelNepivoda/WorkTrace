using Microsoft.AspNetCore.Mvc;
using WorkTrace.Interfaces;
using WorkTrace.Models;

namespace WorkTrace.Controllers
{
    public class BranchesController : Controller
    {
        private readonly ICRUDRepository<Branch> _branchRepository;

        public BranchesController(ICRUDRepository<Branch> branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _branchRepository.GetAllAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            var branch = await _branchRepository.GetByIdAsync(id.Value);
            if (branch == null) return NotFound();
            return View(branch);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Branch branch)
        {
            if (ModelState.IsValid)
            {
                branch.Id = Guid.NewGuid();
                await _branchRepository.CreateAsync(branch);
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var branch = await _branchRepository.GetByIdAsync(id.Value);
            if (branch == null) return NotFound();
            return View(branch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Branch branch)
        {
            if (id != branch.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _branchRepository.UpdateAsync(branch);
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var branch = await _branchRepository.GetByIdAsync(id.Value);
            if (branch == null) return NotFound();
            return View(branch);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch != null)
                await _branchRepository.DeleteAsync(branch);
            return RedirectToAction(nameof(Index));
        }
    }
}