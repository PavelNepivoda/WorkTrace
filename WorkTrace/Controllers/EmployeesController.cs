using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WorkTrace.Interfaces;
using WorkTrace.Models;

namespace WorkTrace.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeesController : Controller
    {
        private readonly ICRUDRepository<Employee> _employeeRepository;
        private readonly ICRUDRepository<Branch> _branchRepository;
        private readonly ICRUDRepository<EmployeeRole> _roleRepository;
        private readonly ICRUDRepository<ContractType> _contractTypeRepository;

        public EmployeesController(
            ICRUDRepository<Employee> employeeRepository,
            ICRUDRepository<Branch> branchRepository,
            ICRUDRepository<EmployeeRole> roleRepository,
            ICRUDRepository<ContractType> contractTypeRepository)
        {
            _employeeRepository = employeeRepository;
            _branchRepository = branchRepository;
            _roleRepository = roleRepository;
            _contractTypeRepository = contractTypeRepository;
        }

        public async Task<IActionResult> Index(Guid? branchId)
        {
            var employees = await _employeeRepository.GetAllAsync();
            var branches = (await _branchRepository.GetAllAsync()).ToList();

            if (branchId.HasValue)
            {
                employees = employees.Where(e => e.BranchId == branchId.Value);
            }

            var branchDict = branches.ToDictionary(b => b.Id);
            var roles = (await _roleRepository.GetAllAsync()).ToDictionary(r => r.Id);
            var contracts = (await _contractTypeRepository.GetAllAsync()).ToDictionary(c => c.Id);

            foreach (var emp in employees)
            {
                if (emp.BranchId.HasValue && branchDict.TryGetValue(emp.BranchId.Value, out var b))
                    emp.Branch = b;
                if (emp.RoleId.HasValue && roles.TryGetValue(emp.RoleId.Value, out var r))
                    emp.Role = r;
                if (emp.ContractTypeId.HasValue && contracts.TryGetValue(emp.ContractTypeId.Value, out var ct))
                    emp.ContractType = ct;
            }

            ViewBag.Branches = new SelectList(branches, "Id", "Name");
            ViewBag.SelectedBranchId = branchId;

            return View(employees);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();
            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null) return NotFound();
            await LoadRelatedData(employee);
            return View(employee);
        }

        public async Task<IActionResult> Create()
        {
            await SetViewBagData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (await _employeeRepository.ExistsAsync(e => e.PINCode == employee.PINCode))
                ModelState.AddModelError("PINCode", "Tento PIN kód je již používán.");

            if (ModelState.IsValid)
            {
                employee.Id = Guid.NewGuid();
                await _employeeRepository.CreateAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            await SetViewBagData();
            return View(employee);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null) return NotFound();
            await SetViewBagData();
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Employee employee)
        {
            if (id != employee.Id) return NotFound();

            if (await _employeeRepository.ExistsAsync(e => e.PINCode == employee.PINCode && e.Id != id))
                ModelState.AddModelError("PINCode", "Tento PIN kód je již používán.");

            if (ModelState.IsValid)
            {
                await _employeeRepository.UpdateAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            await SetViewBagData();
            return View(employee);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();
            var employee = await _employeeRepository.GetByIdAsync(id.Value);
            if (employee == null) return NotFound();
            await LoadRelatedData(employee);
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee != null) await _employeeRepository.DeleteAsync(employee);
            return RedirectToAction(nameof(Index));
        }

        private async Task SetViewBagData()
        {
            ViewBag.Branches = new SelectList(await _branchRepository.GetAllAsync(), "Id", "Name");
            ViewBag.Roles = new SelectList(await _roleRepository.GetAllAsync(), "Id", "Name");
            ViewBag.ContractTypes = new SelectList(await _contractTypeRepository.GetAllAsync(), "Id", "Name");
        }

        private async Task LoadRelatedData(Employee employee)
        {
            if (employee.BranchId.HasValue)
                employee.Branch = await _branchRepository.GetByIdAsync(employee.BranchId.Value);
            if (employee.RoleId.HasValue)
                employee.Role = await _roleRepository.GetByIdAsync(employee.RoleId.Value);
            if (employee.ContractTypeId.HasValue)
                employee.ContractType = await _contractTypeRepository.GetByIdAsync(employee.ContractTypeId.Value);
        }
    }
}