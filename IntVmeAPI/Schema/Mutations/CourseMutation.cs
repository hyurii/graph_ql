using AppAny.HotChocolate.FluentValidation;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using IntVmeAPI.DTOs;
using IntVmeAPI.Middlewares;
using IntVmeAPI.Models;
using IntVmeAPI.Services.Courses;
using IntVmeAPI.Validators;

namespace IntVmeAPI.Schema.Mutations
{
    [ExtendObjectType(typeof(Mutation))]
    public class CourseMutation
    {
        private readonly CoursesRepository _coursesRepository;
        private readonly CourseInputValidator _courseInputValidator;

        public CourseMutation(CoursesRepository coursesRepository, CourseInputValidator courseInputValidator)
        {
            _coursesRepository = coursesRepository;
            _courseInputValidator = courseInputValidator;
        }

        [Authorize]
        [UseUser]
        public async Task<CourseResult> CreateCourse(
            [UseFluentValidation, UseValidator<CourseInputValidator>] CourseInput courseInput,
            [Service] ITopicEventSender topicEventSender,
            [User] User user)
        {
            /*var validation = _courseInputValidator.Validate(courseInput);

            if (!validation.IsValid)
            {
                throw new GraphQLException("Invalid Course Input");
            }*/

            string userId = user.Id;

            CourseDTO courseDTO = new CourseDTO()
            {
                Name = courseInput.Name,
                Subject = courseInput.Subject,
                CreatorId = userId,
                InstructorId = courseInput.InstructorId
            };

            courseDTO = await _coursesRepository.Create(courseDTO);

            CourseResult course = new CourseResult
            {
                Id = courseDTO.Id,
                Name = courseInput.Name,
                Subject = courseInput.Subject,
                InstructorId = courseInput.InstructorId
            };
            await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), course);

            return course;
        }

        [Authorize]
        [UseUser]
        public async Task<CourseResult> UpdateCourse(
            Guid Id,
            [UseFluentValidation, UseValidator<CourseInputValidator>] CourseInput courseInput,
            [Service] ITopicEventSender topicEventSender,
            [User] User user)
        {
            string userId = user.Id;

            CourseDTO currencCourseDTO = await _coursesRepository.GetById(Id);

            if (currencCourseDTO == null)
            {
                throw new GraphQLException(new Error("Course not found", "COURSE_NOT_FOUND"));
            }

            if (currencCourseDTO.CreatorId != userId)
            {
                throw new GraphQLException(new Error("You do not have permission to update this course", "INVALID_PERMISSION"));
            }

            currencCourseDTO.Name = courseInput.Name;
            currencCourseDTO.Subject = courseInput.Subject;
            currencCourseDTO.InstructorId = courseInput.InstructorId;

            currencCourseDTO = await _coursesRepository.Update(currencCourseDTO);

            CourseResult course = new CourseResult
            {
                Id = currencCourseDTO.Id,
                Name = courseInput.Name,
                Subject = courseInput.Subject,
                InstructorId = courseInput.InstructorId
            };

            string updateCourseTopic = $"{course.Id}_{nameof(Subscription.CourseUpdated)}";
            await topicEventSender.SendAsync(updateCourseTopic, course);

            return course;
        }

        [Authorize(Policy = "IsAdmin")]
        public async Task<bool> DeleteCourse(Guid Id)
        {
            try
            {
                return await _coursesRepository.Delete(Id);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
