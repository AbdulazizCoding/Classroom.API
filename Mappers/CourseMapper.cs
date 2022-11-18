using Classroom.API.Dtos;
using Classroom.API.Entities;
using Mapster;

namespace Classroom.API.Mappers;

public static class CourseMapper
{
    public static CourseDto ToDto(this Course course)
    {
        return new CourseDto
        {
            Id = course.Id,
            Name = course.Name,
            Key = course.Key,
            Users = course.Users?.Select(u => u.User?.Adapt<UserDto>()).ToList()
        };
    }
}