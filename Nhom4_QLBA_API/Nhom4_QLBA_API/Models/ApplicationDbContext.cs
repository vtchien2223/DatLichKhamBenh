namespace Nhom4_QLBA_API.Models
{
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Nhom4_QLBA_API.Models;

    public class ApplicationDbContext : IdentityDbContext<User, ApplicationRole, string >
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Patients> Patients { get; set; }
        public DbSet<Doctors> Doctors { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<Specialty> Specialtys { get; set; }
        public DbSet<AppointmentDetails> AppointmentDetails { get; set; }
        public DbSet<MedicalRecordDetails> MedicalRecordDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<AppointmentDetails>()
                .HasOne(ad => ad.Doctor) // Mối quan hệ với bảng Doctors
                .WithMany() // Một doctor có thể có nhiều appointment details
                .HasForeignKey(ad => ad.DoctorId); // Khóa ngoại với bảng Doctors

            builder.Entity<AppointmentDetails>()
                .HasOne(ad => ad.Service) // Mối quan hệ với bảng Services
                .WithMany() // Một service có thể được áp dụng cho nhiều appointment details
                .HasForeignKey(ad => ad.ServiceId); // Khóa ngoại với bảng Services

            builder.Entity<AppointmentDetails>()
                .HasOne(ad => ad.Appointment) // Mối quan hệ với bảng Appointments
                .WithMany() // Một appointment có thể có nhiều appointment details
                .HasForeignKey(ad => ad.AppointmentId); // Khóa ngoại với bảng Appointments
            base.OnModelCreating(builder);
            builder.Entity<User>().Property(u => u.Initials).HasMaxLength(5);
            builder.HasDefaultSchema("identity");
        }
    }
}
