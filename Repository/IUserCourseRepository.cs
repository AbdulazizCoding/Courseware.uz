using courseware.Entities;

namespace courseware.Repository;

public interface IUserCourseRepository : IGenericRepository<UserCourse>
{
    Task AddUserCourseAsync(UserCourse entity);
}