using courseware.Models;

namespace courseware.ViewModels;

public class CourseMultipleViewModel
{
    public List<Entities.UserCourse>? UserCourse { get; set; }
    public bool IsInValidKey { get; set; }
    public bool IsAccessDenied { get; set; }
}