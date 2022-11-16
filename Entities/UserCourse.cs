using System.ComponentModel.DataAnnotations.Schema;

namespace courseware.Entities;

public class UserCourse
{
    public Guid UserId { get; set; }
    public virtual AppUser? User { get; set; }
    
    public Guid CourseId { get; set; }
    public virtual Course? Course { get; set; }
}