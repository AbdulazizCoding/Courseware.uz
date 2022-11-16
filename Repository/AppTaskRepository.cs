using courseware.Data;
using courseware.Entities;

namespace courseware.Repository;

public class AppTaskRepository : GenericRepository<AppTask>, IAppTaskRepository
{
    public AppTaskRepository(AppDbContext context) : base(context) { }
}