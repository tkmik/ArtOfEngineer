using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        //private readonly IConfiguration _configuration;
        //public DepartmentController(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}
        private readonly AppDbContext _dbContext;
        public DepartmentController(AppDbContext db)
        {
            _dbContext = db;
        }
        [HttpGet]
        public async Task<JsonResult> Get()
        {
            //string query = @"SELECT DepartmentId, DepartmentName FROM dbo.Department";
            //DataTable table = new DataTable();
            //string sqlDataSource = _configuration.GetConnectionString("Default");
            //SqlDataReader reader;
            //using (SqlConnection connection = new SqlConnection(sqlDataSource))
            //{
            //    await connection.OpenAsync();
            //    using (SqlCommand command = new SqlCommand(query, connection))
            //    {
            //        reader = command.ExecuteReader();
            //        table.Load(reader);

            //        await reader.CloseAsync();
            //        await connection.CloseAsync();
            //    }
            //}
            var table = await _dbContext.Department
                .Select(d => new { 
                    DepartmentId = d.DepartmentId, 
                    DepartmentName = d.DepartmentName })
                .ToListAsync();
            return new JsonResult(table);
        }

        [HttpPost]
        public async Task<JsonResult> Post(Department department)
        {
            //string query = @"INSERT INTO dbo.Department 
            //                VALUES ('"+department.DepartmentName+@"')";
            //DataTable table = new DataTable();
            //string sqlDataSource = _configuration.GetConnectionString("Default");
            //SqlDataReader reader;
            //using (SqlConnection connection = new SqlConnection(sqlDataSource))
            //{
            //    await connection.OpenAsync();
            //    using (SqlCommand command = new SqlCommand(query, connection))
            //    {
            //        reader = command.ExecuteReader();
            //        table.Load(reader);

            //        await reader.CloseAsync();
            //        await connection.CloseAsync();
            //    }
            //}
            if (ModelState.IsValid)
            {
                if (department is not null)
                {
                    Department dep = new Department() { DepartmentName = department.DepartmentName };
                    await _dbContext.Department.AddAsync(dep);
                    await _dbContext.SaveChangesAsync();

                    return new JsonResult("Added successfully");
                }
            }
            return new JsonResult("The data has been invalid");

        }
        [HttpPut]
        public async Task<JsonResult> Put(Department department)
        {
            //string query = @"UPDATE dbo.Department SET 
            //                DepartmentName = '"+department.DepartmentName+@"'
            //                WHERE DepartmentId = "+department.DepartmentId+@"";
            //DataTable table = new DataTable();
            //string sqlDataSource = _configuration.GetConnectionString("Default");
            //SqlDataReader reader;
            //using (SqlConnection connection = new SqlConnection(sqlDataSource))
            //{
            //    await connection.OpenAsync();
            //    using (SqlCommand command = new SqlCommand(query, connection))
            //    {
            //        reader = command.ExecuteReader();
            //        table.Load(reader);

            //        await reader.CloseAsync();
            //        await connection.CloseAsync();
            //    }
            //}
            if (ModelState.IsValid)
            {
                Department dep = await _dbContext.Department
                       .FirstOrDefaultAsync(dep => dep.DepartmentId == department.DepartmentId);
                if (dep is null)
                {
                    return new JsonResult("Not Found");
                }
                dep.DepartmentName = department.DepartmentName;
                _dbContext.Department.Update(dep);
                await _dbContext.SaveChangesAsync();
            }
            return new JsonResult("Updated successfully");
        }
        [HttpDelete("{id}")]
        public async Task<JsonResult> Delete(int id)
        {
            //string query = @"DELETE FROM dbo.Department 
            //                WHERE DepartmentId = "+id+@"";
            //DataTable table = new DataTable();
            //string sqlDataSource = _configuration.GetConnectionString("Default");
            //SqlDataReader reader;
            //using (SqlConnection connection = new SqlConnection(sqlDataSource))
            //{
            //    await connection.OpenAsync();
            //    using (SqlCommand command = new SqlCommand(query, connection))
            //    {
            //        reader = command.ExecuteReader();
            //        table.Load(reader);

            //        await reader.CloseAsync();
            //        await connection.CloseAsync();
            //    }
            //}
            if (ModelState.IsValid)
            {
                Department dep = await _dbContext.Department
                       .FirstOrDefaultAsync(dep => dep.DepartmentId == id);
                if (dep is null)
                {
                    return new JsonResult("Not Found");
                }
                _dbContext.Department.Remove(dep);
                await _dbContext.SaveChangesAsync();
            }
            return new JsonResult("Deleted successfully");
        }
    }
}
