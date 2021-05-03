using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _enviroment;
        public EmployeeController(AppDbContext db, IWebHostEnvironment env)
        {
            _dbContext = db;
            _enviroment = env;
        }
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var table = await _dbContext.Employee
                .Select(e => new 
                { 
                    EmployeeId = e.EmployeeId, 
                    EmployeeName = e.EmployeeName, 
                    Department = e.Department, 
                    DateOfJoining = e.DateOfJoining.ToString("yyyy-MM-dd"),
                    Photo = e.PhotoFileName })
                .ToListAsync();
            return new JsonResult(table);
        }

        [HttpPost]
        public async Task<JsonResult> Post(Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (employee is not null)
                {
                    Employee emp = new Employee
                    {
                        EmployeeName = employee.EmployeeName,
                        DateOfJoining = employee.DateOfJoining,
                        PhotoFileName = employee.PhotoFileName,
                        DepartmentId = employee.DepartmentId
                    };
                    await _dbContext.Employee.AddAsync(emp);
                    await _dbContext.SaveChangesAsync();

                    return new JsonResult("Added successfully");
                }
            }
            return new JsonResult("The data has been invalid");

        }
        [HttpPut]
        public async Task<JsonResult> Put(Employee employee)
        {
            if (ModelState.IsValid)
            {
                Employee emp = await _dbContext.Employee
                    .FirstOrDefaultAsync(emp => emp.EmployeeId == employee.EmployeeId);
                if (emp is null)
                {
                    return new JsonResult("Not Found");
                }
                emp.EmployeeName = employee.EmployeeName;
                emp.DepartmentId = employee.DepartmentId;               
                emp.DateOfJoining = employee.DateOfJoining;
                emp.PhotoFileName = employee.PhotoFileName;
                _dbContext.Employee.Update(emp);
                await _dbContext.SaveChangesAsync();
            }
            return new JsonResult("Updated successfully");
        }
        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                Employee emp = await _dbContext.Employee
                    .FirstOrDefaultAsync(emp => emp.EmployeeId == id);
                if (emp is null)
                {
                    return new JsonResult("Not Found");
                }
                _dbContext.Employee.Remove(emp);
                await _dbContext.SaveChangesAsync();
            }
            return new JsonResult("Deleted successfully");
        }
        [Route("SaveFile")]
        [HttpPost]
        public async Task<JsonResult> SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _enviroment.ContentRootPath + "/Photos/" + filename;
                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await postedFile.CopyToAsync(stream);
                }
                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anon.png");
            }
        }
        [Route("GetAllDepartmentNames")]
        [HttpGet]
        public async Task<JsonResult> GetAllDepartmentNames()
        {
            var table = await _dbContext.Department
              .Select(d => d.DepartmentName)
              .ToListAsync();
            return new JsonResult(table);
        }
    }
}
