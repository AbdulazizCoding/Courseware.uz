using courseware.Data;
using courseware.Entities;

namespace courseware.Repository;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context) : base(context) { }
}