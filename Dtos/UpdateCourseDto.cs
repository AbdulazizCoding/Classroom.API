using System.ComponentModel.DataAnnotations;

namespace Classroom.API.Dtos;

public class UpdateCourseDto
{
    [Required]
    public string? Name { get; set; }
}