using Microsoft.AspNetCore.Identity;

namespace Classroom.API.Entities;

public class AppUser : IdentityUser<Guid>
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }

    public virtual List<UserCourse>? Courses { get; set; }
}