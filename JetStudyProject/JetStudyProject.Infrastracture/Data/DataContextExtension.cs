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
                    Title = "Webinar"
                },
                new EventType
                {
                    Id = 2,
                    Title = "Course"
                },
                new EventType
                {
                    Id = 3,
                    Title = "Tutorial"
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

            builder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Title = "Software Development"
                },
                new Category
                {
                    Id = 2,
                    Title = "Design"
                },
                new Category
                {
                    Id = 3,
                    Title = "Marketing"
                },
                new Category
                {
                    Id = 4,
                    Title = "Humanitarian"
                });

            builder.Entity<Event>().HasData(
                new Event
                {
                    Id = 1,
                    Title = "UI/UX Design",
                    StartDate = new DateTime(2023, 12, 12),
                    EndDate = new DateTime(2023, 12, 12),
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
                    CreatorId = STUDENT_ID,
                    EventTypeId = 1,
                    CategoryId = 2,
                    StatusForAdministratorId = 1,
                    StatusForInstructorId = 2,
                    StatusForStudentId = 1,
                    Thumbnail = "UX-design-courses.jpg"
                },
                new Event
                {
                    Id = 2,
                    Title = "100 Days of Code: The Complete Python Pro Bootcamp for 2023",
                    StartDate = new DateTime(2023, 12, 15),
                    EndDate = new DateTime(2024, 3, 15),
                    StartTime = new TimeOnly(12, 0),
                    EndTime = new TimeOnly(14, 0),
                    Price = 150,
                    ShortDescription = "Master Python by building 100 projects in 100 days. Learn data science, automation, build websites, games and apps!",
                    LongDescription = "Welcome to the 100 Days of Code - The Complete Python Pro Bootcamp, the only course you need to learn to code with Python. With over 500,000 5 STAR reviews and a 4.8 average, my courses are some of the HIGHEST RATED courses in the history of Udemy!  \r\n\r\n100 days, 1 hour per day, learn to build 1 project per day, this is how you master Python.\r\n\r\nAt 60+ hours, this Python course is without a doubt the most comprehensive Python course available anywhere online. Even if you have zero programming experience, this course will take you from beginner to professional.",
                    TargetedAudience = "No programming experience needed - I'll teach you everything you need to know",
                    SeatsAvailable = 20,
                    Location = "imaginary link to google meet",
                    AdditionalRecources = "file.pdf",
                    IsOnline = true,
                    CreatorId = STUDENT_ID,
                    EventTypeId = 2,
                    CategoryId = 1,
                    StatusForAdministratorId = 2,
                    StatusForInstructorId = 3,
                    StatusForStudentId = 3,
                    Thumbnail = "python-logo.jpg"
                },
                new Event
                {
                    Id = 3,
                    Title = "Machine Learning A-Z™: AI, Python & R + ChatGPT Bonus [2023]",
                    StartDate = new DateTime(2023, 12, 17),
                    EndDate = new DateTime(2023, 12, 18),
                    StartTime = new TimeOnly(16, 0),
                    EndTime = new TimeOnly(20, 0),
                    Price = 200,
                    ShortDescription = "Learn to create Machine Learning Algorithms in Python and R from two Data Science experts. Code templates included.\r\n",
                    LongDescription = "Interested in the field of Machine Learning? Then this course is for you!\r\n\r\nThis course has been designed by a Data Scientist and a Machine Learning expert so that we can share our knowledge and help you learn complex theory, algorithms, and coding libraries in a simple way.\r\n\r\nOver 900,000 students world-wide trust this course.\r\n\r\nWe will walk you step-by-step into the World of Machine Learning. With every tutorial, you will develop new skills and improve your understanding of this challenging yet lucrative sub-field of Data Science.\r\n\r\nThis course can be completed by either doing either the Python tutorials, or R tutorials, or both - Python & R. Pick the programming language that you need for your career.\r\n\r\nThis course is fun and exciting, and at the same time, we dive deep into Machine Learning. It is structured the following way:",
                    TargetedAudience = "Anyone interested in Machine Learning.",
                    SeatsAvailable = 7,
                    Location = "НаУОА, Новий корпус, 101 аудиторія",
                    AdditionalRecources = "Посилання на якусь пдф-ку",
                    IsOnline = false,
                    CreatorId = ADMIN_ID,
                    EventTypeId = 3,
                    CategoryId = 1,
                    StatusForAdministratorId = 1,
                    StatusForInstructorId = 2,
                    StatusForStudentId = 1,
                    Thumbnail = "1_cG6U1qstYDijh9bPL42e-Q.jpg"
                },
                new Event
                {
                    Id = 4,
                    Title = "The Complete JavaScript Course 2024: From Zero to Expert!",
                    StartDate = new DateTime(2023, 12, 26),
                    EndDate = new DateTime(2023, 12, 27),
                    StartTime = new TimeOnly(12, 0),
                    EndTime = new TimeOnly(14, 0),
                    Price = 350,
                    ShortDescription = "The modern JavaScript course for everyone! Master JavaScript with projects, challenges and theory. Many courses in one!",
                    LongDescription = "Why is this the right JavaScript course for you?\r\n\r\nThis is the most complete and in-depth JavaScript course on Udemy (and maybe the entire internet!). It's an all-in-one package that will take you from the very fundamentals of JavaScript, all the way to building modern and complex applications.\r\n\r\nYou will learn modern JavaScript from the very beginning, step-by-step. I will guide you through practical and fun code examples, important theory about how JavaScript works behind the scenes, and beautiful and complete projects.\r\n\r\nYou will become ready to continue learning advanced front-end frameworks like React, Vue, Angular, or Svelte.\r\n\r\nYou will also learn how to think like a developer, how to plan application features, how to architect your code, how to debug code, and a lot of other real-world skills that you will need in your developer job.\r\n\r\nAnd unlike other courses, this one actually contains beginner, intermediate, advanced, and even expert topics, so you don't have to buy any other course in order to master JavaScript from the ground up!",
                    TargetedAudience = "No coding experience is necessary to take this course! I take you from beginner to expert!",
                    SeatsAvailable = 7,
                    Location = "НаУОА, Новий корпус, 101 аудиторія",
                    AdditionalRecources = "Посилання на якусь пдф-ку",
                    IsOnline = false,
                    CreatorId = INSTRUCTOR_ID,
                    EventTypeId = 2,
                    CategoryId = 1,
                    StatusForAdministratorId = 1,
                    StatusForInstructorId = 2,
                    StatusForStudentId = 1,
                    Thumbnail = "ntsnp0tdw5g9f-dowf8toer5smc.png"
                },
                new Event
                {
                    Id = 5,
                    Title = "How to Market Yourself as a Coach or Consultant",
                    StartDate = new DateTime(2023, 12, 28),
                    EndDate = new DateTime(2023, 12, 30),
                    StartTime = new TimeOnly(12, 0),
                    EndTime = new TimeOnly(14, 0),
                    Price = 100,
                    ShortDescription = "Learn a Proven, Step-by-Step Process You Can Use to Package, Brand, Market, & Sell Your Coaching or Consulting Services",
                    LongDescription = "The entire course has been completely updated and re-recorded to ensure it's current and covers everything I believe is important to successfully market an independent coaching or consulting business.\r\n\r\n------------------------------------------------------------------------------------------------------------\r\n\r\nAre you an independent Coach or Consultant who is just starting out in business?\r\n\r\nOr perhaps you've been in business for awhile but you're not attracting enough clients or making enough money?\r\n\r\nThere's a lot that goes into building a successful Coaching or Consulting business.\r\n\r\nIn this course, I'll teach you exactly what you need to KNOW and DO to successfully market yourself as an independent coach or consultant.",
                    TargetedAudience = "Independent coaches and consultants who are not reaching their business goals, or who do not know how to best market themselves.",
                    SeatsAvailable = 7,
                    Location = "НаУОА, Новий корпус, 101 аудиторія",
                    AdditionalRecources = "Посилання на якусь пдф-ку",
                    IsOnline = false,
                    CreatorId = INSTRUCTOR_ID,
                    EventTypeId = 1,
                    CategoryId = 3,
                    StatusForAdministratorId = 1,
                    StatusForInstructorId = 2,
                    StatusForStudentId = 1,
                    Thumbnail = "46538_aa04_10.jpg"
                },
                new Event
                {
                    Id = 6,
                    Title = "Google Ads for Beginners",
                    StartDate = new DateTime(2024, 01, 15),
                    EndDate = new DateTime(2024, 01, 15),
                    StartTime = new TimeOnly(12, 0),
                    EndTime = new TimeOnly(14, 0),
                    Price = 400,
                    ShortDescription = "Learn how to effectively use Google Ads to reach more customers online and grow your business.\r\n",
                    LongDescription = "In just over 3 hours you will learn how to create Google AdWords campaigns that boost traffic, increase sales and build your business online – or your money back! \r\n\r\nThroughout this comprehensive course you will learn all of the elements that go into creating campaigns that deliver a high return on every dollar you spend – from targeting, to research, to writing compelling ads, to campaign optimization. \r\n\r\nYou will also get a firm grasp on the basics of AdWords and how it works, which is necessary to create successful campaigns. And, on top of lifetime access to the course, you'll also get a FREE copy of my Google AdWords for Beginners eBook\r\n\r\nAfter completing Google AdWords for Beginners, you will be armed with the knowledge needed to launch your first campaign or drastically improve an existing one. I include detailed examples and proven strategies that I've personally used to run thousands of successful AdWords campaigns. ",
                    TargetedAudience = "Marketing Professionals",
                    SeatsAvailable = 7,
                    Location = "НаУОА, Новий корпус, 101 аудиторія",
                    AdditionalRecources = "Посилання на якусь пдф-ку",
                    IsOnline = false,
                    CreatorId = INSTRUCTOR_ID,
                    EventTypeId = 3,
                    CategoryId = 3,
                    StatusForAdministratorId = 1,
                    StatusForInstructorId = 2,
                    StatusForStudentId = 1,
                    Thumbnail = "ads.google.com_logo.png"
                },
                new Event
                {
                    Id = 7,
                    Title = "Salary Negotiation: How to Negotiate a Raise or Promotion",
                    StartDate = new DateTime(2023, 12, 12),
                    EndDate = new DateTime(2023, 12, 13),
                    StartTime = new TimeOnly(12, 0),
                    EndTime = new TimeOnly(14, 0),
                    Price = 159,
                    ShortDescription = "Get paid what you are worth. Awesome salary negotiation tips and training to get a raise or a promotion.",
                    LongDescription = "A few years back Google did something pretty stunning, surprising every one of their 25,000 employees worldwide by automatically giving them a 10% raise and a $1,000 bonus. \r\n\r\nFor the rest of us, it's not that easy. \r\n\r\nMost of us feel underpaid and undervalued at work and want to ask for a raise, but are anxious and don't know how to do it. Or maybe you've got a big performance review coming up, and you want a little help to make your case and get paid what you deserve. \r\n\r\nFortunately, there are tips and tricks that you can use to negotiate more money. ",
                    TargetedAudience = "Recommended Udemy course prior to this class: How to Negotiate Salary: The Negotiation Mindset (Free)",
                    SeatsAvailable = 7,
                    Location = "НаУОА, Новий корпус, 101 аудиторія",
                    AdditionalRecources = "Посилання на якусь пдф-ку",
                    IsOnline = false,
                    CreatorId = STUDENT_ID,
                    EventTypeId = 3,
                    CategoryId = 4,
                    StatusForAdministratorId = 1,
                    StatusForInstructorId = 2,
                    StatusForStudentId = 1,
                    Thumbnail = "How-to-Respond-to-Pushback-During-Salary-Negotiation-2-1024x512.jpg"
                },
                new Event
                {
                    Id = 8,
                    Title = "Mind Mapping Mastery –> Effective Mind Maps -> Step by Step",
                    StartDate = new DateTime(2024, 01, 04),
                    EndDate = new DateTime(2024, 01, 04),
                    StartTime = new TimeOnly(09, 0),
                    EndTime = new TimeOnly(12, 0),
                    Price = 99,
                    ShortDescription = "Mind Mapping - SMART Productivity and Memory Tool: Think Clearly, Organise, & Plan better; Study, Learn, & Recall Faster\r\n",
                    LongDescription = "Learn effective mind mapping to enhance your career and accelerate your studies at school, university, college or business school. At the same time maximising your personal development, your goal setting and planning, the preparing and completing of assignments, and much more\r\n\r\nThe course goes BEYOND Mind Mapping itself and covers both how the memory works and how to integrate mind mapping into your daily work or study. It can help you remember and recall articles, remember OTHER E-LEARNING courses, and help prepare and revise for exams etc, etc.",
                    TargetedAudience = "Aimed primarily at professionals in business but is suitable for all ages and all groups\r\n",
                    SeatsAvailable = 7,
                    Location = "НаУОА, Новий корпус, 101 аудиторія",
                    AdditionalRecources = "Посилання на якусь пдф-ку",
                    IsOnline = false,
                    CreatorId = STUDENT_ID,
                    EventTypeId = 1,
                    CategoryId = 4,
                    StatusForAdministratorId = 1,
                    StatusForInstructorId = 2,
                    StatusForStudentId = 1,
                    Thumbnail = "47285_35a2_20.jpg"
                },
                new Event
                {
                    Id = 9,
                    Title = "Photoshop CS6 Crash Course",
                    StartDate = new DateTime(2023, 12, 13),
                    EndDate = new DateTime(2023, 12, 13),
                    StartTime = new TimeOnly(11, 30),
                    EndTime = new TimeOnly(15, 29),
                    Price = 80,
                    ShortDescription = "Photoshop CS6 will be yours to command in 4 hours!\r\n",
                    LongDescription = "The three legs of Photoshop are:\r\n* Layers\r\n* Selections\r\n* Color Correction\r\nIf you understand how to manipulate those three elements you can accomplish just about anything with Photoshop. This course starts by going over those concepts and then shows how to combine them for powerful results. Rather than showing you every single feature this course focuses on the features people actually use without boring you to tears on the other tools. The instructor Jeremy Shuback has taught over 150 000 people Photoshop and works as a professional designer. He uses Photoshop every day to create everything from billboards to photorealistic matte paintings for feature films. More importantly he understands that you don't want to spend 14 hours straight trying to learn Photoshop.",
                    TargetedAudience = "Whether you're new to Photoshop or have been using it for years but were never quite comfortable with concepts such as layers, this course will get you up to speed.",
                    SeatsAvailable = 7,
                    Location = "НаУОА, Новий корпус, 101 аудиторія",
                    AdditionalRecources = "Посилання на якусь пдф-ку",
                    IsOnline = false,
                    CreatorId = INSTRUCTOR_ID,
                    EventTypeId = 3,
                    CategoryId = 2,
                    StatusForAdministratorId = 1,
                    StatusForInstructorId = 2,
                    StatusForStudentId = 1,
                    Thumbnail = "15620_896b_8.jpg"
                },
                new Event
                {
                    Id = 10,
                    Title = "The Ultimate Drawing Course - Beginner to Advanced\r\n",
                    StartDate = new DateTime(2024, 11, 10),
                    EndDate = new DateTime(2024, 11, 11),
                    StartTime = new TimeOnly(12, 0),
                    EndTime = new TimeOnly(14, 0),
                    Price = 300,
                    ShortDescription = "Learn the #1 most important building block of all art, Drawing. This course will teach you how to draw like a pro!.",
                    LongDescription = "The Ultimate Drawing Course will show you how to create advanced art that will stand up as professional work. This course will enhance or give you skills in the world of drawing - or your money back\r\n\r\nThe course is your track to obtaining drawing skills like you always knew you should have! Whether for your own projects or to draw for other people.\r\n\r\nThis course will take you from having little knowledge in drawing to creating advanced art and having a deep understanding of drawing fundamentals.\r\n\r\nSo what else is in it for you?\r\n\r\nYou’ll create over 50 different projects in this course that will take you from beginner to expert!\r\n\r\nYou’ll gain instant access to all 11 sections of the course.\r\n\r\nThe course is setup to quickly take you through step by step, the process of drawing in many different styles. It will equip you with the knowledge to create stunning designs and illustration!\r\n\r\nDon’t believe me? I offer you a full money back guarantee within the first 30 days of purchasing the course.\r\n\r\n.",
                    TargetedAudience = "Students willing to put in a couple hours to learn how to draw",
                    SeatsAvailable = 7,
                    Location = "НаУОА, Новий корпус, 101 аудиторія",
                    AdditionalRecources = "Посилання на якусь пдф-ку",
                    IsOnline = false,
                    CreatorId = ADMIN_ID,
                    EventTypeId = 2,
                    CategoryId = 2,
                    StatusForAdministratorId = 1,
                    StatusForInstructorId = 2,
                    StatusForStudentId = 1,
                    Thumbnail = "1694276_5e6f_3.jpg"
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
