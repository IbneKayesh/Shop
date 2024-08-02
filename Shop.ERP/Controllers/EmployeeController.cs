using Microsoft.AspNetCore.Mvc;
using Shop.ERP.Models;

namespace Shop.ERP.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            //create a new object
            Employee newEmp = new Employee()
            {
                Id = 1,
                EmployeeName = "Intern",
                Salary = 1000
            };
            //Method 1 :: new employee object, used for new data insert
            Employee employee = ChangeEmpName1(1, "Junior Executive", 1100);

            //Method 2 :: orginal object, used for old data modification  (not recommended)
            ChangeEmpName2(ref employee, "Executive", 1310);


            // Method 3 :: Recommended method, used for old data modification
            ChangeEmpName3(employee, "Senior Executive", 1510);

            return View();
        }


        // Method 1: Returning a new object
        // Creates a new Employee object, copies the Name, Salary property (and potentially other properties) from the original object, and assigns the new name.
        public Employee ChangeEmpName1(int newId, string newName, int newSalary)
        {
            Employee newEmp = new Employee();
            newEmp.Id = newId;
            newEmp.EmployeeName = newName;
            newEmp.Salary = newSalary;
            return newEmp;
        }

        // Method 2: Using ref parameter, without returning object
        // Directly modifies the Name, Salary property of the original Employee object passed by reference.
        public void ChangeEmpName2(ref Employee e, string newName, int newSalary)
        {
            e.EmployeeName = newName;
            e.Salary = newSalary;
        }


        // Recommended method: Using setter, without returning object
        // Uses the setter of the Name, Salary property to modify the object.
        public void ChangeEmpName3(Employee e, string newName, int newSalary)
        {
            e.EmployeeName = newName;
            e.Salary = newSalary;
        }

    }
}
