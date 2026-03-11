using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security.Claims;

namespace NetCoreSeguridadEmpleados.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryHospital repo;

        public ManagedController(RepositoryHospital repo)
        {
            this.repo = repo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            int idEmpleado = int.Parse(password);
            Empleado empleado = await this.repo.LogInEmpleadoAsync(username, idEmpleado);

            if (empleado != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);

                Claim claimName = new Claim(ClaimTypes.Name, username);
                Claim claimId = new Claim(ClaimTypes.NameIdentifier, empleado.IdEmpleado.ToString());
                Claim claimRole = new Claim(ClaimTypes.Role, empleado.Oficio);
                Claim claimSalario = new Claim("Salario", empleado.Salario.ToString());
                Claim claimDept = new Claim("Departamento", empleado.IdDepartamento.ToString());

                identity.AddClaim(claimName);
                identity.AddClaim(claimId);
                identity.AddClaim(claimRole);
                identity.AddClaim(claimSalario);
                identity.AddClaim(claimDept);

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal);

                return RedirectToAction("PerfilEmpleado", "Empleados");
            }
            else
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }
    }
}