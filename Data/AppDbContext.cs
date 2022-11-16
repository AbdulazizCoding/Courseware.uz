using courseware.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace courseware.Data;

public class AppDbContext : IdentityDbContext<AppUser, AppUserRole, Guid>
{
    public DbSet<Course>? Courses { get; set; }
    public DbSet<UserCourse>? UserCourses { get; set; }
    public DbSet<AppTask>? Tasks { get; set; }
    public DbSet<UserTask>? UserTasks { get; set; }
    public DbSet<UserTaskComment>? UserTaskComments { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserCourse>()
            .HasKey(uc => new { uc.UserId, uc.CourseId });
        builder.Entity<UserCourse>()
            .HasOne(uc => uc.User)
            .WithMany(b => b.UserCourses)
            .HasForeignKey(uc => uc.UserId);
        builder.Entity<UserCourse>()
            .HasOne(uc => uc.Course)
            .WithMany(c => c.UserCourses)
            .HasForeignKey(uc => uc.CourseId);

        List<string> roleNames = new() {"Admin","Teacher","Student"};
        
        foreach(var roleName in roleNames)
            builder.Entity<AppUserRole>().HasData(new AppUserRole()
            {
                Id = Guid.NewGuid(),
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            });
    }
}