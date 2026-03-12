using Microsoft.EntityFrameworkCore;
using NetCoreSeguridadEmpleados.Data;
using NetCoreSeguridadEmpleados.Models;

namespace NetCoreSeguridadEmpleados.Repositories
{
    public class RepositoryHospital
    {
        private readonly HospitalContext context;

        public RepositoryHospital(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            return await context.Empleados.ToListAsync();
        }

        public async Task<Empleado> FindEmpleadoAsync(int id)
        {
            return await context.Empleados.FirstOrDefaultAsync(x => x.IdEmpleado == id);
        }

        public async Task<List<Empleado>> GetEmpleadosDepartamentoAsync(int idDepartamento)
        {
            return await context.Empleados.Where(x => x.IdDepartamento == idDepartamento).ToListAsync();
        }

        public async Task UpdateSalarioEmpleadosAsync(int idDepartamento, int incremento)
        {
            List<Empleado> empleados = await GetEmpleadosDepartamentoAsync(idDepartamento);

            foreach (Empleado empleado in empleados)
            {
                empleado.Salario += incremento;
            }

            await context.SaveChangesAsync();
        }

        public async Task<Empleado> LogInEmpleadoAsync(string apellido, int idEmpleado)
        {
            Empleado empleado = await this.context.Empleados.FirstOrDefaultAsync(z => z.Apellido == apellido && z.IdEmpleado == idEmpleado);

            return empleado;
        }

        public async Task DeleteEmpleadoAsync(int id)
        {
            await this.context.Empleados.Where(x => x.IdEmpleado == id).ExecuteDeleteAsync();
        }

        public async Task<List<Empleado>> GetEmpleadosSubordinadosAsync(int id)
        {
            return await this.context.Empleados.Where(x => x.Director == id).ToListAsync();
        }
    }
}