using DataTables_JQuery.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

namespace DataTables_JQuery.Controllers
{
    public class HomeController : Controller
    {
        private readonly string cadenaSQL;

        public HomeController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListaEmpleados()
        {
            List<Empleado> lista = new List<Empleado>();
            using(var conn = new SqlConnection(cadenaSQL))
            {
                conn.Open();
                var cmd = new SqlCommand("sp_lista_empleados", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                
                using(var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Empleado
                        {
                            IdEmpleado = dr["idEmpleado"].ToString(),
                            Nombre = dr["Nombre"].ToString(),
                            Cargo = dr["Cargo"].ToString(),
                            Oficina = dr["Oficina"].ToString(),
                            Salario = dr["Salario"].ToString(),
                            Telefono = dr["Telefono"].ToString(),
                            FechaIngreso = dr["FechaIngreso"].ToString()
                            

                        }) ;
                    }
                }
                return Json(new { data = lista });

            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}