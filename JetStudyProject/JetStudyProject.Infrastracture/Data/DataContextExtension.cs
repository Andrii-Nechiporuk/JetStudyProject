using JetStudyProject.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.Data
{
    public static class DataContextExtension
    {
        public static void Seed(this ModelBuilder builder)
        {
            string ADMIN_ROLE_ID = Guid.NewGuid().ToString();
            string INSTRUCTOR_ROLE_ID = Guid.NewGuid().ToString();
            string STUDENT_ROLE_ID = Guid.NewGuid().ToString();

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = ADMIN_ROLE_ID,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = INSTRUCTOR_ROLE_ID,
                    Name = "Instructor",
                    NormalizedName = "INSTRUCTOR"
                },
                new IdentityRole
                {
                    Id = STUDENT_ROLE_ID,
                    Name = "Student",
                    NormalizedName = "STUDENT"
                });

            string ADMIN_ID = Guid.NewGuid().ToString();
            string INSTRUCTOR_ID = Guid.NewGuid().ToString();
            string STUDENT_ID = Guid.NewGuid().ToString();

            var admin = new User
            {
                Id = ADMIN_ID,
                Name = "Андрій",
                Surname = "Нечипорук",
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                NormalizedEmail = "admin@gmail.com".ToUpper(),
                NormalizedUserName = "admin@gmail.com".ToUpper()
            };

            var instructor = new User
            {
                Id = INSTRUCTOR_ID,
                Name = "Максим",
                Surname = "Матвійчук",
                UserName = "instructor@gmail.com",
                Email = "instructor@gmail.com",
                EmailConfirmed = true,
                NormalizedEmail = "instructor@gmail.com".ToUpper(),
                NormalizedUserName = "instructor@gmail.com".ToUpper()
            };

            var student = new User
            {
                Id = STUDENT_ID,
                Name = "Вадим",
                Surname = "Верба",
                UserName = "student@gmail.com",
                Email = "student@gmail.com",
                EmailConfirmed = true,
                NormalizedEmail = "student@gmail.com".ToUpper(),
                NormalizedUserName = "student@gmail.com".ToUpper()
            };

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            admin.PasswordHash = hasher.HashPassword(admin, "admin$Pass1");
            instructor.PasswordHash = hasher.HashPassword(instructor, "instructor$Pass1");
            student.PasswordHash = hasher.HashPassword(student, "student$Pass1");

            builder.Entity<User>().HasData(admin, instructor, student);

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = ADMIN_ROLE_ID,
                    UserId = ADMIN_ID
                },
                new IdentityUserRole<string>
                {
                    RoleId = INSTRUCTOR_ROLE_ID,
                    UserId = INSTRUCTOR_ID
                }, 
                new IdentityUserRole<string>
                {
                    RoleId = STUDENT_ROLE_ID,
                    UserId = INSTRUCTOR_ID
                },
                new IdentityUserRole<string>
                {
                    RoleId = STUDENT_ROLE_ID,
                    UserId = STUDENT_ID
                });

            builder.Entity<EventType>().HasData(
                new EventType
                {
                    Id = 1,
                    Title = "Майстер клас"
                },
                new EventType
                {
                    Id = 2,
                    Title = "Лекція"
                },
                new EventType
                {
                    Id = 3,
                    Title = "Live coding"
                });

            builder.Entity<StatusForAdministrator>().HasData(
                new StatusForAdministrator
                {
                    Id = 1,
                    Title = "На модерації"
                },
                new StatusForAdministrator
                {
                    Id = 2,
                    Title = "Опубліковано"
                },
                new StatusForAdministrator
                {
                    Id = 3,
                    Title = "Відхилено"
                });
            
            builder.Entity<StatusForInstructor>().HasData(
                new StatusForInstructor
                {
                    Id = 1,
                    Title = "Чернетка"
                },
                new StatusForInstructor
                {
                    Id = 2,
                    Title = "На модерації"
                },
                new StatusForInstructor
                {
                    Id = 3,
                    Title = "Опублікоовано"
                },
                new StatusForInstructor
                {
                    Id = 4,
                    Title = "Відхилено"
                },
                new StatusForInstructor
                {
                    Id = 5,
                    Title = "Архівовано"
                });

            builder.Entity<StatusForStudent>().HasData(
                new StatusForStudent
                {
                    Id = 1,
                    Title = "Невидима"
                },
                new StatusForStudent
                {
                    Id = 2,
                    Title = "Майбутня"
                },
                new StatusForStudent
                {
                    Id = 3,
                    Title = "Триває набір"
                },
                new StatusForStudent
                {
                    Id = 4,
                    Title = "Завершена"
                });

            builder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    Title = "«UI/UX Design",
                    StartDate = new DateTime(2023, 12, 10),
                    EndDate = new DateTime(2023, 12, 11),
                    StartTime = new TimeOnly(12, 0),
                    EndTime = new TimeOnly(14, 0),
                    Price = 200,
                    ShortDescription = "Discover design fundamentals: UI components, prototyping, typography, grid, layouts and more.",
                    LongDescription = "Discover design fundamentals: UI components, prototyping, typography, grid, layouts and more.\n" +
                    "We want you to join the course prepared and move fast with the group when it starts. Reading theory and doing the tasks below will " +
                    "increase your productivity during the course and in a long run help you be employed in a better company with a higher salary.",
                    TargetedAudience = "Підходить для тих, хто хоче розпочати кар'єру в ІТ та захоплюється мистецтвом",
                    SeatsAvailable = 7,
                    Location = "НаУОА, Новий корпус, 101 аудиторія",
                    AdditionalRecources = "Посилання на якусь пдф-ку",
                    IsOnline = false,
                    CreatorId = INSTRUCTOR_ID,
                    EventTypeId = 1,
                    StatusForAdministratorId = 1,
                    StatusForInstructorId = 2,
                    StatusForStudentId = 1,
                    Thumbnail = "UX-design-courses.jpg"
                },
                new Event
                {
                    Id = 2,
                    Title = "«UI/UX Design",
                    StartDate = new DateTime(2023, 12, 10),
                    EndDate = new DateTime(2023, 12, 11),
                    StartTime = new TimeOnly(12, 0),
                    EndTime = new TimeOnly(14, 0),
                    Price = 200,
                    ShortDescription = "Discover design fundamentals: UI components, prototyping, typography, grid, layouts and more.",
                    LongDescription = "Discover design fundamentals: UI components, prototyping, typography, grid, layouts and more.\n" +
                    "We want you to join the course prepared and move fast with the group when it starts. Reading theory and doing the tasks below will " +
                    "increase your productivity during the course and in a long run help you be employed in a better company with a higher salary.",
                    TargetedAudience = "Підходить для тих, хто хоче розпочати кар'єру в ІТ та захоплюється мистецтвом",
                    SeatsAvailable = 7,
                    Location = "НаУОА, Новий корпус, 101 аудиторія",
                    AdditionalRecources = "Посилання на якусь пдф-ку",
                    IsOnline = false,
                    CreatorId = INSTRUCTOR_ID,
                    EventTypeId = 1,
                    StatusForAdministratorId = 2,
                    StatusForInstructorId = 3,
                    StatusForStudentId = 3,
                    Thumbnail = "UX-design-courses.jpg"
                });

            builder.Entity<ReadCourse>().HasData(
                new ReadCourse
                {
                    Id = 1,
                    EventId = 1,
                    UserId = INSTRUCTOR_ID
                },
                new ReadCourse
                {
                    Id = 2,
                    EventId = 1,
                    UserId = ADMIN_ID
                },
                new ReadCourse
                {
                    Id = 3,
                    EventId = 2,
                    UserId = ADMIN_ID
                });

            builder.Entity<ApplicationToEvent>().HasData(
                new ApplicationToEvent
                {
                    Id = 1,
                    EventId = 1,
                    UserId = STUDENT_ID
                }); 
            
            builder.Entity<ListenCourse>().HasData(
                new ListenCourse
                {
                    Id = 1,
                    EventId = 2,
                    UserId = STUDENT_ID
                });
        }
    }
}
