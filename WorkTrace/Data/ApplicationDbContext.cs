using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WorkTrace.Models;

namespace WorkTrace.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
        public DbSet<AbsenceType> AbsenceTypes { get; set; }
        public DbSet<Absence> Absences { get; set; }
        public DbSet<EmployeeRole> EmployeeRoles { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }

        public DbSet<SystemSetting> SystemSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Employee>()
                .HasIndex(e => e.PINCode)
                .IsUnique();

            builder.Entity<AttendanceRecord>()
                .HasIndex(ar => new { ar.EmployeeId, ar.Date })
                .IsUnique();


            builder.Entity<ContractType>()
                .Property(c => c.HourlyWage)
                .HasPrecision(18, 2);


            builder.Entity<EmployeeRole>().HasData(
                new EmployeeRole { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Zaměstnanec", Description = "Běžný zaměstnanec" },
                new EmployeeRole { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Manažer", Description = "Vedoucí" },
                new EmployeeRole { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Admin", Description = "Administrátor systému" }
            );

            builder.Entity<ContractType>().HasData(
                new ContractType { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "HPP", HourlyWage = 200 },
                new ContractType { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "DPP", HourlyWage = 150 },
                new ContractType { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), Name = "DPČ", HourlyWage = 120 }
            );

            builder.Entity<AbsenceType>().HasData(
                new AbsenceType { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Dovolená", Description = "Placená dovolená" },
                new AbsenceType { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Nemoc", Description = "Nemocenská" },
                new AbsenceType { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Neplacené volno", Description = "" }
            );

            builder.Entity<SystemSetting>().HasData(
                new SystemSetting { Key = "WorkingDayHours", Value = "8", Description = "Počet hodin v pracovním dni (pro převod absencí na hodiny)" }
            );
        }
    }
}