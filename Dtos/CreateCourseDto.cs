using System.ComponentModel.DataAnnotations;

namespace Classroom.API.Dtos;

public class CreateCourseDto
{
    [Required]
    public string? Name { get; set; }
}