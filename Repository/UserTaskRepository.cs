using courseware.Data;
using courseware.Entities;

namespace courseware.Repository;

public class UserTaskRepository : GenericRepository<UserTask>, IUserTaskRepository
{
    public UserTaskRepository(AppDbContext context) : base(context) { }
}