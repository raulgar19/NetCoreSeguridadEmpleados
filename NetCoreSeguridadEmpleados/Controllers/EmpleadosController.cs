using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Filters;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security.Claims;

namespace NetCoreSeguridadEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryHospital repo;

        public EmpleadosController(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();

            return View(empleados);
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Details(int id)
        {
            Empleado empleado = await this.repo.FindEmpleadoAsync(id);

            return View(empleado);
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> PerfilEmpleado()
        {
            return View();
        }

        [AuthorizeEmpleados(Policy = "SOLOJEFES")]
        public async Task<IActionResult> Compis()
        {
            string dato = HttpContext.User.FindFirstValue("Departamento");
            int idDepartamento = int.Parse(dato);

            List<Empleado> empleados = await this.repo.GetEmpleadosDepartamentoAsync(idDepartamento);

            return View(empleados);
        }

        [AuthorizeEmpleados]
        [HttpPost]
        public async Task<IActionResult> Compis(int incremento)
        {
            string dato = HttpContext.User.FindFirstValue("Departamento");
            int idDepartamento = int.Parse(dato);

            await this.repo.UpdateSalarioEmpleadosAsync(idDepartamento, incremento);

            List<Empleado> empleados = await this.repo.GetEmpleadosDepartamentoAsync(idDepartamento);

            return View(empleados);
        }

        [AuthorizeEmpleados(Policy = "AdminOnly")]
        public async Task<IActionResult> AdminEmpleados()
        {
            return View();
        }

        [AuthorizeEmpleados(Policy = "SoloRicos")]
        public async Task<IActionResult> ZonaNoble()
        {
            return View();
        }

        [AuthorizeEmpleados]
        public async Task<IActionResult> Delete(int id)
        {
            await this.repo.DeleteEmpleadoAsync(id);

            return RedirectToAction("Index");
        }

        [AuthorizeEmpleados(Policy = "DeleteEmpleados")]
        public async Task<IActionResult> DeleteV2(int id)
        {
            await this.repo.DeleteEmpleadoAsync(id);

            return RedirectToAction("Index");
        }
    }
}