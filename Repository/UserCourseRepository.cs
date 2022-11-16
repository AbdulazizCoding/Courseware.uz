using courseware.Data;
using courseware.Entities;

namespace courseware.Repository;

public class UserCourseRepository : GenericRepository<UserCourse>, IUserCourseRepository
{
    private readonly AppDbContext _context;
    public UserCourseRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task AddUserCourseAsync(UserCourse entity)
    {
        _context.Set<UserCourse>().Add(entity);
        await _context.SaveChangesAsync();
    }
}