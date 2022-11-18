using System.ComponentModel.DataAnnotations.Schema;

namespace Classroom.API.Entities;

public class UserCourse
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual AppUser? User { get; set; }

    public Guid CourseId { get; set; }
    [ForeignKey(nameof(CourseId))]
    public virtual Course? Course { get; set; }

    public bool IsAdmin { get; set; }
}