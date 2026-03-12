using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using NetCoreSeguridadEmpleados.Models;
using NetCoreSeguridadEmpleados.Repositories;
using System.Security.Claims;

namespace NetCoreSeguridadEmpleados.Policies
{
    public class DeleteEmpleadoRequirement : AuthorizationHandler<DeleteEmpleadoRequirement>, IAuthorizationRequirement
    {
        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, DeleteEmpleadoRequirement requirement)
        {
            var filterContext = context.Resource as AuthorizationFilterContext;
            var httpContext = filterContext.HttpContext;

            RepositoryHospital repo = httpContext.RequestServices.GetService<RepositoryHospital>();

            if (context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier) == false)
            {
                context.Fail();
            }
            else
            {
                string idString = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
                int id = int.Parse(idString);
                List<Empleado> subordinados = await repo.GetEmpleadosSubordinadosAsync(id);
                if (subordinados.Count > 0)
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
            return Task.CompletedTask;
        }
    }
}
