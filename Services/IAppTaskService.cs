using courseware.Models;

namespace courseware.Services;

public interface IAppTaskService
{
    Task<Result<AppTask>> CreateAsync(AppTask model);
}