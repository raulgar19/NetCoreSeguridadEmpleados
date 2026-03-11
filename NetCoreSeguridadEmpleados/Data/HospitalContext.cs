using Microsoft.EntityFrameworkCore;
using NetCoreSeguridadEmpleados.Models;

namespace NetCoreSeguridadEmpleados.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options) { }

        public DbSet<Empleado> Empleados { get; set; }
    }
}