using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
            var table = await _dbContext.Department.Select(d => new { d.DepartmentId, d.DepartmentName }).ToListAsync();
            return new JsonResult(table);
        }
    }
}
