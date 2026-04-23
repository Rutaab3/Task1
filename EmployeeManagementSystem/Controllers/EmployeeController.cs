using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicatonDbContext _context;

        public EmployeesController(ApplicatonDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //Get All Employees from the database
            // Employees = table data from the DbContext
            //ToList() = convert data into a list(collection)
            var Employees = _context.Employees.ToList();

            return View(Employees);
        }

    }
}

