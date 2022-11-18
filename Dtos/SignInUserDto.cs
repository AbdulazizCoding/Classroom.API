using System.ComponentModel.DataAnnotations;

namespace Classroom.API.Dtos;

public class SignInUserDto
{
    [Required]
    public string? UserName { get; set; }
    
    [Required]
    public string? Password { get; set; }
}