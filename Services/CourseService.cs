using courseware.Models;
using courseware.Repository;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace courseware.Services;

public class CourseService : ICourseService
{
    private readonly ILogger<CourseService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Entities.AppUser> _userManager;

    public CourseService(
        ILogger<CourseService> logger,
        IUnitOfWork unitOfWork,
        UserManager<Entities.AppUser> userManager)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<Result<Entities.Course>> CreateAsync(Models.Course model, string? ownerName)
    {
        if(model is null) throw new ArgumentNullException(nameof(model));

        var owner = _userManager.Users.FirstOrDefault(u => u.UserName == ownerName);
        if(owner is null) return new("User is invalid.");

        var entity = model.Adapt<Entities.Course>();

        try
        {
            Random rnd = new Random();
            entity.CreatedBy = owner.Id;
            entity.SecurityKey = Guid.NewGuid().ToString("N").Substring(0, 7);
            entity.CreatedAt = DateTime.UtcNow;
            entity.ImageType = (uint)rnd.Next(0, 9);
            entity.Status = ToEntity(Models.ECourseStatus.Created);

            var createdCourse = await _unitOfWork.Courses.AddAsync(entity);

            var UserCourseEntity = new Entities.UserCourse()
            {
                UserId = owner.Id,
                CourseId = createdCourse.Id
            };

            await _unitOfWork.UserCourses.AddUserCourseAsync(UserCourseEntity);
            _unitOfWork.Save();

            return new(true) { Data = createdCourse };
        }
        catch (Exception exception)
        {
            _logger.LogError($"Error occured at {nameof(CourseService)}", exception);
            throw new("Couldn't create Course. Contact support.", exception);
        }
    }

    public async Task<bool> Exists(Guid id)
    {
        var existedCourseResult = await GetCourseByIdAsync(id);
        return existedCourseResult.IsSuccess;
    }

    public async Task<Result<IEnumerable<Entities.Course>>> GetAllCoursesAsync()
    {
        var existingCourses = _unitOfWork.Courses.GetAll();
        if(existingCourses is null) return new("No courses found.");

        try
        {
            var selectedCourses = await existingCourses.ToListAsync();
            return new(true) { Data = selectedCourses };
        }
        catch (Exception exception)
        {
            _logger.LogError($"Error occured at {nameof(CourseService)}", exception);
            throw new($"Couldn't get Courses. Contact support.", exception);
        }
    }

    public async Task<Result<Entities.Course>> GetCourseByIdAsync(Guid id)
    {
        try
        {
            var existingCourse = await _unitOfWork.Courses.GetAll().FirstOrDefaultAsync(s => s.Id == id);
            if(existingCourse is null) return new("Course with given id not found.");

            return new(true) { Data = existingCourse };
        }
        catch (Exception exception)
        {
            _logger.LogError($"Error occured at {nameof(CourseService)}", exception);
            throw new($"Couldn't get Courses. Contact support.", exception);
        }
    }

    public async Task<Result<Entities.Course>> GetCourseByKeyAsync(string? secretKey)
    {
        if(string.IsNullOrWhiteSpace(secretKey)) return new("Key is invalid.");

        try
        {
            var existingCourse = await _unitOfWork.Courses.GetAll().FirstOrDefaultAsync(s => s.SecurityKey == secretKey);
            if(existingCourse is null) return new("Course with given KEY not found.");

            return new(true) { Data = existingCourse };
        }
        catch (Exception exception)
        {
            _logger.LogError($"Error occured at {nameof(CourseService)}", exception);
            throw new($"Couldn't get Courses. Contact support.", exception);
        }
    }

    public async Task<Result<Entities.Course>> RemoveByIdAsync(Guid id)
    {
        try
        {
            var existingCourse = await _unitOfWork.Courses.GetAll().FirstOrDefaultAsync(s => s.Id == id);
            if(existingCourse is null) return new("Course with given id not found.");

            var removedResult = await _unitOfWork.Courses.RemoveAsync(existingCourse);
            if(removedResult is null) return new("Removing the Course failed. Contact support.");

            return new(true) { Data = removedResult };
        }
        catch (Exception exception)
        {
            _logger.LogError($"Error occured at {nameof(CourseService)}", exception);
            throw new($"Couldn't get Courses. Contact support.", exception);
        }
    }
    
    public async Task<Result<Entities.Course>> UpdateAsync(Guid id, Models.Course model)
    {
        if(model is null) throw new ArgumentNullException(nameof(model));

        if(string.IsNullOrWhiteSpace(model.Name)) return new("Name is invalid.");

        var existingCourse = _unitOfWork.Courses.GetById(id);
        if(existingCourse is null) return new("Course not found.");

        try
        {
            var updatedCourse = await _unitOfWork.Courses.UpdateAsync(existingCourse);
            if(updatedCourse is null) return new("Updating course failed.");

            return new(true) { Data = updatedCourse };
        }
        catch (Exception exception)
        {
            _logger.LogError($"Error occured at {nameof(CourseService)}", exception);
            throw new($"Couldn't get Courses. Contact support.", exception);
        }
    }

    public void Save() => _unitOfWork.Save();


    private Entities.ECourseStatus ToEntity(Models.ECourseStatus status)
    => status switch
    {
        Models.ECourseStatus.Created => Entities.ECourseStatus.Created,
        Models.ECourseStatus.Deleted => Entities.ECourseStatus.Deleted,
        _ => Entities.ECourseStatus.Active
    };
}