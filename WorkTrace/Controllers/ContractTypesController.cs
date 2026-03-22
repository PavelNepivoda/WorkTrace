using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkTrace.Interfaces;
using WorkTrace.Models;

namespace WorkTrace.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContractTypesController : Controller
    {
        private readonly ICRUDRepository<ContractType> _contractRepo;

        public ContractTypesController(ICRUDRepository<ContractType> contractRepo)
        {
            _contractRepo = contractRepo;
        }

        public async Task<IActionResult> Index()
        {
            var contracts = await _contractRepo.GetAllAsync();
            return View(contracts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContractType contract)
        {
            if (ModelState.IsValid)
            {
                contract.Id = Guid.NewGuid();
                await _contractRepo.CreateAsync(contract);
                return RedirectToAction(nameof(Index));
            }
            return View(contract);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var contract = await _contractRepo.GetByIdAsync(id.Value);
            if (contract == null) return NotFound();
            return View(contract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ContractType contract)
        {
            if (id != contract.Id) return NotFound();
            if (ModelState.IsValid)
            {
                await _contractRepo.UpdateAsync(contract);
                return RedirectToAction(nameof(Index));
            }
            return View(contract);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var contract = await _contractRepo.GetByIdAsync(id.Value);
            if (contract == null) return NotFound();
            return View(contract);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var contract = await _contractRepo.GetByIdAsync(id);
            if (contract != null) await _contractRepo.DeleteAsync(contract);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            var contract = await _contractRepo.GetByIdAsync(id.Value);
            if (contract == null) return NotFound();
            return View(contract);
        }
    }
}