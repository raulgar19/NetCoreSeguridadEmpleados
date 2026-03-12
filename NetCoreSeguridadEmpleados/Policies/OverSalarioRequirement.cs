using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace NetCoreSeguridadEmpleados.Policies
{
    public class OverSalarioRequirement : AuthorizationHandler<OverSalarioRequirement>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OverSalarioRequirement requirement)
        {
            if (context.User.HasClaim(x => x.Type == "Salario") == false)
            {
                context.Fail();
            }
            else
            {
                string data = context.User.FindFirstValue("Salario");

                int salario = int.Parse(data);

                if (salario >= 300000)
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