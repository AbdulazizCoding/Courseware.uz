using courseware.Data;
using courseware.Entities;

namespace courseware.Repository;

public class UserTaskCommentRepository : GenericRepository<UserTaskComment>, IUserTaskCommentRepository
{
    public UserTaskCommentRepository(AppDbContext context) : base(context) { }
}